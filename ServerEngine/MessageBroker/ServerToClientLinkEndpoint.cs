using Amqp;
using Amqp.Framing;
using Amqp.Listener;
using System;
using System.Threading;

namespace ServerEngine.MessageBroker
{
    internal class ServerToClientLinkEndpoint : ServerLinkEndpoint
    {
        public ServerToClientLinkEndpoint(ListenerLink link)
            : base(link)
        {
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
                    this.Link.SendMessage(message);
                }
                catch (Exception ex)
                {
                    // TODO: log details
                }
            }
            else
            {
                Interlocked.Increment(ref this.credit); // TODO: need better handling here instead of just not sending the message and returning void
            }
        }
    }
}
