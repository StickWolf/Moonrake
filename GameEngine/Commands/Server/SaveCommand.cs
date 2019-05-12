using GameEngine.Characters;
using System;
using System.Collections.Generic;

namespace GameEngine.Commands.Internal
{
    internal class SaveCommand : ICommandServer
    {
        public List<string> ActivatingWords => new List<string>() { "save" };

        public void Execute(List<string> extraWords, Client executingClient)
        {
            var validSlotNames = GameState.GetValidSaveSlotNames();
            validSlotNames.Add("New Save");

            var slotToSave = executingClient.Choose("What slot do you want to save to?", validSlotNames, includeCancel: true);
            if (slotToSave == null)
            {
                executingClient.SendMessage("Canceled Save");
                return;
            }
            if (slotToSave.Equals("New Save"))
            {
                executingClient.SendMessage("Slot name?");
                slotToSave = Console.ReadLine();
            }

            executingClient.SendMessage($"Saving {slotToSave}.");
            GameState.SaveGameState(slotToSave);
            executingClient.SendMessage("Saving complete.");
        }
    }
}
