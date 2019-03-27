using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Commands
{
    class ClearCommand : ICommand
    {
        void ICommand.Exceute(EngineInternal engine)
        {
            Console.Clear();
        }

        bool ICommand.IsActivatedBy(string word)
        {
            return word.Equals("clear", StringComparison.OrdinalIgnoreCase);
        }
    }
}
