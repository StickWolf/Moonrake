using ServerEngine.Characters;
using ServerEngine.Commands;
using ServerEngine.GrainSiloAndClient;
using ServerEngine.MessageBroker;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ServerEngine.GrainInterfaces;

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

        private static bool shutdownStarted;
        private static object shutdownLock = new object();

        /// <summary>
        /// Runs the game until they win, die or exit.
        /// </summary>
        public static void StartEngine()
        {
            AttachedClients.DetachAllClients("Server restart");
            GrainSiloHost.StopAsync().Wait();
            GrainClusterClient.StopAsync().Wait();
            RunGameLoop = true;
            RunFactory = true;
            shutdownStarted = false;

            var engineStartingAccount = new Account()
            {
                Permissions = new List<string>() { "Sysop" }
            };
            CommandRunner.TryRunCommandFromAccount("autoloadbestgamestate", new List<string>(), engineStartingAccount);

            // After the game state is loaded is the appropriate time to start accepting connections
            try
            {
                GrainSiloHost.StartAsync().Wait();
                Task.Delay(5000).Wait();
                GrainClusterClient.StartAsync().Wait();

                // TODO: This would need to be in the createnewgamestate command instead of here
                Guid testGuid = Guid.NewGuid();
                var item = GrainClusterClient.ClusterClient.GetGrain<IItemGrain>(testGuid);
                item.SetDisplayName("Test thing").Wait();

                var item2 = GrainClusterClient.ClusterClient.GetGrain<IItemGrain>(testGuid);
                var desc = item2.GetDescription(2).Result;

                Broker.StartHost();

                // Main game loop goes 1 loop for 1 game turn.
                while (RunGameLoop)
                {
                    GameState.AutoSaveIfNeeded(forceSave: false);

                    // Get all characters in the game that are still alive
                    var turningNPC = GameState.CurrentGameState.GetNextTurnNPC();

                    if (turningNPC == null)
                    {
                        Task.Delay(200).Wait();
                        continue;
                    }
                    else
                    {
                        // Only characters that are alive get a turn
                        if (!turningNPC.IsDead())
                        {
                            turningNPC.Turn();
                        }
                        else if (turningNPC.CanRespawn())
                        {
                            turningNPC.Respawn();
                        }
                        turningNPC.TurnComplete();
                    }
                }
            }
            finally
            {
                EngineShutdown(null, null);
            }
        }

        private static void EngineShutdown(object sender, EventArgs e)
        {
            lock (shutdownLock)
            {
                if (!shutdownStarted)
                {
                    shutdownStarted = true;
                    GameState.AutoSaveIfNeeded(forceSave: true);
                    Broker.StopHost();
                }
            }
        }
    }
}
