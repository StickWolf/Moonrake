using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Commands
{
    class GrabCommand : ICommand
    {
        public void Exceute(EngineInternal engine)
        {
            var playersLoc = GameState.CurrentGameState.CharacterLocations["Player"];
            var locationItems = GameState.CurrentGameState.GetLocationItems(playersLoc);
            
            if (locationItems == null || locationItems.Keys.Count == 0)
            {
                Console.WriteLine("There is nothing to grab.");
                return;
            }

            var availableItems = locationItems.Keys.ToList();
            availableItems.Add("Cancel");
            var itemToPickUp = Console.Choose("What do you want to pick up?", availableItems);

            if(itemToPickUp == "Cancel")
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
