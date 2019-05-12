using System.Collections.Generic;

namespace GameEngine.Commands.Internal
{
    internal class LoadCommand : ICommandServer
    {
        public List<string> ActivatingWords => new List<string>() { "load" };

        public void Execute(List<string> extraWords, Client executingClient)
        {
            var validSlotNames = GameState.GetValidSaveSlotNames();
            validSlotNames.Add("Start a new game");

            bool includeCancel = false;
            if (GameState.CurrentGameState != null)
            {
                includeCancel = true;
            }

            var slotToLoad = executingClient.Choose("Load or start new game?", validSlotNames, includeCancel: includeCancel);
            if (slotToLoad == null)
            {
                executingClient.SendMessage("Canceled Load");
            }
            else if (slotToLoad.Equals("Start a new game"))
            {
                EngineInternal.StartNewGame(executingClient); // TODO: starting a new game should be a separate command from loading the game state
            }
            else
            {
                executingClient.SendMessage();
                executingClient.SendMessage($"Loading {slotToLoad}.");
                GameState.LoadGameState(slotToLoad);
                executingClient.SendMessage("Loading complete.");
            }
        }
    }
}
