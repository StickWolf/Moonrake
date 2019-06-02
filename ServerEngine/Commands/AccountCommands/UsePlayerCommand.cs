using BaseClientServerDtos.ToClient;
using System;
using System.Collections.Generic;

namespace ServerEngine.Commands.AccountCommands
{
    internal class UsePlayerCommand : IAccountCommand
    {
        public List<string> ActivatingWords => new List<string>() { "useplayer" };

        public string PermissionNeeded => null;

        public void Execute(List<string> extraWords, Account executingAccount)
        {
            AttachedClient executingClient = AttachedClients.GetAccountFocusedClient(executingAccount);
            if (executingAccount == null || executingClient == null || GameState.CurrentGameState == null)
            {
                var errorMsgDto = new DescriptiveTextDto("The use player command is currently unavailable.");
                executingClient?.SendDtoMessage(errorMsgDto);
                return;
            }

            if (extraWords.Count != 1)
            {
                var errorMsgDto = new DescriptiveTextDto("Wrong number of parameters.");
                executingClient?.SendDtoMessage(errorMsgDto);
                return;
            }

            var character = executingAccount.GetCharacter(extraWords[0]);
            if (character == null)
            {
                var errorMsgDto = new DescriptiveTextDto("Unknown character.");
                executingClient?.SendDtoMessage(errorMsgDto);
                return;
            }

            // TODO: this instantly will switch active players in an account.. there needs to be a cooldown time here to logout then login
            AttachedClients.SetClientFocusedCharacter(executingClient.TrackingId, character.TrackingId);
            if (character.IsNew)
            {
                character.IsNew = false;
                var gameIntroMsgDto = new DescriptiveTextDto(GameState.CurrentGameState.GameIntroductionText);
                executingClient?.SendDtoMessage(gameIntroMsgDto);
            }
            else
            {
                var successMsgDto = new DescriptiveTextDto($"You have entered {character.GetLocation().LocationName}.");
                executingClient?.SendDtoMessage(successMsgDto);
            }
            var othersMsgDto = new DescriptiveTextDto($"{character.Name} has entered the area.");
            character.GetLocation().SendDescriptiveTextDtoMessage($"{character.Name} has entered the area.", character);
        }
    }
}
