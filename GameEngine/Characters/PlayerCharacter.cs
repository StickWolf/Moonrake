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
            this.SendMessage(">", false);
            input = Console.ReadLine();
            SendMessage();
            var extraWords = new List<string>(input.Split(' '));
            var word = extraWords[0];
            extraWords.RemoveAt(0);

            // Look for internal commands to run
            if (CommandHelper.TryRunInternalCommand(word, extraWords, engine, this))
            {
                return;
            }

            // Then look for public commands to run
            if (CommandHelper.TryRunPublicCommand(word, extraWords, this))
            {
                return;
            }

            SendMessage($"I don't know what you mean by '{word}'.");
        }
    }
}
