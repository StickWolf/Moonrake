using System.Collections.Generic;

namespace GameEngine.Commands
{
    internal interface ICommandInternal
    {
        bool IsActivatedBy(string word);

        void Exceute(EngineInternal engine, List<string> extraWords);
    }
}
