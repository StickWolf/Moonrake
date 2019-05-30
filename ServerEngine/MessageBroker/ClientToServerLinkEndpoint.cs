using Amqp.Listener;
using System;

namespace ServerEngine.MessageBroker
{
    internal class ClientToServerLinkEndpoint : ServerLinkEndpoint
    {
        public Action<MessageContext> OnMessageAction { get; set; }

        public ClientToServerLinkEndpoint(ListenerLink link)
            : base(link)
        {
        }

        public override void OnMessage(MessageContext messageContext)
        {
            OnMessageAction?.Invoke(messageContext);
        }
    }
}
