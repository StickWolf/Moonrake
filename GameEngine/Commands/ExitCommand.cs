using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class ExitCommand : ICommandInternal
    {
        public void Execute(EngineInternal engine, List<string> extraWords)
        {
            while (true)
            {
                Console.Write("Would you like to save your game first? (Yes, No or Cancel): ");
                var answer = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine();
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
                    CommandHelper.TryRunInternalCommand("save", new List<string>(), engine);
                    engine.RunGameLoop = false;
                    engine.RunFactory = false;
                    return;
                }
                else
                {
                    Console.WriteLine($"Unknown response: {answer}");
                }
            }
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("exit", StringComparison.OrdinalIgnoreCase);
        }
    }
}
