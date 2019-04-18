using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class InteractCommand : ICommand
    {
        /// <summary>
        /// If we see these words when parsing a sentence, we'll just skip over them instead of trying
        /// to match them to an item name.
        /// </summary>
        private List<string> skipWords { get; set; } = new List<string>() { "the", "on", "with", "in" };

        public void Exceute(EngineInternal engine, List<string> extraWords)
        {
            var playersLoc = GameState.CurrentGameState.GetCharacterLocation("Player");
            var locationItems = GameState.CurrentGameState.GetLocationItems(playersLoc) ?? new Dictionary<string, int>();
            var characterItems = GameState.CurrentGameState.GetCharacterItems("Player") ?? new Dictionary<string, int>();

            var interactableLocationItems = locationItems
                .Select(i => new Tuple<Item, int>(engine.GameData.GetItem(i.Key), i.Value))
                .Where(i => i.Item1 != null && i.Item1.IsInteractable && i.Item1.IsVisible) // Only choose items that are interactable and visible
                .Select(i => new KeyValuePair<string, string>(
                                 i.Item1.TrackingName,
                                 i.Item1.GetDescription(i.Item2, GameState.CurrentGameState).UppercaseFirstChar()
                                 ));

            var interactableCharacterItems = characterItems
                .Select(i => new Tuple<Item, int>(engine.GameData.GetItem(i.Key), i.Value))
                .Where(i => i.Item1 != null && i.Item1.IsInteractable && i.Item1.IsVisible) // Only choose items that are interactable and visible
                .Select(i => new KeyValuePair<string, string>(
                                 i.Item1.TrackingName,
                                 i.Item1.GetDescription(i.Item2, GameState.CurrentGameState).UppercaseFirstChar()
                                 ));

            var allInteractableItems = interactableLocationItems
                .Union(interactableCharacterItems)
                .ToDictionary(i => i.Key, i => i.Value); // This also removes duplicates

            if (!allInteractableItems.Any())
            {
                Console.WriteLine("There is nothing available to interact with.");
                return;
            }

            // Try to auto-determine the choices if extra words are typed in
            TryGetItemsFromExtraWords(extraWords, allInteractableItems, engine, out Item item1, out Item item2);

            // If we weren't able to determine at-least the first item through extra words, then try via prompt mode
            if (item1 == null)
            {
                if (!TryGetItemsFromPrompts(allInteractableItems, engine, out item1, out item2))
                {
                    return;
                }
            }

            // If we have just item1 then interact with just that
            if (item1 != null && item2 == null)
            {
                item1.Interact(GameState.CurrentGameState, null);
            }
            else if(item1 != null && item2 != null)
            {
                item2.Interact(GameState.CurrentGameState, item1.TrackingName);
            }
        }

        private void TryGetItemsFromExtraWords(List<string> extraWords, Dictionary<string, string> allInteractableItems, EngineInternal engine, out Item item1, out Item item2)
        {
            // We look for words using sentences both backwards and forwards to increase the matching logic
            // e.g. "use key in keyhole" - key is present in both of these item descriptions.
            // We aren't guaranteed that the items will be in any order when we loop through them so the first
            // one in the list may be the keyhole and we'd end up with item1=keyhole, item2=null
            // reversing the words "keyhole in key" and then trying again results in: item1=key, item2=keyhole
            // Not sure if this will work well in other cases, probably needs improvement

            var set1 = GetItemsFromExtraWords(extraWords, allInteractableItems, engine);
            extraWords.Reverse();
            var set2 = GetItemsFromExtraWords(extraWords, allInteractableItems, engine);

            if (set1.Count >= set2.Count)
            {
                item1 = set1.Count > 0 ? set1[0] : null;
                item2 = set1.Count > 1 ? set1[1] : null;
            }
            else if (set2.Count > 1)
            {
                item1 = set2[1];
                item2 = set2[0];
            }
            else if (set2.Count == 1)
            {
                item1 = set2[0];
                item2 = null;
            }
            else
            {
                item1 = null;
                item2 = null;
            }
        }

        private List<Item> GetItemsFromExtraWords(List<string> extraWords, Dictionary<string, string> allInteractableItems, EngineInternal engine)
        {
            var response = new List<Item>();
            if (!extraWords.Any())
            {
                return response;
            }

            var usedTrackingNames = new List<string>();
            var interactableItems = allInteractableItems.Keys
                .Select(i => engine.GameData.GetItem(i))
                .ToList();

            foreach (var extraWord in extraWords)
            {
                if (skipWords.Contains(extraWord.ToLower()))
                {
                    continue;
                }

                // Look through all interactable items and see if we can find one that looks right
                foreach (var item in interactableItems)
                {
                    if (usedTrackingNames.Contains(item.TrackingName))
                    {
                        continue;
                    }

                    if (item.DisplayName.ToLower().Contains(extraWord.ToLower()))
                    {
                        usedTrackingNames.Add(item.TrackingName);
                        response.Add(item);
                        break;
                    }
                }
            }

            return response;
        }

        private bool TryGetItemsFromPrompts(Dictionary<string, string> allInteractableItems, EngineInternal engine, out Item item1, out Item item2)
        {
            allInteractableItems.Add("CancelChoice", "Cancel");
            var itemToInteractWith = Console.Choose("What do you want to use?", allInteractableItems);
            if (itemToInteractWith == "CancelChoice")
            {
                Console.WriteLine("Canceled interaction");
                item1 = item2 = null;
                return false;
            }
            var item = engine.GameData.GetItem(itemToInteractWith);
            allInteractableItems.Remove(itemToInteractWith); // Remove the item being used

            while (true)
            {
                // If only the cancel item is left then auto-choose 'No'
                string answer = "N";
                if (allInteractableItems.Count != 1)
                {
                    Console.Write($"Do you want use the {item.DisplayName} on another item? (Yes, No or Cancel): ");
                    answer = Console.ReadKey().KeyChar.ToString();
                }

                Console.WriteLine();
                if (answer.Equals("C", StringComparison.OrdinalIgnoreCase))
                {
                    // Cancel
                    item1 = item2 = null;
                    return false;
                }
                else if (answer.Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    // Interact directly with the item
                    item1 = item;
                    item2 = null;
                    return true;
                }
                else if (answer.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    // Prompt for other item to interact with
                    var secondItemToInteractWith = Console.Choose($"What do you want to use the {item.DisplayName} on?", allInteractableItems);
                    if (secondItemToInteractWith == "CancelChoice")
                    {
                        Console.WriteLine("Canceled interaction");
                        item1 = item2 = null;
                        return false;
                    }
                    item1 = item;
                    item2 = engine.GameData.GetItem(secondItemToInteractWith);
                    return true;
                }
                else
                {
                    Console.WriteLine($"Unknown response: {answer}");
                }
            }
        }


        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "use", "interact" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
