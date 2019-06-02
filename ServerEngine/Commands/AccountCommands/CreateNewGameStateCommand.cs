using ServerEngine.Characters.Behaviors;
using System.Collections.Generic;

namespace ServerEngine.Commands.AccountCommands
{
    internal class CreateNewGameStateCommand : IAccountCommand
    {
        public List<string> ActivatingWords => new List<string>() { "createnewgamestate" };

        public string PermissionNeeded => "Sysop";

        public void Execute(List<string> extraWords, Account executingAccount)
        {
            // Create a new game
            GameState.CreateNewGameState();

            // Add built-in things
            GameState.CurrentGameState.AddTurnBehavior(BuiltInTurnBehaviors.Random, new TurnBehaviorRandom());

            // Have the game fill in its game data
            EngineInternal.NewWorldCreator();

            // Save the newly created state
            GameState.SaveGameState();
        }
    }
}
