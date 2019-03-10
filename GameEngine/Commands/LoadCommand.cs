using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Commands
{
    public class LoadCommand : ICommand
    {
        public bool IsActivatedBy(string word)
        {
            return word.Equals("load", StringComparison.OrdinalIgnoreCase);
        }

        // TODO: If the user types in load and they haven't typed in a second word then
        // TODO: prompt them on what they want to load using the standard loading screen.
        // TODO: The act of loading a game should go through creating a whole new engine instance
    }
}
