using ServerEngine.Characters;
using System.Collections.Generic;

namespace ServerEngine.Commands.Public
{
    public interface ICommand
    {
        List<string> ActivatingWords { get; }

        void Execute(List<string> extraWords, Character executingCharacter);
    }
}
