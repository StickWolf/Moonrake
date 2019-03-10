using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Commands
{
    public static class CommandHelper
    {
        private static List<ICommand> AllCommands { get; set; }

        static CommandHelper()
        {
            AllCommands = new List<ICommand>()
            {
                new MoveCommand(),
                new SaveCommand(),
                new LoadCommand(),
                new ExitCommand()
            };
        }

        public static ICommand GetCommand(string word)
        {
            var commandToRun = AllCommands.FirstOrDefault(c => c.IsActivatedBy(word));
            return commandToRun;
        }
    }
}
