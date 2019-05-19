using System.Collections.Generic;

namespace GameClient.Commands
{
    public interface IClientCommand
    {
        List<string> ActivatingWords { get; }

        void Execute(List<string> extraWords);
    }
}
