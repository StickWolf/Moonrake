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

        internal void InternalTurn(EngineInternal engine)
        {
            string input;
            Console.Write(">");
            input = Console.ReadLine();
            Console.WriteLine();
            var extraWords = new List<string>(input.Split(' '));
            var word = extraWords[0];
            extraWords.RemoveAt(0);

            // Look for internal commands to run
            if (CommandHelper.TryRunInternalCommand(word, extraWords, engine))
            {
                return;
            }

            // Then look for public commands to run
            if (CommandHelper.TryRunPublicCommand(word, extraWords, engine.GameData))
            {
                return;
            }

            Console.WriteLine($"I don't know what you mean by '{word}'.");
        }
    }
}
