using System.Collections.Generic;

namespace GameEngine.Commands
{
    internal interface ICommand
    {
        bool IsActivatedBy(string word);

        void Exceute(EngineInternal engine, List<string> extraWords);
    }
}
