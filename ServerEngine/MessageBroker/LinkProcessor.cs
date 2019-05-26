using Amqp;
using Amqp.Framing;
using Amqp.Listener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerEngine.MessageBroker
{
    internal class LinkProcessor : ILinkProcessor
    {
        void ILinkProcessor.Process(AttachContext attachContext)
        {
            // Note Attach.Role is
            // Sender = false
            // Receiver = true

            // If a Receiver link is attaching
            if (attachContext.Attach.Role)
            {
                var endpoint = new ServerToClientLinkEndpoint(attachContext.Link);

                // Completes the attach operation with success
                attachContext.Complete(endpoint, 0);
            }
            else
            {
                // TODO: add sender link attach logic
            }
            
        }
    }
}
