using Amqp.Listener;

namespace ServerEngine.MessageBroker
{
    internal class LinkProcessor : ILinkProcessor
    {
        void ILinkProcessor.Process(AttachContext attachContext)
        {
            // Note Attach.Role is
            // Sender = false
            // Receiver = true

            var client = AttachedClients.GetClientFromConnection(attachContext.Link.Session.Connection);

            // If a Receiver link is attaching
            if (attachContext.Attach.Role)
            {
                var endpoint = new ServerToClientLinkEndpoint(attachContext.Link);
                client.SetServerToClientLinkEndpoint(endpoint);
                System.Console.WriteLine($"Client receiver link attached. LinkName: {attachContext.Link.Name}");

                // Completes the attach operation with success
                attachContext.Complete(endpoint, 0);
            }
            else
            {
                var endpoint = new ServerLinkEndpoint(attachContext.Link);
                client.SetClientToServerLinkEndpoint(endpoint);
                System.Console.WriteLine($"Client sender link attached. LinkName: {attachContext.Link.Name}");

                // Completes the attach operation with success
                attachContext.Complete(endpoint, 0);
            }
        }
    }
}
