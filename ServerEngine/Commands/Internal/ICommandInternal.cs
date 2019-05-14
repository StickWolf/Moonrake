using ServerEngine.Characters;
using System.Collections.Generic;

namespace ServerEngine.Commands.Internal
{
    internal interface ICommandInternal
    {
        List<string> ActivatingWords { get; }

        void Execute(List<string> extraWords, Character executingCharacter);
    }
}
