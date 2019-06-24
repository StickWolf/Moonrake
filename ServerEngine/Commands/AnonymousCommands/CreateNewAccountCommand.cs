using BaseClientServerDtos.ToClient;
using ServerEngine.Commands.AnonymousCommands;
using ServerEngine.GrainSiloAndClient;
using System.Collections.Generic;

namespace ServerEngine.Commands.AccountCommands
{
    internal class CreateNewAccountCommand : IAnonymousCommand
    {
        public List<string> ActivatingWords => new List<string>() { "createnewaccount" };

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

            var potentialAccount = GrainClusterClient.Accounts.CreateAccount(extraWords[0], extraWords[1]).Result;
            if (potentialAccount == null)
            {
                var errorMsgDto = new DescriptiveTextDto("Error creating account.");
                executingClient?.SendDtoMessage(errorMsgDto);
                return;
            }

            var successMsgDto = new DescriptiveTextDto("Account created.");
            executingClient?.SendDtoMessage(successMsgDto);
        }
    }
}
