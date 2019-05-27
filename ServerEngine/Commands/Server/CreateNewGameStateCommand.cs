using ServerEngine.Characters.Behaviors;
using ServerEngine.Commands.Public;
using System.Collections.Generic;

namespace ServerEngine.Commands.Internal
{
    internal class CreateNewGameStateCommand : ICommandServer
    {
        public List<string> ActivatingWords => new List<string>() { "createnewgamestate" };

        public void Execute(List<string> extraWords, Client executingClient)
        {
            // Create a new game
            GameState.CreateNewGameState();

            // Add built-in things
            GameState.CurrentGameState.AddTurnBehavior(BuiltInTurnBehaviors.FocusedPlayer, new TurnBehaviorFocusedPlayer());
            GameState.CurrentGameState.AddTurnBehavior(BuiltInTurnBehaviors.Random, new TurnBehaviorRandom());
            PublicCommandHelper.AddPublicCommandsToGameState();

            // TODO: remove hack after this can be done from the client proper
            GameState.CurrentGameState.CreateAccount("ServerUser");

            // Have the game fill in its game data
            EngineInternal.NewWorldCreator();
        }
    }
}
