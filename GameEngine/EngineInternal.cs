using GameEngine.Characters;
using GameEngine.Characters.Behaviors;
using GameEngine.Commands.Internal;
using GameEngine.Commands.Public;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GameEngine
{
    internal static class EngineInternal
    {
        /// <summary>
        /// serverClient is the server admin window
        /// </summary>
        public static Client ServerClient { get; set; }

        /// <summary>
        /// Indicates if the main game loop should keep running game turns
        /// </summary>
        public static bool RunGameLoop { get; set; }

        /// <summary>
        /// If set to false, triggers the engine factory to stop generating new engines and exit.
        /// </summary>
        public static bool RunFactory { get; set; }

        public static Action NewGameFiller { get; set; }

        /// <summary>
        /// Runs the game until they win, die or exit.
        /// </summary>
        public static void StartEngine()
        {
            AttachedClients.DetachAllClients();
            RunGameLoop = true;
            RunFactory = true;

            // serverClient is the server admin window
            ServerClient = new Client();
            AttachedClients.AttachClient(ServerClient);
            InternalCommandHelper.TryRunServerCommand("load", new List<string>(), ServerClient);

            // Main game loop goes 1 loop for 1 game turn.
            while (RunGameLoop)
            {
                // Get all characters in the game that are still alive
                var allLocateableCharacters = GameState.CurrentGameState.GetAllCharacters();
                var sw = new Stopwatch();
                sw.Start();
                foreach (var gameCharacter in allLocateableCharacters) // TODO: Sort turn order by character speed, fastest should go first.
                {
                    // Only characters that are alive get a turn
                    if (!gameCharacter.IsDead())
                    {
                        gameCharacter.Turn();
                    }
                }
                sw.Stop();
                if (sw.Elapsed.TotalSeconds < 4)
                {
                    Task.Delay(2000).Wait();
                }

                // TODO: Respawn rules will bring NPCs back to life, maybe even player characters too
                // TODO: We should never let the client have the server exit, but a client should be able to disconnect
            }
        }

        /// <summary>
        /// Sets up everything to start a new game and shows the game introduction text
        /// </summary>
        public static void StartNewGame(Client startingNewGameClient)
        {
            // Create a new game
            GameState.CreateNewGameState();

            // Add built-in things
            GameState.CurrentGameState.AddTurnBehavior(BuiltInTurnBehaviors.FocusedPlayer, new TurnBehaviorFocusedPlayer());
            GameState.CurrentGameState.AddTurnBehavior(BuiltInTurnBehaviors.Random, new TurnBehaviorRandom());
            PublicCommandHelper.AddPublicCommandsToGameState();

            // Have the game fill in its game data
            NewGameFiller();

            // Show the intro and take a look around

            // Until we have client/server that can have the client specify what character they want to focus on,
            // we just pick the first character that has prompting behavior
            var loaderCharacter = GameState.CurrentGameState.GetAllCharacters()
                .First(c => c.HasPromptingBehaviors());

            AttachedClients.SetClientFocusedCharacter(startingNewGameClient.TrackingId, loaderCharacter.TrackingId);

            InternalCommandHelper.TryRunInternalCommand("clear", new List<string>(), loaderCharacter);
            loaderCharacter.SendMessage(GameState.CurrentGameState.GameIntroductionText);
            loaderCharacter.SendMessage();
            PublicCommandHelper.TryRunPublicCommand("look", new List<string>(), loaderCharacter);
        }
    }
}
