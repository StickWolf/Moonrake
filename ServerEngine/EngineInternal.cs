using ServerEngine.Characters;
using ServerEngine.Commands.Internal;
using ServerEngine.MessageBroker;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ServerEngine
{
    internal static class EngineInternal
    {
        /// <summary>
        /// Indicates if the main game loop should keep running game turns
        /// </summary>
        public static bool RunGameLoop { get; set; }

        /// <summary>
        /// If set to false, triggers the engine factory to stop generating new engines and exit.
        /// </summary>
        public static bool RunFactory { get; set; }

        public static Action NewWorldCreator { get; set; }

        public static Func<Character> NewPlayerCreator { get; set; }

        /// <summary>
        /// Runs the game until they win, die or exit.
        /// </summary>
        public static void StartEngine()
        {
            AttachedClients.DetachAllClients("Server restart");
            RunGameLoop = true;
            RunFactory = true;

            InternalCommandHelper.TryRunServerCommand("autoloadbestgamestate", new List<string>(), null);

            // After the game state is loaded is the appropriate time to start accepting connections
            try
            {
                Broker.StartHost();

                // Main game loop goes 1 loop for 1 game turn.
                while (RunGameLoop)
                {
                    // Get all characters in the game that are still alive
                    var presentCharacters = GameState.CurrentGameState.GetAllCharactersPresentInWorld();
                    var sw = new Stopwatch();
                    sw.Start();
                    foreach (var gameCharacter in presentCharacters) // TODO: Sort turn order by character speed, fastest should go first.
                    {
                        // Only characters that are alive get a turn
                        if (!gameCharacter.IsDead())
                        {
                            gameCharacter.Turn();
                        }
                        else if (gameCharacter.CanRespawn())
                        {
                            gameCharacter.Respawn();
                        }
                    }
                    sw.Stop();
                    if (sw.Elapsed.TotalSeconds < 4)
                    {
                        Task.Delay(60000).Wait();
                    }
                }
            }
            finally
            {
                Broker.StopHost();
            }
        }
    }
}
