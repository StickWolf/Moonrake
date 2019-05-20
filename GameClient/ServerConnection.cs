using BaseClientServerDtos;
using BaseClientServerDtos.ToClient;
using NetworkUtils;
using System.Threading;

namespace GameClient
{
    public static class ServerConnection
    {
        private static TcpClientHelper Helper { get; set; }

        private static Thread IncomingMessageProcessingThread { get; set; }

        public static void Connect()
        {
            Helper = new TcpClientHelper();
            Helper.SetTcpClient("127.0.0.1", 15555, "Client");

            // Start a thread to process incoming messages from the server
            IncomingMessageProcessingThread = new Thread(ProcessIncomingMessages);
            IncomingMessageProcessingThread.Start();
        }

        public static void Disconnect()
        {
            if (Helper != null)
            {
                Helper.StayConnected = false;
            }
        }

        private static void ProcessIncomingMessages()
        {
            while (Helper.StayConnected)
            {
                string nextMessage = Helper.ReceiveMessage();
                if (nextMessage == null)
                {
                    // TODO: during a time when there are no messages, this thread will be spinning pretty fast..
                    // TODO: maybe instead build an event that can be subscribed to or something into the Helper so we only get
                    // TODO: get called when needed
                    continue;
                }

                // Get the type of Dto
                var dtoName = JsonDtoSerializer.GetDtoName(nextMessage);
                if (string.IsNullOrWhiteSpace(dtoName))
                {
                    continue;
                }

                // TODO: recode this to not be a switch (instead a map)
                switch (dtoName)
                {
                    case "DescriptiveTextDto":
                        var dto = JsonDtoSerializer.DeserializeAs<DescriptiveTextDto>(nextMessage);
                        Windows.Main.txtGameText.Dispatcher.Invoke(() => Windows.Main.txtGameText.Text = dto.Text);
                        break;
                }

            }
        }
    }
}
