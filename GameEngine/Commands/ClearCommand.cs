using System;
using System.Collections.Generic;

namespace GameEngine.Commands
{
    internal class ClearCommand : ICommandInternal
    {
        public void Exceute(EngineInternal engine, List<string> extraWords)
        {
            Console.Clear();
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("clear", StringComparison.OrdinalIgnoreCase);
        }
    }
}
