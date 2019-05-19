using NetworkUtils;
using System.Collections.Generic;

namespace GameClient.Commands
{
    public class ConnectCommand : IClientCommand
    {
        public List<string> ActivatingWords => new List<string>() { "connect" };

        public void Execute(List<string> extraWords)
        {
            GameConnection.ServerConnection = new TcpClientHelper();
            GameConnection.ServerConnection.SetClient("127.0.0.1", 15555, "Client");
            GameConnection.ServerConnection.StartMessageHandlers();
            GameConnection.ServerConnection.SendMessage("Hello server, I know where you are!");
        }
    }
}
