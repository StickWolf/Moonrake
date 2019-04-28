using System;
using System.Collections.Generic;

namespace GameEngine.Commands
{
    internal class SaveCommand : ICommandInternal
    {
        public void Exceute(EngineInternal engine, List<string> extraWords)
        {
            var validSlotNames = GameState.GetValidSaveSlotNames();
            validSlotNames.Add("New Save");
            validSlotNames.Add("Cancel");

            var slotToSave = Console.Choose("What slot do you want to save to?", validSlotNames);
            if (slotToSave.Equals("New Save"))
            {
                Console.WriteLine("Slot name?");
                slotToSave = Console.ReadLine();
            }
            if (slotToSave == "Cancel")
            {
                Console.WriteLine("Canceled Save");
                return;
            }

            Console.WriteLine($"Saving {slotToSave}.");
            GameState.SaveGameState(slotToSave);
            Console.WriteLine("Saving complete.");
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("save", StringComparison.OrdinalIgnoreCase);
        }
    }
}
