using GameEngine.Characters;
using System.Collections.Generic;

namespace GameEngine.Commands
{
    internal interface ICommandInternal
    {
        bool IsActivatedBy(string word);

        void Execute(EngineInternal engine, List<string> extraWords, Character executingCharacter);
    }
}
