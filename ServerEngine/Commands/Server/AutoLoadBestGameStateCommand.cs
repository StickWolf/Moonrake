using System.Collections.Generic;
using System.Linq;

namespace ServerEngine.Commands.Internal
{
    internal class AutoLoadBestGameStateCommand : ICommandServer
    {
        public List<string> ActivatingWords => new List<string>() { "autoloadbestgamestate" };

        public void Execute(List<string> extraWords, Client executingClient)
        {
            try
            {
                var validSlotNames = GameState.GetValidSaveSlotNames(); // TODO: make it so the most recent saved game comes out as the first item in this list
                if (validSlotNames.Count == 0)
                {
                    InternalCommandHelper.TryRunServerCommand("createnewgamestate", new List<string>(), executingClient);
                }
                else
                {
                    var slotToLoad = validSlotNames.First();
                    InternalCommandHelper.TryRunServerCommand("loadgamestate", new List<string>() { slotToLoad }, executingClient);
                }
            }
            catch
            {
                // TODO: make sure this doesn't start overwriting the older game states.
                InternalCommandHelper.TryRunServerCommand("createnewgamestate", new List<string>(), executingClient);
            }
        }
    }
}
