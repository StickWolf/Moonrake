using ServerEngine.Characters;
using ServerEngine.Characters.Behaviors;
using ServerEngine.Commands.Internal;
using ServerEngine.Commands.Public;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ServerEngine
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

        public static Action NewWorldCreator { get; set; }

        public static Func<Character> NewPlayerCreator { get; set; }

        /// <summary>
        /// Runs the game until they win, die or exit.
        /// </summary>
        public static void StartEngine()
        {
            AttachedClients.DetachAllClients();
            RunGameLoop = true;
            RunFactory = true;

            // TODO: remove this and the property after client/server split. The goal is that all commands can be run from the client itself.
            ServerClient = new Client();
            AttachedClients.AttachClient(ServerClient);

            // TODO: make it so (by default) when the server starts up, it just loads the first available saved game state
            // TODO: or in the case wherere there is no game state. It just creates up a new one.
            InternalCommandHelper.TryRunServerCommand("loadgamestate", new List<string>(), ServerClient);

            // TODO: Remove after client/server accounts are fully functional. Until then we're creating a "Server" account
            // TODO: that the server user will be automatically "logged" into
            ServerClient.AttachedAccount = GameState.CurrentGameState.GetAccount("ServerUser");

            // TODO: For now until we have real clients, we auto-create a new player character (if needed) here
            // TODO: This simulates a client connecting to an account and adding a new character to their account.
            if (ServerClient.AttachedAccount.Characters.Count == 0)
            {
                InternalCommandHelper.TryRunServerCommand("createnewplayer", new List<string>(), ServerClient);
            }
            else
            {
                AttachedClients.SetClientFocusedCharacter(ServerClient.TrackingId, ServerClient.AttachedAccount.Characters[0]);
            }

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
                    else if(gameCharacter.CanRespawn())
                    {
                        gameCharacter.Respawn();
                    }
                }
                sw.Stop();
                if (sw.Elapsed.TotalSeconds < 4)
                {
                    Task.Delay(2000).Wait();
                }
            }
        }
    }
}
