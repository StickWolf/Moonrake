using Amqp.Listener;

namespace ServerEngine.MessageBroker
{
    internal class LinkProcessor : ILinkProcessor
    {
        void ILinkProcessor.Process(AttachContext attachContext)
        {
            var client = AttachedClients.GetClientFromConnection(attachContext.Link.Session.Connection);

            // If a Receiver link is attaching
            if (attachContext.Attach.Role)
            {
                var endpoint = new ServerToClientLinkEndpoint(attachContext.Link);
                client.SetServerToClientLinkEndpoint(endpoint);

                // Completes the attach operation with success
                attachContext.Complete(endpoint, 0);
            }
            else
            {
                var endpoint = new ClientToServerLinkEndpoint(attachContext.Link);
                client.SetClientToServerLinkEndpoint(endpoint);

                // Completes the attach operation with success
                attachContext.Complete(endpoint, 1);
            }
        }
    }
}
