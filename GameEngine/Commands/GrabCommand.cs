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
            var roomItems = GameState.CurrentGameState.GetLocationItems(playersLoc);
            
            if (roomItems.Keys.Count == 0)
            {
                Console.WriteLine("There is nothing to grab");
                return;
            }

            
            var itemToPickUp = Console.Choose("What do you want to pick up?", roomItems.Keys.ToList());
            var itemAmount = roomItems[itemToPickUp];
            GameState.CurrentGameState.TryAddCharacterItemCount("Player", itemToPickUp, itemAmount, engine.GameData);
            roomItems.Remove(itemToPickUp);
        }

        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "grab", "take" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
