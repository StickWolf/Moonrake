using NetworkUtils;
using System.Collections.Generic;

namespace GameClient.Commands
{
    public class ConnectCommand : IClientCommand
    {
        public List<string> ActivatingWords => new List<string>() { "connect" };

        public void Execute(List<string> extraWords)
        {
            ServerConnection.Connect();
        }
    }
}
