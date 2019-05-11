using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands.Internal
{
    internal static class InternalCommandHelper
    {
        private static List<ICommandInternal> AllInternalCommands { get; set; }

        static InternalCommandHelper()
        {
            // Internal commands gain access to the EngineInternal when they are executed
            // The intention is that internal commands are only available to the player
            AllInternalCommands = new List<ICommandInternal>()
            {
                new ClearCommand(),
                new LetPlayerChangeTheirNameCommand(),
                new LoadCommand(),
                new SaveCommand(),
                new ExitCommand(),
            };
        }

        /// <summary>
        /// Runs an internal command
        /// </summary>
        /// <param name="word">The command word</param>
        /// <param name="extraWords">Extra words to pass along to the command</param>
        /// <param name="gameData">The game source data</param>
        /// <param name="executingCharacter">The character who is running the command</param>
        /// <returns>True if the command was found and ran, false if the command was not found</returns>
        internal static bool TryRunInternalCommand(string word, List<string> extraWords, EngineInternal engine, Character executingCharacter)
        {
            var commandToRun = AllInternalCommands
                .FirstOrDefault(c => c.ActivatingWords.Any(w => w.Equals(word, StringComparison.OrdinalIgnoreCase)));
            if (commandToRun == null)
            {
                return false;
            }

            // The command is a real command if we got this far
            commandToRun.Execute(engine, extraWords, executingCharacter);
            return true;
        }
    }
}
