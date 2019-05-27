using ServerEngine.Characters;
using System.Collections.Generic;

namespace ServerEngine.Commands.GameCommands
{
    /// <summary>
    /// A game command is one that you need to be logged in as a player, playing the game to run.
    /// These commands are run as the player and take some form of action within the context of the game.
    /// </summary>
    public interface IGameCommand
    {
        List<string> ActivatingWords { get; }

        string PermissionNeeded { get; }

        void Execute(List<string> extraWords, Character executingCharacter);
    }
}
