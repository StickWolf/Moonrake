using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Commands
{
    public class ExitCommand : ICommand
    {
        public bool IsActivatedBy(string word)
        {
            return word.Equals("exit", StringComparison.OrdinalIgnoreCase);
        }

        // TODO: If the user types in exit, ask them if they want to save first, then set keepPlaying to false and break;
    }
}
