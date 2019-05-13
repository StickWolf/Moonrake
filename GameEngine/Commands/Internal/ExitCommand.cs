using GameEngine.Characters;
using System;
using System.Collections.Generic;

namespace GameEngine.Commands.Internal
{
    internal class ExitCommand : ICommandInternal
    {
        public List<string> ActivatingWords => new List<string>() { "exit" };

        public void Execute(List<string> extraWords, Character exitingCharacter)
        {
            while (true)
            {
                exitingCharacter.SendMessage("Would you like to save your game first? (Yes, No or Cancel): "); // TODO: the exit command should be split out into a client command that just exits without running save.
                var answer = Console.ReadKey().KeyChar.ToString();
                exitingCharacter.SendMessage();
                if (answer.Equals("C", StringComparison.OrdinalIgnoreCase))
                {
                    // Cancel exit operation
                    return;
                }
                else if (answer.Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    // Exit without saving
                    EngineInternal.RunGameLoop = false;
                    EngineInternal.RunFactory = false;
                    return;
                }
                else if (answer.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    // Save and then exit
                    InternalCommandHelper.TryRunServerCommand("savegamestate", new List<string>(), exitingCharacter.GetClient());
                    EngineInternal.RunGameLoop = false;
                    EngineInternal.RunFactory = false;
                    return;
                }
                else
                {
                    exitingCharacter.SendMessage($"Unknown response: {answer}");
                }
            }
        }
    }
}
