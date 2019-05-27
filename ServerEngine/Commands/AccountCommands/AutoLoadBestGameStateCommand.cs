using System.Collections.Generic;
using System.Linq;

namespace ServerEngine.Commands.AccountCommands
{
    internal class AutoLoadBestGameStateCommand : IAccountCommand
    {
        public List<string> ActivatingWords => new List<string>() { "autoloadbestgamestate" };

        public string PermissionNeeded => "Sysop";

        public void Execute(List<string> extraWords, Account executingAccount)
        {
            try
            {
                var validSlotNames = GameState.GetValidSaveSlotNames(); // TODO: make it so the most recent saved game comes out as the first item in this list
                if (validSlotNames.Count == 0)
                {
                    CommandRunner.TryRunCommandFromAccount("createnewgamestate", new List<string>(), executingAccount);
                }
                else
                {
                    var slotToLoad = validSlotNames.First();
                    CommandRunner.TryRunCommandFromAccount("loadgamestate", new List<string>() { slotToLoad }, executingAccount);
                }
            }
            catch
            {
                // TODO: make sure this doesn't start overwriting the older game states.
                CommandRunner.TryRunCommandFromAccount("createnewgamestate", new List<string>(), executingAccount);
            }
        }
    }
}
