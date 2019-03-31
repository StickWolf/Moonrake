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
            // TODO: The inventory command should simply list what the player has in their inventory and how much of each item.
        }

        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "inv", "inventory" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
