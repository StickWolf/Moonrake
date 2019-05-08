using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class ExitCommand : ICommandInternal
    {
        public void Execute(EngineInternal engine, List<string> extraWords, Character exitingCharacter)
        {
            while (true)
            {
                exitingCharacter.SendMessage("Would you like to save your game first? (Yes, No or Cancel): ");
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
                    engine.RunGameLoop = false;
                    engine.RunFactory = false;
                    return;
                }
                else if (answer.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    // Save and then exit
                    CommandHelper.TryRunInternalCommand("save", new List<string>(), engine, exitingCharacter);
                    engine.RunGameLoop = false;
                    engine.RunFactory = false;
                    return;
                }
                else
                {
                    exitingCharacter.SendMessage($"Unknown response: {answer}");
                }
            }
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("exit", StringComparison.OrdinalIgnoreCase);
        }
    }
}
