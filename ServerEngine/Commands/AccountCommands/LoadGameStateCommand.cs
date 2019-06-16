using BaseClientServerDtos.ToClient;
using ServerEngine.GrainInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerEngine.Commands.AccountCommands
{
    internal class LoadGameStateCommand : IAccountCommand
    {
        public List<string> ActivatingWords => new List<string>() { "loadgamestate" };

        public string PermissionNeeded => "Sysop";

        // Slot to load must be passed in extraWords
        public void Execute(List<string> extraWords, IAccountGrain executingAccount)
        {
            AttachedClient executingClient = AttachedClients.GetAccountFocusedClient(executingAccount);
            if (executingAccount == null || GameState.CurrentGameState != null)
            {
                var errorMsgDto = new DescriptiveTextDto("The load game state command is currently unavailable.");
                executingClient?.SendDtoMessage(errorMsgDto);
                return;
            }

            if (extraWords == null || extraWords.Count != 1)
            {
                var errorMsgDto = new DescriptiveTextDto("Wrong number of parameters passed.");
                executingClient?.SendDtoMessage(errorMsgDto);
                return;
            }

            try
            {
                var validSlotNames = GameState.GetValidSaveSlotNames();
                var slotToLoad = validSlotNames.FirstOrDefault(s => s.Equals(extraWords[0], StringComparison.OrdinalIgnoreCase));
                if (slotToLoad == null)
                {
                    var errorMsgDto = new DescriptiveTextDto("Invalid slot name.");
                    executingClient?.SendDtoMessage(errorMsgDto);
                    return;
                }

                GameState.LoadGameState(slotToLoad);
                var successMsgDto = new DescriptiveTextDto($"Loaded game state {slotToLoad}.");
                executingClient?.SendDtoMessage(successMsgDto);
            }
            catch (Exception ex)
            {
                // Show error in server console
                System.Console.WriteLine(ex.ToString());

                // Send error message to client
                var errorMsgDto = new DescriptiveTextDto("An error occurred while loading the game state.");
                executingClient?.SendDtoMessage(errorMsgDto);
            }
        }
    }
}
