using ServerEngine.Characters;
using System;
using System.Collections.Generic;

namespace ServerEngine.Commands.Internal
{
    internal class SaveGameStateCommand : ICommandServer
    {
        public List<string> ActivatingWords => new List<string>() { "savegamestate" };

        // TODO: make it so the server auto-saves the current game state every minute or so

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
