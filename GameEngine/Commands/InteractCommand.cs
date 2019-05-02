using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class InteractCommand : ICommand
    {
        public void Exceute(List<string> extraWords)
        {
            var interactingCharacter = GameState.CurrentGameState.GetPlayerCharacter();

            var playersLoc = GameState.CurrentGameState.GetCharacterLocation(interactingCharacter.TrackingId);
            var locationItems = GameState.CurrentGameState.GetLocationItems(playersLoc.TrackingId) ?? new Dictionary<Item, int>();
            var characterItems = GameState.CurrentGameState.GetCharacterItems(interactingCharacter.TrackingId) ?? new Dictionary<Item, int>();

            var interactableLocationItems = locationItems
                .Where(i => i.Key.IsInteractable && i.Key.IsVisible) // Only choose items that are interactable and visible
                .Select(i => new KeyValuePair<Item, string>(
                                 i.Key,
                                 i.Key.GetDescription(i.Value).UppercaseFirstChar()
                                 ));

            var interactableCharacterItems = characterItems
                .Where(i => i.Key.IsInteractable && i.Key.IsVisible) // Only choose items that are interactable and visible
                .Select(i => new KeyValuePair<Item, string>(
                                 i.Key,
                                 i.Key.GetDescription(i.Value).UppercaseFirstChar()
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
            var wordItemMap = CommandHelper.WordsToItems(extraWords, allInteractableItems.Keys.ToList());
            var foundItems = wordItemMap
                .Where(i => i.Value != null)
                .Select(i => i.Value)
                .ToList();
            var item1 = foundItems.Count > 0 ? foundItems[0] : null;
            var item2 = foundItems.Count > 1 ? foundItems[1] : null;

            // If we weren't able to determine at-least the first item through extra words, then try via prompt mode
            if (item1 == null)
            {
                if (!TryGetItemsFromPrompts(allInteractableItems, out item1, out item2))
                {
                    return;
                }
            }

            // If we have just item1 then interact with just that
            if (item1 != null && item2 == null)
            {
                item1.Interact(null);
            }
            else if(item1 != null && item2 != null)
            {
                item2.Interact(item1);
            }
        }

        private bool TryGetItemsFromPrompts(Dictionary<Item, string> allInteractableItems, out Item item1, out Item item2)
        {
            var primaryItem = Console.Choose("What do you want to use?", allInteractableItems, includeCancel: true);
            if (primaryItem == null)
            {
                Console.WriteLine("Canceled interaction");
                item1 = item2 = null;
                return false;
            }
            allInteractableItems.Remove(primaryItem); // Remove the item being used

            while (true)
            {
                // If only the cancel item is left then auto-choose 'No'
                string answer = "N";
                if (allInteractableItems.Count != 1)
                {
                    Console.Write($"Do you want use the {primaryItem.DisplayName} on another item? (Yes, No or Cancel): ");
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
                    item1 = primaryItem;
                    item2 = null;
                    return true;
                }
                else if (answer.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    // Prompt for other item to interact with
                    var secondaryItem = Console.Choose($"What do you want to use the {primaryItem.DisplayName} on?", allInteractableItems, includeCancel: true);
                    if (secondaryItem == null)
                    {
                        Console.WriteLine("Canceled interaction");
                        item1 = item2 = null;
                        return false;
                    }
                    item1 = primaryItem;
                    item2 = secondaryItem;
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
