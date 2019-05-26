using Amqp;
using Amqp.Framing;
using Amqp.Listener;
using System;
using System.Threading;

namespace ServerEngine.MessageBroker
{
    internal class ServerLinkEndpoint : LinkEndpoint
    {
        public ListenerLink Link { get; set; }
        protected int credit;

        public ServerLinkEndpoint(ListenerLink link)
        {
            this.Link = link;
        }

        public void Disconnect(string reason)
        {
            Error disconnectReason = null;
            if (!string.IsNullOrWhiteSpace(reason))
            {
                disconnectReason = new Error(ErrorCode.DetachForced);
                disconnectReason.Description = reason;
            }

            Link.Close(TimeSpan.Zero, disconnectReason);
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
