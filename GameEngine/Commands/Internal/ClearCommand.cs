using GameEngine.Characters;
using System.Collections.Generic;

namespace GameEngine.Commands.Internal
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
