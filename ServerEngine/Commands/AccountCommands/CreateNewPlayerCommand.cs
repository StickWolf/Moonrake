using BaseClientServerDtos.ToClient;
using ServerEngine.GrainInterfaces;
using System;
using System.Collections.Generic;

namespace ServerEngine.Commands.AccountCommands
{
    internal class CreateNewPlayerCommand : IAccountCommand
    {
        public List<string> ActivatingWords => new List<string>() { "createnewplayer" };

        public string PermissionNeeded => null;

        public void Execute(List<string> extraWords, IAccountGrain executingAccount)
        {
            AttachedClient executingClient = AttachedClients.GetAccountFocusedClient(executingAccount);
            if (executingAccount == null || executingClient == null || GameState.CurrentGameState == null || !executingAccount.CanCreateNewPlayer().Result)
            {
                var errorMsgDto = new DescriptiveTextDto("The create new player command is currently unavailable.");
                executingClient?.SendDtoMessage(errorMsgDto);
                return;
            }

            if (extraWords.Count != 1)
            {
                var errorMsgDto = new DescriptiveTextDto("Wrong number of parameters.");
                executingClient?.SendDtoMessage(errorMsgDto);
                return;
            }

            if (GameState.CurrentGameState.IsPlayerCharacterNameInUse(extraWords[0]))
            {
                var errorMsgDto = new DescriptiveTextDto("This character name is already taken.");
                executingClient?.SendDtoMessage(errorMsgDto);
                return;
            }

            var newPlayerCharacter = EngineInternal.NewPlayerCreator(); // TODO: move this NewPlayerCreator into the game Universe grain
            newPlayerCharacter.Name = extraWords[0];

            // Mark all player characters as needing focus to stay in the world
            newPlayerCharacter.NeedsFocus = true;
            executingAccount.AddCharacter(newPlayerCharacter.TrackingId).Wait();

            var successMsgDto = new DescriptiveTextDto("New player created.");
            executingClient?.SendDtoMessage(successMsgDto);
        }
    }
}
