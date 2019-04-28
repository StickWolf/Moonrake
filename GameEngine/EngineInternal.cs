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

        public GameSourceData GameData { get; set; }

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
                // TODO: save character data (hp, etc) to the save file. Track characters in game state.
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
                        gameCharacter.Turn(this.GameData);
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

            // Move all character instances into gameState
            foreach (var character in GameData.Characters.Values)
            {
                string characterLocationName = GameData.DefaultCharacterLocations[character.TrackingId];
                GameState.CurrentGameState.AddCharacter(character, characterLocationName);
            }

            // Add game vars that represent the inital game state
            foreach (var gv in GameData.DefaultGameVars)
            {
                GameState.CurrentGameState.SetGameVarValue(gv.Key, gv.Value);
            }

            // Set the default locations of where trade posts exist at
            foreach (var tp in GameData.DefaultTradePostLocations)
            {
                GameState.CurrentGameState.SetTradePostLocation(tp.Key, tp.Value);
            }

            // Set the default items that all characters have
            foreach (var characterName in GameData.DefaultCharacterItems.Keys)
            {
                foreach (var itemTrackingName in GameData.DefaultCharacterItems[characterName].Keys)
                {
                    GameState.CurrentGameState.TryAddCharacterItemCount(characterName, itemTrackingName, GameData.DefaultCharacterItems[characterName][itemTrackingName], GameData);
                }
            }

            foreach (var locationName in GameData.DefaultLocationItems.Keys)
            {
                foreach (var itemTrackingName in GameData.DefaultLocationItems[locationName].Keys)
                {
                    GameState.CurrentGameState.TryAddLocationItemCount(locationName, itemTrackingName, GameData.DefaultLocationItems[locationName][itemTrackingName], GameData);
                }
            }

            // Show the intro
            Console.Clear();
            Console.WriteLine(GameData.GameIntroductionText);
            Console.WriteLine();

            CommandHelper.TryRunPublicCommand("look", new List<string>(), this.GameData);
        }
    }
}
