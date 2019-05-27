using System.Collections.Generic;

namespace GameClient.Commands
{
    public interface ICommand
    {
        List<string> ActivatingWords { get; }

        void Execute(List<string> extraWords);
    }
}
