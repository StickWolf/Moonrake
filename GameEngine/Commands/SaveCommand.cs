﻿using System;

namespace GameEngine.Commands
{
    internal class SaveCommand : ICommand
    {
        public void Exceute(EngineInternal engine)
        {
            var validSlotNames = GameState.GetValidSaveSlotNames();
            validSlotNames.Add("New Save");

            var slotToSave = Console.Choose("What slot do you want to save to?", validSlotNames);
            if (slotToSave.Equals("New Save"))
            {
                Console.WriteLine("Slot name?");
                slotToSave = Console.ReadLine();
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
