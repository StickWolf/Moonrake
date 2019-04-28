using GameEngine.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Characters
{
    public class PlayerCharacter : Character
    {
        public static string TrackingName { get; }  = "Player";

        public PlayerCharacter(int hp) : base(TrackingName, hp)
        {
        }

        public override void Turn(EngineInternal engine)
        {
            string input;
            Console.Write(">");
            input = Console.ReadLine();
            Console.WriteLine();
            var partsOfInput = new List<string>(input.Split(' '));
            var firstWord = partsOfInput[0];

            var commandToRun = CommandHelper.GetCommand(firstWord);
            if (commandToRun == null)
            {
                Console.WriteLine($"I don't know what you mean by '{firstWord}'.");
                return;
            }

            // The command is a real command if we got this far
            partsOfInput.RemoveAt(0);
            commandToRun.Exceute(engine, partsOfInput);
        }
    }
}
