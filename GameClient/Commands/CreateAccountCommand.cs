using BaseClientServerDtos.ToClient;
using System.Collections.Generic;

namespace GameClient.Commands
{
    public class CreateAccountCommand : IClientCommand
    {
        public List<string> ActivatingWords => new List<string>() { "createaccount" };

        public void Execute(List<string> extraWords)
        {
            var createDialog = new CreateAccountDialog();
            createDialog.ShowDialog();
        }
    }
}
