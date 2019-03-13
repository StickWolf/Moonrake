using System;
using System.Collections.Generic;

namespace GameEngine.Commands
{
    internal class LoadCommand : ICommand
    {
        public void Exceute(EngineInternal engine)
        {
            if(engine == null)
            {
                GameState.CurrentGameState = PickSavedGame();
            }
            else
            {
                engine.RunGameLoop = false;
            }
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("load", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Displays any saved games to the user and loads the one they choose
        /// </summary>
        /// <returns>
        /// Returns the loaded game state.
        /// Returns null when there are no saved games or the user choose not to load a saved game.
        /// </returns>
        private GameState PickSavedGame()
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
