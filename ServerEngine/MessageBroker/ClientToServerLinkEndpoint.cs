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
            //Link.SetCredit(50, false, true);
        }

        public override void OnMessage(MessageContext messageContext)
        {
            OnMessageAction?.Invoke(messageContext);
        }
    }
}
