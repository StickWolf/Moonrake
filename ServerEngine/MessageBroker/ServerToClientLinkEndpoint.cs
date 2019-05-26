using Amqp;
using Amqp.Framing;
using Amqp.Listener;
using Amqp.Types;
using System;
using System.Threading;

namespace ServerEngine.MessageBroker
{
    internal class ServerToClientLinkEndpoint : LinkEndpoint
    {
        ListenerLink link;
        Client client;
        int credit;

        public ServerToClientLinkEndpoint(ListenerLink link)
        {
            this.link = link;
            this.link.Closed += Link_Closed;
            System.Console.WriteLine($"Client receiver link attached. LinkName: {link.Name}");
            client = new Client();
            client.SetServerToClientLinkEndpoint(this);
            AttachedClients.AttachClient(client);
        }

        public bool IsConnected()
        {
            return link != null && !link.IsClosed;
        }

        public void Disconnect(string reason)
        {
            Error disconnectReason = null;
            if (!string.IsNullOrWhiteSpace(reason))
            {
                disconnectReason = new Error(ErrorCode.DetachForced);
                disconnectReason.Description = reason;
            }

            link.Close(TimeSpan.Zero, disconnectReason);
        }

        private void Link_Closed(IAmqpObject sender, Error error)
        {
            System.Console.WriteLine($"Client receiver link disconnected. LinkName: {link.Name}");
            AttachedClients.DetachClient(client);
        }

        public void SendMessage(object body, string subject)
        {
            if (Interlocked.Decrement(ref this.credit) >= 0)
            {
                var message = new Message(body);
                message.Properties = new Properties()
                {
                    Subject = subject
                };

                try
                {
                    this.link.SendMessage(message);
                }
                catch (Exception ex)
                {
                    // TODO: log details
                }
            }
            else
            {
                Interlocked.Increment(ref this.credit);
            }
        }

        public override void OnDisposition(DispositionContext dispositionContext)
        {
        }

        public override void OnFlow(FlowContext flowContext)
        {
            Interlocked.Add(ref this.credit, flowContext.Messages);
        }
    }
}
