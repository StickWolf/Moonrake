using BaseClientServerDtos.ToClient;
using ServerEngine.Commands.AnonymousCommands;
using ServerEngine.GrainSiloAndClient;
using System.Collections.Generic;

namespace ServerEngine.Commands.AccountCommands
{
    internal class LoginCommand : IAnonymousCommand
    {
        public List<string> ActivatingWords => new List<string>() { "login" };

        // extrawords
        // 1 - username
        // 2 - password
        public void Execute(List<string> extraWords, AttachedClient executingClient)
        {
            if (executingClient == null)
            {
                return;
            }

            if (extraWords.Count != 2)
            {
                var errorMsgDto = new DescriptiveTextDto("Wrong number of parameters.");
                executingClient?.SendDtoMessage(errorMsgDto);
                return;
            }

            if (AttachedClients.IsAccountLoggedIn(extraWords[0]))
            {
                var errorMsgDto = new DescriptiveTextDto("Account is already logged in.");
                executingClient?.SendDtoMessage(errorMsgDto);
                return;
            }

            var potentialAccount = GrainClusterClient.Universe.GetAccount(extraWords[0]).Result;
            if (potentialAccount != null && potentialAccount.ValidatePassword(extraWords[1]).Result)
            {
                executingClient.AttachedAccount = potentialAccount;
                var successMsgDto = new DescriptiveTextDto("Login success.");
                executingClient?.SendDtoMessage(successMsgDto);
            }
            else
            {
                var errorMsgDto = new DescriptiveTextDto("Either the username or password were incorrect.");
                executingClient?.SendDtoMessage(errorMsgDto);
            }
        }
    }
}
