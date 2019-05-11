using GameEngine.Characters;
using System.Collections.Generic;

namespace GameEngine.Commands.Public
{
    public interface ICommand
    {
        List<string> ActivatingWords { get; }

        void Execute(List<string> extraWords, Character executingCharacter);
    }
}
