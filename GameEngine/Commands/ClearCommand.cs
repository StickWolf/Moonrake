using GameEngine.Characters;
using System;
using System.Collections.Generic;

namespace GameEngine.Commands
{
    internal class ClearCommand : ICommandInternal
    {
        public void Execute(EngineInternal engine, List<string> extraWords, Character clearingCharacter)
        {
            Console.Clear();
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("clear", StringComparison.OrdinalIgnoreCase);
        }
    }
}
