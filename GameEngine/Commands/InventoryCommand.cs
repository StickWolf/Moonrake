using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Commands
{
    internal class InventoryCommand : ICommand
    {
        public void Exceute(EngineInternal engine)
        {
            var charaterItems = GameState.CurrentGameState.GetCharacterItems("Player");
            // TODO: make the player charater name constant or global.
            if(charaterItems == null)
            {
                Console.WriteLine("You have no items");
                return;
            }
            
            foreach(var characterItem in charaterItems)
            {
                Console.WriteLine(characterItem.Key + " - " + characterItem.Value);
            }
        }

        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "inv", "inventory" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
