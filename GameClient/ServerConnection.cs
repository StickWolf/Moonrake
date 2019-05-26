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
        private static Connection connection;
        private static Session session;
        private static string replyTo;

        public static void Connect()
        {
            var factory = new ConnectionFactory();
            factory.SASL.Profile = SaslProfile.Anonymous;

            // If using a self-signed cert in the server, uncomment this line for testing
            factory.SSL.RemoteCertificateValidationCallback = (a, b, c, d) => true;
            //factory.SSL.CheckCertificateRevocation = false;

            connection = factory.CreateAsync(new Address(amqpServerAddress)).Result;
            session = new Session(connection);
            replyTo = "client-" + Guid.NewGuid().ToString();

            Attach recvAttach = new Attach()
            {
                Source = new Source() { Address = "request_processor" },
                Target = new Target() { Address = replyTo }
            };

            receiver = new ReceiverLink(session, "request-client-receiver", recvAttach, null);
            receiver.Start(300, OnMessageReceived);
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
            receiver.Accept(message);
            Windows.Main.txtGameText.Dispatcher.Invoke(() => Windows.Main.txtGameText.Text += message.Body.ToString());
        }



        // TODO:
        //public static void SendDtoMessage(FiniteDto messageDto)
        //{
        //    var serialized = JsonDtoSerializer.SerializeDto(messageDto);
        //    Helper.SendMessage(serialized);
        //}


                //// Get the type of Dto
                //var dtoName = JsonDtoSerializer.GetDtoName(nextMessage);
                //if (string.IsNullOrWhiteSpace(dtoName))
                //{
                //    continue;
                //}

                //// TODO: recode this to not be a switch (instead a map)
                //switch (dtoName)
                //{
                //    case "DescriptiveTextDto":
                //        var dto = JsonDtoSerializer.DeserializeAs<DescriptiveTextDto>(nextMessage);
                //        Windows.Main.txtGameText.Dispatcher.Invoke(() => Windows.Main.txtGameText.Text = dto.Text);
                //        break;
                //}

    }
}
