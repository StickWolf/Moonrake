using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class DropCommand : ICommand
    {
        public void Execute(List<string> extraWords, Guid droppingCharacterTrackingId)
        {
            // TODO: instead use the passed in tracking id
            var droppingCharacter = GameState.CurrentGameState.GetPlayerCharacter();
            var playersLoc = GameState.CurrentGameState.GetCharacterLocation(droppingCharacter.TrackingId);
            var characterItems = GameState.CurrentGameState.GetCharacterItems(droppingCharacter.TrackingId);
            if (characterItems == null || characterItems.Count == 0)
            {
                Console.WriteLine("You have nothing to drop.");
                return;
            }

            var availableItems = characterItems
                .Where(i => !i.Key.IsBound && i.Key.IsVisible) // Filter out bound and invisible items because these cannot be dropped
                .Select(i => new KeyValuePair<Item, string>(
                                 i.Key,
                                 i.Key.GetDescription(i.Value).UppercaseFirstChar()
                                 ))
                .ToDictionary(i => i.Key, i => i.Value);

            if (!availableItems.Any())
            {
                Console.WriteLine("You have nothing that can be dropped.");
                return;
            }

            // Try to auto-determine what the player is trying to drop
            var wordItemMap = CommandHelper.WordsToItems(extraWords, availableItems.Keys.ToList());
            var foundItems = wordItemMap
                .Where(i => i.Value != null)
                .Select(i => i.Value)
                .ToList();
            Item itemToDrop;
            if (foundItems.Count > 0)
            {
                itemToDrop = foundItems[0];
            }
            else
            {
                itemToDrop = Console.Choose("What do you want to drop?", availableItems, includeCancel: true);
                if (itemToDrop == null)
                {
                    Console.WriteLine("Canceled Drop");
                    return;
                }
            }

            var itemAmountToDrop = characterItems[itemToDrop];
            if (itemAmountToDrop > 1)
            {
                var leftWords = wordItemMap.Where(i => i.Value == null).Select(i => i.Key).ToList();
                var wordNumberMap = CommandHelper.WordsToNumbers(leftWords);
                var foundNumbers = wordNumberMap.Where(i => i.Value.HasValue).Select(i => i.Value.Value).ToList();
                if (foundNumbers.Count > 0)
                {
                    itemAmountToDrop = foundNumbers[0];
                }
                else
                {
                    Console.WriteLine("How many do you want to drop?");
                    itemAmountToDrop = int.Parse(Console.ReadLine());
                }

                if (itemAmountToDrop <= 0)
                {
                    return;
                }
                if (itemAmountToDrop > characterItems[itemToDrop])
                {
                    itemAmountToDrop = characterItems[itemToDrop];
                }
            }

            itemToDrop.Drop(itemAmountToDrop, droppingCharacter.TrackingId);
        }

        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "drop" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
