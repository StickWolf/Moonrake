using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class DropCommand : ICommand
    {
        public void Exceute(GameSourceData gameData, List<string> extraWords)
        {
            var droppingCharacter = GameState.CurrentGameState.GetPlayerCharacter();
            var playersLoc = GameState.CurrentGameState.GetCharacterLocation(droppingCharacter.TrackingId);
            var characterItems = GameState.CurrentGameState.GetCharacterItems(droppingCharacter.TrackingId);
            if (characterItems == null || characterItems.Count == 0)
            {
                Console.WriteLine("You have nothing to drop.");
                return;
            }

            var availableItems = characterItems
                .Select(i => new Tuple<Item, int>(gameData.GetItem(i.Key), i.Value))
                .Where(i => i.Item1 != null && !i.Item1.IsBound && i.Item1.IsVisible) // Filter out bound and invisible items because these cannot be dropped
                .Select(i => new KeyValuePair<string, string>(
                                 i.Item1.TrackingName,
                                 i.Item1.GetDescription(i.Item2, GameState.CurrentGameState).UppercaseFirstChar()
                                 ))
                .ToDictionary(i => i.Key, i => i.Value);

            if (!availableItems.Any())
            {
                Console.WriteLine("You have nothing that can be dropped.");
                return;
            }

            // Try to auto-determine what the player is trying to drop
            var wordItemMap = CommandHelper.WordsToItems(extraWords, availableItems.Keys.ToList(), gameData);
            var foundItems = wordItemMap
                .Where(i => i.Value != null)
                .Select(i => i.Value)
                .ToList();
            Item item;
            if (foundItems.Count > 0)
            {
                item = foundItems[0];
            }
            else
            {
                var itemToDrop = Console.Choose("What do you want to drop?", availableItems, includeCancel: true);
                if (itemToDrop == null)
                {
                    Console.WriteLine("Canceled Drop");
                    return;
                }

                item = gameData.GetItem(itemToDrop);
            }

            var itemAmountToDrop = characterItems[item.TrackingName];
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
                if (itemAmountToDrop > characterItems[item.TrackingName])
                {
                    itemAmountToDrop = characterItems[item.TrackingName];
                }
            }

            item.Drop(itemAmountToDrop, droppingCharacter.TrackingId, GameState.CurrentGameState);
        }

        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "drop" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
