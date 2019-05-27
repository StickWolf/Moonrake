using System;
using System.Collections.Generic;
using System.Linq;

namespace GameClient.Commands
{
    public static class CommandHelper
    {
        private static List<ICommand> AllClientCommands { get; set; }

        static CommandHelper()
        {
            AllClientCommands = new List<ICommand>()
            {
                new ConnectCommand(),
                new CreateAccountCommand(),
            };
        }

        internal static void TryRunCommand(string word, List<string> extraWords)
        {
            var commandToRun = AllClientCommands
                .FirstOrDefault(c => c.ActivatingWords.Any(w => w.Equals(word, StringComparison.OrdinalIgnoreCase)));
            if (commandToRun != null)
            {
                commandToRun.Execute(extraWords);
            }
            else
            {
                var genericCommand = new GenericServerCommand();
                extraWords.Insert(0, word);
                genericCommand.Execute(extraWords);
            }
        }
    }
}
