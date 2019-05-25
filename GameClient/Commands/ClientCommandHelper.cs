using System;
using System.Collections.Generic;
using System.Linq;

namespace GameClient.Commands
{
    public static class ClientCommandHelper
    {
        private static List<IClientCommand> AllClientCommands { get; set; }

        static ClientCommandHelper()
        {
            AllClientCommands = new List<IClientCommand>()
            {
                new ConnectCommand(),
                new CreateAccountCommand(),
            };
        }

        internal static bool TryRunClientCommand(string word, List<string> extraWords)
        {
            var commandToRun = AllClientCommands
                .FirstOrDefault(c => c.ActivatingWords.Any(w => w.Equals(word, StringComparison.OrdinalIgnoreCase)));
            if (commandToRun == null)
            {
                return false;
            }

            // The command is a real command if we got this far
            commandToRun.Execute(extraWords);
            return true;
        }
    }
}
