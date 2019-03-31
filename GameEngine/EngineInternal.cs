using GameEngine.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public GameSourceDataBase GameData { get; set; }

        public EngineInternal(GameSourceDataBase gameData)
        {
            GameData = gameData;
        }

        /// <summary>
        /// Runs the game until they win, die or exit.
        /// </summary>
        public void StartEngine()
        {
            // Ask the player to pick to load a saved game if there are any
            var loadCommand = CommandHelper.GetCommand("load");
            loadCommand.Exceute(this);

            // Main game loop goes 1 loop for 1 game turn.
            while (RunGameLoop)
            {
                ProcessUserInput();

                // TODO: Fix this to check the actual player instead of hardcoding true here.
                if (PlayerIsDead)
                {
                    Console.WriteLine();
                    Console.WriteLine("You have died. Please press a key.");
                    Console.ReadKey();
                    RunGameLoop = false;
                }
                else if (PlayerHasWon)
                {
                    // TODO: Print out the end of game story. This should be provided by the game data.
                    RunGameLoop = false;
                }
            }
        }

        private void ProcessUserInput()
        {
            string input;
            Console.Write(">");
            input = Console.ReadLine();
            Console.WriteLine();
            var partsOfInput = input.Split(' ');
            var firstWord = partsOfInput[0];

            var commandToRun = CommandHelper.GetCommand(firstWord);
            if (commandToRun == null)
            {
                Console.WriteLine($"I don't know what you mean by '{firstWord}'.");
                return;
            }

            // The command is a real command if we got this far
            commandToRun.Exceute(this);
        }

        /// <summary>
        /// Sets up everything to start a new game and shows the game introduction text
        /// </summary>
        public void StartNewGame()
        {
            GameState.CreateNewGameState();

            // Transfer all defaults from the game data to game state
            GameState.CurrentGameState.PlayerName = GameData.DefaultPlayerName;
            GameState.CurrentGameState.CharacterLocations["Player"] = GameData.StartingLocationName;

            // Add game vars that represent the inital game state
            foreach (var gv in GameData.DefaultGameVars)
            {
                GameState.CurrentGameState.GameVars.Add(gv.Key, gv.Value);
            }

            // Set the default locations of where trade posts exist at
            foreach (var tp in GameData.DefaultTradePostLocations)
            {
                GameState.CurrentGameState.CurrentTradePostLocations.Add(tp.Key, tp.Value);
            }

            // Set the default items that all characters have
            foreach (var characterName in GameData.DefaultCharacterItems.Keys)
            {
                foreach (var itemName in GameData.DefaultCharacterItems[characterName].Keys)
                {
                    GameState.CurrentGameState.TryAddCharacterItemCount(characterName, itemName, GameData.DefaultCharacterItems[characterName][itemName], GameData);
                }
            }

            // Show the intro
            Console.Clear();
            Console.WriteLine(GameData.GameIntroductionText);
            Console.WriteLine();

            var lookCommand = CommandHelper.GetCommand("look");
            lookCommand.Exceute(this);
        }
    }
}
