using BaseClientServerDtos.ToClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerEngine.Commands.Internal
{
    internal class LoadGameStateCommand : ICommandServer
    {
        public List<string> ActivatingWords => new List<string>() { "loadgamestate" };

        // Slot to load must be passed in extraWords
        public void Execute(List<string> extraWords, Client executingClient)
        {
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
