using GameEngine.Characters.Behaviors;
using GameEngine.Commands.Public;
using System.Collections.Generic;

namespace GameEngine.Commands.Internal
{
    internal class LoadGameStateCommand : ICommandServer
    {
        public List<string> ActivatingWords => new List<string>() { "loadgamestate" };

        public void Execute(List<string> extraWords, Client executingClient)
        {
            var validSlotNames = GameState.GetValidSaveSlotNames();
            validSlotNames.Add("Start a new game state");

            var slotToLoad = executingClient.Choose("Please choose a game state to load", validSlotNames, includeCancel: true);
            if (slotToLoad == null)
            {
                executingClient.SendMessage("Canceled game state load");
            }
            else if (slotToLoad.Equals("Start a new game state"))
            {
                // Create a new game
                GameState.CreateNewGameState();

                // Add built-in things
                GameState.CurrentGameState.AddTurnBehavior(BuiltInTurnBehaviors.FocusedPlayer, new TurnBehaviorFocusedPlayer());
                GameState.CurrentGameState.AddTurnBehavior(BuiltInTurnBehaviors.Random, new TurnBehaviorRandom());
                PublicCommandHelper.AddPublicCommandsToGameState();

                // Have the game fill in its game data
                EngineInternal.NewWorldCreator();
            }
            else
            {
                executingClient.SendMessage();
                executingClient.SendMessage($"Loading game state {slotToLoad}.");
                GameState.LoadGameState(slotToLoad);
                executingClient.SendMessage("Loading game state complete.");
            }
        }
    }
}
