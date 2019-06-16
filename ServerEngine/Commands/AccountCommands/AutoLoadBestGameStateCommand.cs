using ServerEngine.GrainInterfaces;
using System.Collections.Generic;
using System.Linq;

namespace ServerEngine.Commands.AccountCommands
{
    internal class AutoLoadBestGameStateCommand : IAccountCommand
    {
        public List<string> ActivatingWords => new List<string>() { "autoloadbestgamestate" };

        public string PermissionNeeded => "Sysop";

        public void Execute(List<string> extraWords, IAccountGrain executingAccount)
        {
            try
            {
                var validSlotNames = GameState.GetValidSaveSlotNames();
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
