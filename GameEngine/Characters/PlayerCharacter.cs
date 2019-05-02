using GameEngine.Commands;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GameEngine.Characters
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PlayerCharacter : Character
    {
        public PlayerCharacter(string name, int hp) : base(name, hp)
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
            if (CommandHelper.TryRunPublicCommand(word, extraWords))
            {
                return;
            }

            Console.WriteLine($"I don't know what you mean by '{word}'.");
        }
    }
}
