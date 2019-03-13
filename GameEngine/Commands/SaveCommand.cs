using System;
using System.Collections.Generic;

namespace GameEngine.Commands
{
    internal class SaveCommand : ICommand
    {
        public void Exceute(EngineInternal engine)
        {
            int displaycount = 1;
            Dictionary<int, string> SlotNames = new Dictionary<int, string>();
            Console.WriteLine("What slot do you want to save to?");
            foreach (var slotName in GameState.GetValidSaveSlotNames())
            {
                Console.WriteLine();
                Console.WriteLine($"{displaycount}. {slotName}");
                SlotNames.Add(displaycount, slotName);
                displaycount++;
            }
            Console.WriteLine();
            Console.WriteLine($"{displaycount}. New Save");
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
            Console.WriteLine();

            string slotToSave;
            if (SlotNames.ContainsKey(input))
            {
                slotToSave = SlotNames[input];
            }
            else if (input == displaycount)
            {
                Console.WriteLine("Slot name?");
                slotToSave = Console.ReadLine();
            }
            else
            {
                // no save
                return;
            }

            Console.WriteLine($"Saving {slotToSave}.");
            GameState.SaveGameState(slotToSave, GameState.CurrentGameState);
            Console.WriteLine("Saving complete.");
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("save", StringComparison.OrdinalIgnoreCase);
        }
    }
}
