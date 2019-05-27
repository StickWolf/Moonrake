using System.Collections.Generic;

namespace GameClient.Commands
{
    public class ConnectCommand : ICommand
    {
        public List<string> ActivatingWords => new List<string>() { "connect" };

        public void Execute(List<string> extraWords)
        {
            ServerConnection.Connect();
        }
    }
}
