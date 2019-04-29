using GameEngine.Characters;
using GameEngine.Commands;
using System.Collections.Generic;

namespace GameEngine
{
    internal class EngineInternal
    {
        /// <summary>
        /// Indicates if the main game loop should keep running game turns
        /// </summary>
        public bool RunGameLoop { get; set; } = true;

        /// <summary>
        /// If set to false, triggers the engine factory to stop generating new engines and exit.
        /// </summary>
        public bool RunFactory { get; set; } = true;

        public bool PlayerIsDead { get; set; } = false;
        public bool PlayerHasWon { get; set; } = false;

        private GameSourceData GameData { get; set; }

        public EngineInternal(GameSourceData gameData)
        {
            GameData = gameData;
        }

        /// <summary>
        /// Runs the game until they win, die or exit.
        /// </summary>
        public void StartEngine()
        {
            // Ask the player to pick to load a saved game if there are any
            CommandHelper.TryRunInternalCommand("load", new List<string>(), this);

            // Main game loop goes 1 loop for 1 game turn.
            while (RunGameLoop)
            {
                // Get all characters in the game that are still alive
                var allLocateableCharacters = GameState.CurrentGameState.GetAllCharacters();
                foreach (var gameCharacter in allLocateableCharacters) // TODO: Sort turn order by character speed, fastest should go first.
                {
                    // Only characters that are alive get a turn
                    if (gameCharacter.IsDead())
                    {
                        continue;
                    }

                    // If this is the player character, then call a special version of "Turn"
                    if (gameCharacter is PlayerCharacter)
                    {
                        (gameCharacter as PlayerCharacter).InternalTurn(this);
                    }
                    else
                    {
                        gameCharacter.Turn();
                    }
                }

                var playerCharacter = GameState.CurrentGameState.GetPlayerCharacter();
                if (playerCharacter.Hp <= 0)
                {
                    PlayerIsDead = true;
                }
                if (PlayerIsDead)
                {
                    Console.WriteLine();
                    Console.WriteLine("You have died. Please press a key.");
                    Console.ReadKey();
                    RunGameLoop = false;
                }
                // TODO: Find a way to figure out when the player has won.
                else if (PlayerHasWon)
                {
                    Console.WriteLine(GameData.GameEndingText);
                    Console.WriteLine("             |--The End--|             ");
                    Console.ReadLine();
                    RunGameLoop = false;
                }
            }
        }

        /// <summary>
        /// Sets up everything to start a new game and shows the game introduction text
        /// </summary>
        public void StartNewGame()
        {
            GameState.CreateNewGameState();

            // Move locations over
            foreach (var location in GameData.TODO_Delete_And_Use_GameState_Instead_Locations)
            {
                GameState.CurrentGameState.AddLocation(location.Value);
            }

            // Move Items over
            foreach (var item in GameData.TODO_Delete_And_Use_GameState_Instead_Items.Values)
            {
                GameState.CurrentGameState.AddItem(item);
            }

            // Move portals over
            foreach (var portal in GameData.TODO_Delete_And_Use_GameState_Instead_Portals.Values)
            {
                GameState.CurrentGameState.AddPortal(portal);
            }

            // Move all character instances into gameState
            foreach (var character in GameData.TODO_Delete_And_Use_GameState_Instead_Characters.Values)
            {
                var locationTrackingId = GameData.TODO_Delete_And_Use_GameState_Instead_DefaultCharacterLocations[character.TrackingId];
                GameState.CurrentGameState.AddCharacter(character, locationTrackingId);
            }

            // Port tradesets over
            foreach (var tradeSet in GameData.TODO_Delete_And_Use_GameState_Instead_TradeSets.Values)
            {
                GameState.CurrentGameState.AddTradeSet(tradeSet);
            }

            // Port tradePosts over
            foreach (var tradePost in GameData.TODO_Delete_And_Use_GameState_Instead_TradePosts.Values)
            {
                var locationTrackingId = GameData.TODO_Delete_And_Use_GameState_Instead_DefaultTradePostLocations[tradePost.TrackingId];
                GameState.CurrentGameState.AddTradePost(tradePost, locationTrackingId);
            }

            // Add game vars that represent the inital game state
            foreach (var gv in GameData.TODO_Delete_And_Use_GameState_Instead_DefaultGameVars)
            {
                GameState.CurrentGameState.SetGameVarValue(gv.Key, gv.Value);
            }

            // Set the default items that all characters have
            foreach (var characterTrackingId in GameData.TODO_Delete_And_Use_GameState_Instead_DefaultCharacterItems.Keys)
            {
                foreach (var itemTrackingId in GameData.TODO_Delete_And_Use_GameState_Instead_DefaultCharacterItems[characterTrackingId].Keys)
                {
                    GameState.CurrentGameState.TryAddCharacterItemCount(characterTrackingId, itemTrackingId, GameData.TODO_Delete_And_Use_GameState_Instead_DefaultCharacterItems[characterTrackingId][itemTrackingId]);
                }
            }

            foreach (var locationTrackingId in GameData.TODO_Delete_And_Use_GameState_Instead_DefaultLocationItems.Keys)
            {
                foreach (var itemTrackingId in GameData.TODO_Delete_And_Use_GameState_Instead_DefaultLocationItems[locationTrackingId].Keys)
                {
                    GameState.CurrentGameState.TryAddLocationItemCount(locationTrackingId, itemTrackingId, GameData.TODO_Delete_And_Use_GameState_Instead_DefaultLocationItems[locationTrackingId][itemTrackingId]);
                }
            }

            // Show the intro
            Console.Clear();
            Console.WriteLine(GameData.GameIntroductionText);
            Console.WriteLine();

            CommandHelper.TryRunPublicCommand("look", new List<string>());
        }
    }
}
