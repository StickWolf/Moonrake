using BaseClientServerDtos.ToClient;
using ServerEngine.GrainInterfaces;
using System.Collections.Generic;

namespace ServerEngine.Commands.AccountCommands
{
    internal class SaveGameStateCommand : IAccountCommand
    {
        public List<string> ActivatingWords => new List<string>() { "savegamestate" };

        public string PermissionNeeded => "Sysop";

        public void Execute(List<string> extraWords, IAccountGrain executingAccount)
        {
            AttachedClient executingClient = AttachedClients.GetAccountFocusedClient(executingAccount);
            if (executingAccount == null || GameState.CurrentGameState == null)
            {
                var errorMsgDto = new DescriptiveTextDto("The save game state command is currently unavailable.");
                executingClient?.SendDtoMessage(errorMsgDto);
                return;
            }

            GameState.SaveGameState();
            var successMsgDto = new DescriptiveTextDto("Save complete.");
            executingClient?.SendDtoMessage(successMsgDto);
        }
    }
}
