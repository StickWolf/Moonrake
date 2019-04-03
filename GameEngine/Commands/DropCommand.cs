using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Commands
{
    class DropCommand : ICommand
    {
        public void Exceute(EngineInternal engine)
        {
            var characterItems = GameState.CurrentGameState.GetCharacterItems("Player");
            var playersLoc = GameState.CurrentGameState.CharacterLocations["Player"];

            if (characterItems == null || characterItems.Keys.Count == 0)
            {
                Console.WriteLine("You have nothing to drop.");
                return;
            }

            var itemToDrop = Console.Choose("What do you want to drop?", characterItems.Keys.ToList());
            var itemAmount = characterItems[itemToDrop];
            Console.WriteLine("How many do you want to drop?");
            int itemAmountToDrop = int.Parse(Console.ReadLine());
            if (itemAmountToDrop < 0)
            {
                return;
            }
            if(itemAmountToDrop > itemAmount)
            {
                itemAmountToDrop = itemAmount;
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
