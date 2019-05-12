using System.Collections.Generic;

namespace GameEngine.Commands.Internal
{
    /// <summary>
    /// Server commands run outside the context of a game. i.e. You don't need to run
    /// the command as a player. Though the command may effect the game itself.
    /// </summary>
    internal interface ICommandServer
    {
        List<string> ActivatingWords { get; }

        void Execute(List<string> extraWords, Client executingClient);
    }
}
