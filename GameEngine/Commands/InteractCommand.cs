using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class InteractCommand : ICommand
    {
        public void Execute(List<string> extraWords, Character interactingCharacter)
        {
            var interactingCharacterLocation = GameState.CurrentGameState.GetCharacterLocation(interactingCharacter.TrackingId);
            var locationItems = GameState.CurrentGameState.GetLocationItems(interactingCharacterLocation.TrackingId) ?? new Dictionary<Item, int>();
            var characterItems = GameState.CurrentGameState.GetCharacterItems(interactingCharacter.TrackingId) ?? new Dictionary<Item, int>();

            var useableLocationItems = locationItems
                .Where(i => i.Key.IsVisible) // Only allow interaction with visible items
                .Where(i => i.Key.IsUseableFrom == ItemUseableFrom.All || i.Key.IsUseableFrom == ItemUseableFrom.Location) // Only choose items that can be used
                .Select(i => new KeyValuePair<Item, string>(
                                 i.Key,
                                 i.Key.GetDescription(i.Value).UppercaseFirstChar()
                                 ));

            var useableCharacterItems = characterItems
                .Where(i => i.Key.IsVisible) // Only allow interaction with visible items
                .Where(i => i.Key.IsUseableFrom == ItemUseableFrom.All || i.Key.IsUseableFrom == ItemUseableFrom.Inventory) // Only choose items that can be used
                .Select(i => new KeyValuePair<Item, string>(
                                 i.Key,
                                 i.Key.GetDescription(i.Value).UppercaseFirstChar()
                                 ));

            // This represents all items that can be used (either directory or indirectly)
            var allUseableItems = useableLocationItems
                .Union(useableCharacterItems)
                .ToDictionary(i => i.Key, i => i.Value); // This also removes duplicates

            // This represnts only primary interaction items
            var allPrimaryItems = allUseableItems
                .Where(i => i.Key.IsInteractionPrimary)
                .ToDictionary(i => i.Key, i => i.Value); // This also removes duplicates

            // The end goal of interact is to interact with a primary item,
            // so if none of those exist here, we are done.
            if (!allPrimaryItems.Any())
            {
                interactingCharacter.SendMessage("There is nothing available to interact with.");
                interactingCharacterLocation.SendMessage($"{interactingCharacter.Name} is looking around for something.", interactingCharacter);
                return;
            }

            Item primaryItem = null;
            Item secondaryItem = null;

            // If there were extra words typed in, then attempt to auto-map them
            if (extraWords.Count > 0)
            {
                // Try to auto-determine the choices if extra words are typed in
                var wordItemMap = CommandHelper.WordsToItems(extraWords, allUseableItems.Keys.ToList());
                var foundItems = wordItemMap
                    .Where(i => i.Value != null)
                    .Select(i => i.Value)
                    .ToList();

                // The primary item is considered the last item in the sentence that is a primary interaction item
                primaryItem = foundItems.LastOrDefault(i => i.IsInteractionPrimary);

                // This example contains only 1 item that has its primary property set (keyhole)
                // Considering that a player could type sentences like this
                //     use key on keyhole       - This syntax works also when there are 2 primaries involved
                //     use keyhole with key     - This syntax won't work with 2 primaries, it will get them backwards
                //                                For this reason, we disable this pattern all-together to avoid confusion
                // In cases where we are working with 2 primary items, the code can't really determine if the first
                // or the second is the primary. It works fine with 1 primary only both ways, but then messes up when there 
                // are two primaries. Therefore to make it so sentences always produce a dependable result we make the
                // rule that a secondary item MUST come before the primary item in the sentence.
                // We only set secondary if a primary item was found though
                if (primaryItem != null)
                {
                    foreach (var foundItem in foundItems)
                    {
                        // We reached the primary item, stop looking as it is invalid
                        // for the secondary item to exist past the primary
                        if (foundItem.TrackingId == primaryItem.TrackingId)
                        {
                            break;
                        }
                        secondaryItem = foundItem;
                        break;
                    }
                }

                if (
                    primaryItem == null || // extra words were typed, but no primary found
                    (primaryItem != null && secondaryItem == null && foundItems.Count > 1) || // the extra words looked like more items than what we could map to
                    (primaryItem != null && secondaryItem != null && foundItems.Count > 2) // the extra words looked like more items than what we could map to
                    )
                {
                    interactingCharacter.SendMessage("I don't quite understand, make sure the target item you want to use is last in the sentence. Or just type 'use' alone to enter prompt mode.");
                    return;
                }

                // Otherwise the extra words seem to have translated into the items to use properly
                primaryItem.Interact(secondaryItem, interactingCharacter);
            }
            // No extra words were typed, process this command via prompt mode
            else if (TryGetItemsFromPrompts(allUseableItems, out primaryItem, out secondaryItem, interactingCharacter))
            {
                primaryItem.Interact(secondaryItem, interactingCharacter);
            }
        }

        private bool TryGetItemsFromPrompts(Dictionary<Item, string> allUseableItems, out Item primaryItem, out Item secondaryItem, Character interactingCharacter)
        {
            // This represents only primary interaction items
            var allPrimaryItems = allUseableItems
                .Where(i => i.Key.IsInteractionPrimary)
                .ToDictionary(i => i.Key, i => i.Value); // This also removes duplicates

            primaryItem = interactingCharacter.Choose("What do you want to use?", allPrimaryItems, includeCancel: true);
            if (primaryItem == null)
            {
                interactingCharacter.SendMessage("Canceled interaction");
                primaryItem = secondaryItem = null;
                return false;
            }
            allUseableItems.Remove(primaryItem); // Remove the primary item being used
            // If the primary item was the only one available then use just that
            if (allUseableItems.Count == 0)
            {
                secondaryItem = null;
                return true;
            }

            // Determine the secondary item to use
            Console.Write($"Do you want use another item on the {primaryItem.DisplayName}? (Yes, No or Cancel): ");
            while (true)
            {
                string answer = Console.ReadKey().KeyChar.ToString();
                interactingCharacter.SendMessage();
                if (answer.Equals("C", StringComparison.OrdinalIgnoreCase))
                {
                    // Cancel
                    primaryItem = secondaryItem = null;
                    return false;
                }
                else if (answer.Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    // Interact directly with the primary item without a secondary
                    secondaryItem = null;
                    return true;
                }
                else if (answer.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    // Prompt for other item to interact with
                    secondaryItem = interactingCharacter.Choose($"What item do you want to use on the {primaryItem.DisplayName}?", allUseableItems, includeCancel: true);
                    if (secondaryItem == null)
                    {
                        interactingCharacter.SendMessage("Canceled interaction");
                        primaryItem = secondaryItem = null;
                        return false;
                    }
                    return true;
                }
                else
                {
                    interactingCharacter.SendMessage($"Unknown response: {answer}");
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
