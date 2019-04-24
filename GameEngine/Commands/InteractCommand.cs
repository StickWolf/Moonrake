using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class InteractCommand : ICommand
    {
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
            var wordItemMap = CommandHelper.WordsToItems(extraWords, allInteractableItems.Keys.ToList(), engine);
            var foundItems = wordItemMap
                .Where(i => i.Value != null)
                .Select(i => i.Value)
                .ToList();
            var item1 = foundItems.Count > 0 ? foundItems[0] : null;
            var item2 = foundItems.Count > 1 ? foundItems[1] : null;

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
