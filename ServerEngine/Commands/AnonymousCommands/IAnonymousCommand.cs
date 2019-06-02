using System.Collections.Generic;

namespace ServerEngine.Commands.AnonymousCommands
{
    /// <summary>
    /// Anonymous commands can be run by any connected client and require no authentication or authorization to run.
    /// The only thing required is a connected client.
    /// </summary>
    internal interface IAnonymousCommand
    {
        List<string> ActivatingWords { get; }

        void Execute(List<string> extraWords, AttachedClient executingClient);
    }
}
