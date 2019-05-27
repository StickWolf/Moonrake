using Amqp;
using Amqp.Framing;
using Amqp.Sasl;
using BaseClientServerDtos;
using BaseClientServerDtos.ToClient;
using System;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace GameClient
{
    public static class ServerConnection
    {
        private static string amqpServerAddress = "amqps://127.0.0.1:5671";
        private static ReceiverLink receiver;
        private static SenderLink sender;
        private static Connection connection;
        private static Session session;
        private static string clientAddressName;

        public static void Connect()
        {
            var factory = new ConnectionFactory();
            factory.SASL.Profile = SaslProfile.Anonymous;

            // If using a self-signed cert in the server, uncomment this line for testing
            factory.SSL.RemoteCertificateValidationCallback = (a, b, c, d) => true;
            //factory.SSL.CheckCertificateRevocation = false;

            connection = factory.CreateAsync(new Address(amqpServerAddress)).Result;
            session = new Session(connection);
            clientAddressName = "client-" + Guid.NewGuid().ToString();

            Attach recvAttach = new Attach()
            {
                Source = new Source() { Address = "Server" },
                Target = new Target() { Address = clientAddressName }
            };

            receiver = new ReceiverLink(session, "Link-STC", recvAttach, null);
            receiver.Start(300, OnMessageReceived);

            sender = new SenderLink(session, "Link-CTS", clientAddressName);
        }

        public static void Disconnect(string reason)
        {
            var reasonError = new Error(ErrorCode.ConnectionForced);
            reasonError.Description = reason;

            receiver?.Close(TimeSpan.Zero, reasonError);
            session?.Close(TimeSpan.Zero, reasonError);
            connection?.Close(TimeSpan.Zero, reasonError);
        }

        private static void OnMessageReceived(IReceiverLink receiver, Message message)
        {
            // Ack that we got the message.
            receiver.Accept(message);

            // Ignore messages that don't have a body
            if (message.Body == null)
            {
                return;
            }

            // Ignore messages that are not a serialized FiniteDto
            var dtoName = JsonDtoSerializer.GetDtoName(message.Body.ToString());
            if (string.IsNullOrWhiteSpace(dtoName))
            {
                return;
            }

            // TODO: recode this to not be a switch (instead a map)
            switch (dtoName)
            {
                case "DescriptiveTextDto":
                    var dto = JsonDtoSerializer.DeserializeAs<DescriptiveTextDto>(message.Body.ToString());
                    Windows.Main.txtGameText.Dispatcher.Invoke(() => {
                        var textbox = Windows.Main.txtGameText;
                        textbox.AppendText($"\r\n{dto.Text}");
                        textbox.CaretIndex = textbox.Text.Length;
                        textbox.ScrollToEnd();
                    });
                    break;
            }
        }

        public static void SendDtoMessage(FiniteDto messageDto)
        {
            var serialized = JsonDtoSerializer.SerializeDto(messageDto);
            var message = new Message(serialized);
            try
            {
                sender.SendAsync(message);
            }
            catch
            {
                // TODO: 
            }
        }
    }
}
