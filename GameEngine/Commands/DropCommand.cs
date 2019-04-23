using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class DropCommand : ICommand
    {
        public void Exceute(EngineInternal engine, List<string> extraWords)
        {
            var playersLoc = GameState.CurrentGameState.GetCharacterLocation("Player");
            var characterItems = GameState.CurrentGameState.GetCharacterItems("Player");
            if (characterItems == null || characterItems.Count == 0)
            {
                Console.WriteLine("You have nothing to drop.");
                return;
            }

            var availableItems = characterItems
                .Select(i => new Tuple<Item, int>(engine.GameData.GetItem(i.Key), i.Value))
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

            availableItems.Add("CancelChoice", "Cancel");

            var itemToDrop = Console.Choose("What do you want to drop?", availableItems);
            
            if (itemToDrop == "CancelChoice")
            {
                Console.WriteLine("Canceled Drop");
                return;
            }

            var itemAmountToDrop = characterItems[itemToDrop];
            if (itemAmountToDrop > 1)
            {
                Console.WriteLine("How many do you want to drop?");
                itemAmountToDrop = int.Parse(Console.ReadLine());
                if (itemAmountToDrop <= 0)
                {
                    return;
                }
                if (itemAmountToDrop > characterItems[itemToDrop])
                {
                    itemAmountToDrop = characterItems[itemToDrop];
                }
            }

            // Remove it from the player's inventory
            var removeCharResult = GameState.CurrentGameState.TryAddCharacterItemCount("Player", itemToDrop, -itemAmountToDrop, engine.GameData);
            // And place it on the floor
            var addLocationResult = GameState.CurrentGameState.TryAddLocationItemCount(playersLoc, itemToDrop, itemAmountToDrop, engine.GameData);
        }

        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "drop" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
