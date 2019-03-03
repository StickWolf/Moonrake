using System;
using System.Collections.Generic;

namespace GameEngine
{
    public static class EngineFactory
    {
        /// <summary>
        /// Starts the factory.
        /// The factory will create games and start them over and over until stopped.
        /// </summary>
        /// <param name="gameDataFactory">A method that returns a GameData</param>
        public static void Start(Func<IGameData> gameDataFactory)
        {
            bool keepPlaying = true;
            while (keepPlaying)
            {
                // Create a new game data instance
                var gameData = gameDataFactory();

                // Create a new engine 
                var engine = new EngineInternal(gameData);

                // Ask the player to pick to load a saved game if there are any
                var loadedGameState = PickSavedGame();

                // Start the game
                engine.Start(loadedGameState);
            }
        }

        /// <summary>
        /// Displays any saved games to the user and loads the one they choose
        /// </summary>
        /// <returns>
        /// Returns the loaded game state.
        /// Returns null when there are no saved games or the user choose not to load a saved game.
        /// </returns>
        private static GameState PickSavedGame()
        {
            int displaycount = 1;
            Dictionary<int, string> SlotNames = new Dictionary<int, string>();
            Console.WriteLine("Load or start new game?");
            foreach (var slotName in GameState.GetValidSaveSlotNames())
            {
                Console.WriteLine();
                Console.WriteLine($"{displaycount}. {slotName}");
                SlotNames.Add(displaycount, slotName);
                displaycount++;
            }
            Console.WriteLine();
            Console.WriteLine($"{displaycount}. Start a new game");
            Console.WriteLine("-----------------------------------------");

            int input;
            while (true)
            {
                try
                {
                    Console.Write("> ");
                    var chosenKey = Console.ReadKey();
                    input = int.Parse(chosenKey.KeyChar.ToString());
                    break;
                }
                catch
                {
                    Console.WriteLine("Please pick one of the numbers above.");
                }
            }

            GameState loadedGameState = null;
            if (SlotNames.ContainsKey(input))
            {
                string slotToLoad;
                slotToLoad = SlotNames[input];
                Console.WriteLine($"Loading {slotToLoad}.");
                loadedGameState = GameState.LoadGameState(slotToLoad);
                Console.WriteLine("Loading complete.");
            }

            return loadedGameState;
        }
    }
}
