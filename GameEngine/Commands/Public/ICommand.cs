using GameEngine.Characters;
using System.Collections.Generic;

namespace GameEngine.Commands.Public
{
    public interface ICommand
    {
        bool IsActivatedBy(string word);

        void Execute(List<string> extraWords, Character executingCharacter);
    }
}
