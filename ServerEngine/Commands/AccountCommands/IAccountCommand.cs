using System.Collections.Generic;

namespace ServerEngine.Commands.AccountCommands
{
    /// <summary>
    /// An account command is one that you must be logged into an account to run.
    /// Needing to be logged into the game as a player is not needed for an account command.
    /// These commands usually do some sort of operation of game or system maintenance.
    /// </summary>
    internal interface IAccountCommand
    {
        List<string> ActivatingWords { get; }

        string PermissionNeeded { get; }

        void Execute(List<string> extraWords, Account executingAccount);
    }
}
