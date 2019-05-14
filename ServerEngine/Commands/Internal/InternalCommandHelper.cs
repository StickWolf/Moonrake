using ServerEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerEngine.Commands.Internal
{
    internal static class InternalCommandHelper
    {
        private static List<ICommandInternal> AllInternalCommands { get; set; }
        private static List<ICommandServer> AllServerCommands { get; set; }

        static InternalCommandHelper()
        {
            // Internal commands gain access to the EngineInternal when they are executed
            // The intention is that internal commands are only available to the player
            AllInternalCommands = new List<ICommandInternal>()
            {
                new ClearCommand(),
                new LetPlayerChangeTheirNameCommand(),
                new ExitCommand(),
            };

            AllServerCommands = new List<ICommandServer>()
            {
                new CreateNewPlayerCommand(),
                new LoadGameStateCommand(),
                new SaveGameStateCommand(),
            };
        }

        /// <summary>
        /// Runs an internal command
        /// </summary>
        /// <param name="word">The command word</param>
        /// <param name="extraWords">Extra words to pass along to the command</param>
        /// <param name="executingCharacter">The character who is running the command</param>
        /// <returns>True if the command was found and ran, false if the command was not found</returns>
        internal static bool TryRunInternalCommand(string word, List<string> extraWords, Character executingCharacter)
        {
            var commandToRun = AllInternalCommands
                .FirstOrDefault(c => c.ActivatingWords.Any(w => w.Equals(word, StringComparison.OrdinalIgnoreCase)));
            if (commandToRun == null)
            {
                return false;
            }

            // The command is a real command if we got this far
            commandToRun.Execute(extraWords, executingCharacter);
            return true;
        }

        internal static bool TryRunServerCommand(string word, List<string> extraWords, Client executingClient)
        {
            var commandToRun = AllServerCommands
                .FirstOrDefault(c => c.ActivatingWords.Any(w => w.Equals(word, StringComparison.OrdinalIgnoreCase)));
            if (commandToRun == null)
            {
                return false;
            }

            // The command is a real command if we got this far
            commandToRun.Execute(extraWords, executingClient);
            return true;
        }
    }
}
