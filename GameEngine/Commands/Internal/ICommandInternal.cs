using GameEngine.Characters;
using System.Collections.Generic;

namespace GameEngine.Commands.Internal
{
    internal interface ICommandInternal
    {
        List<string> ActivatingWords { get; }

        void Execute(EngineInternal engine, List<string> extraWords, Character executingCharacter);
    }
}
