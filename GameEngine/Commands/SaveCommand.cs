﻿using GameEngine.Characters;
using System;
using System.Collections.Generic;

namespace GameEngine.Commands
{
    internal class SaveCommand : ICommandInternal
    {
        public void Execute(EngineInternal engine, List<string> extraWords, Character savingCharacter)
        {
            var validSlotNames = GameState.GetValidSaveSlotNames();
            validSlotNames.Add("New Save");

            var slotToSave = savingCharacter.Choose("What slot do you want to save to?", validSlotNames, includeCancel: true);
            if (slotToSave == null)
            {
                savingCharacter.SendMessage("Canceled Save");
                return;
            }
            if (slotToSave.Equals("New Save"))
            {
                savingCharacter.SendMessage("Slot name?");
                slotToSave = Console.ReadLine();
            }

            savingCharacter.SendMessage($"Saving {slotToSave}.");
            GameState.SaveGameState(slotToSave);
            savingCharacter.SendMessage("Saving complete.");
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("save", StringComparison.OrdinalIgnoreCase);
        }
    }
}
