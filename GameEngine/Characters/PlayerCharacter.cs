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
            Console.Clear();
            string input;
            this.SendMessage($"{Name}:", false);
            Console.WriteLine("");
            PublicCommandHelper.TryRunPublicCommand("look", new List<string>(), this);
            SendMessage("> ", false);
            input = Console.ReadLine();
            SendMessage();
            var extraWords = new List<string>(input.Split(' '));
            var word = extraWords[0];
            extraWords.RemoveAt(0);

            // Look for internal commands to run
            if (InternalCommandHelper.TryRunInternalCommand(word, extraWords, engine, this))
            {
                Console.ReadLine();
                return;
            }

            // Then look for public commands to run
            if (PublicCommandHelper.TryRunPublicCommand(word, extraWords, this))
            {
                Console.ReadLine();
                return;
            }

            SendMessage($"I don't know what you mean by '{word}'.");
        }
    }
}
