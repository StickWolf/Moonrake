using ServerEngine.Characters;
using System.Collections.Generic;

namespace ServerEngine.Commands.Internal
{
    internal class ClearCommand : ICommandInternal
    {
        public List<string> ActivatingWords => new List<string>() { "clear" };

        public void Execute(List<string> extraWords, Character clearingCharacter)
        {
            Console.Clear();
        }
    }
}
