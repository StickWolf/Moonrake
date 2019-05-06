using System;
using System.Collections.Generic;

namespace GameEngine.Commands
{
    public interface ICommand
    {
        bool IsActivatedBy(string word);

        void Execute(List<string> extraWords, Guid executingCharacterTrackingId);
    }
}
