using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Commands
{
    public class ExitCommand : ICommand
    {
        public void Exceute()
        {
            // TODO: If the user types in exit, ask them if they want to save first,
            // TODO: then set keepPlaying to false and break;
            var commandToRun = CommandHelper.GetCommand("save");
            //TODO: Implement this.
            throw new NotImplementedException();
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("exit", StringComparison.OrdinalIgnoreCase);
        }



    }
}
