using System;
using System.Collections.Generic;

namespace GameEngine.Commands
{
    internal class LoadCommand : ICommand
    {
        public void Exceute(EngineInternal engine)
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

            if (SlotNames.ContainsKey(input))
            {
                string slotToLoad;
                slotToLoad = SlotNames[input];
                Console.WriteLine();
                Console.WriteLine($"Loading {slotToLoad}.");
                GameState.LoadGameState(slotToLoad);
                Console.WriteLine("Loading complete.");
            }
            else
            {
                engine.StartNewGame();
            }
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("load", StringComparison.OrdinalIgnoreCase);
        }
    }
}
