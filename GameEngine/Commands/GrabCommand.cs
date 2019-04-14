using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    class GrabCommand : ICommand
    {
        public void Exceute(EngineInternal engine)
        {
            var playersLoc = GameState.CurrentGameState.GetCharacterLocation("Player");
            var locationItems = GameState.CurrentGameState.GetLocationItems(playersLoc);
            if (locationItems == null || locationItems.Count == 0)
            {
                Console.WriteLine("There is nothing to grab.");
                return;
            }

            var availableItems = locationItems
                .Select(i => new Tuple<Item, int>(engine.GameData.GetItem(i.Key), i.Value))
                .Where(i => i.Item1 != null && !i.Item1.IsBound) // Filter out bound items because these cannot be picked up
                .Select(i => new KeyValuePair<string, string>(
                                 i.Item1.TrackingName,
                                 i.Item1.GetDescription(i.Item2, engine.GameData, GameState.CurrentGameState).UppercaseFirstChar()
                                 ))
                .ToDictionary(i => i.Key, i => i.Value);

            if (!availableItems.Any())
            {
                Console.WriteLine("There is nothing that can be grabed.");
                return;
            }

            availableItems.Add("CancelChoice", "Cancel");

            var itemToPickUp = Console.Choose("What do you want to pick up?", availableItems);

            if(itemToPickUp == "CancelChoice")
            {
                Console.WriteLine("Canceled Grab");
                return;
            }
            var itemAmount = locationItems[itemToPickUp];

            // Remove it from the floor
            var removeLocationResult = GameState.CurrentGameState.TryAddLocationItemCount(playersLoc, itemToPickUp, -itemAmount, engine.GameData);
            // And place it in the player's inventory
            var addCharResult = GameState.CurrentGameState.TryAddCharacterItemCount("Player", itemToPickUp, itemAmount, engine.GameData);
        }

        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "grab", "take" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
