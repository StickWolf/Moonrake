using System;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace NetworkUtils
{
    public class TcpClientHelper : IDisposable
    {
        private string Name { get; set; }
        private TcpClient Client { get; set; }
        private bool InternalClient { get; set; } // indicates if the client was new'd internally and should be disposed internally as well.
        private StreamReader Reader { get; set; }
        private StreamWriter Writer { get; set; }
        public bool StayConnected { get; set; } = true;
        private Thread ReaderThread { get; set; }
        private Thread WriterThread { get; set; }
        private ConcurrentQueue<string> OutgoingMessageQueue { get; set; } = new ConcurrentQueue<string>();

        public void SetClient(string hostName, int port, string clientName)
        {
            var tcpClient = new TcpClient(hostName, port);
            InternalClient = true;
            SetClient(tcpClient, clientName);
        }

        public void SetClient(TcpClient tcpClient, string clientName)
        {
            Name = clientName;
            Client = tcpClient;
            Reader = new StreamReader(Client.GetStream());
            Writer = new StreamWriter(Client.GetStream());
        }

        public void BlockUntilDisconnect()
        {
            while (StayConnected) // TODO: also check if the client is actually connected
            {
                Thread.Sleep(1000);
            }
        }

        public void StartMessageHandlers()
        {
            if (Client == null || Reader == null || Writer == null || Client.Connected == false)
            {
                return;
            }

            // start a new thread to process inbound messages
            ReaderThread = new Thread(HandleInboundMessages);
            ReaderThread.Start();

            WriterThread = new Thread(HandleOutboundMessages);
            WriterThread.Start();
        }

        private void HandleOutboundMessages()
        {
            while (StayConnected)
            {
                // Send any messages that we have ready to the other client
                if (OutgoingMessageQueue.Count != 0)
                {
                    if (OutgoingMessageQueue.TryDequeue(out string messageToSend))
                    {
                        Writer.WriteLine(messageToSend);
                        Writer.Flush();
                    }
                }
            }
        }

        private void HandleInboundMessages()
        {
            while (StayConnected)
            {
                try
                {
                    // Look for a message from the other client
                    string inboundMessage = Reader.ReadLine(); // TODO: we'll need a way to read an entire json chunk and recognize when it ends also

                    // Inbound Message will be null if the client disconnects
                    if (inboundMessage == null)
                    {
                        StayConnected = false;
                    }
                    if (inboundMessage != null)
                    {
                        Console.WriteLine($"Got Message from {Name}: {inboundMessage}"); // TODO: decide what we'll really do with the message
                    }
                }
                catch (Exception ex)
                {
                    StayConnected = false; // TODO: logging about error
                }
            }
        }

        public void SendMessage(string message)
        {
            OutgoingMessageQueue.Enqueue(message);
        }

        public void Dispose()
        {
            Reader?.Close();
            Writer?.Close();
            Reader?.Dispose();
            Writer?.Dispose();
            if (InternalClient)
            {
                Client?.Close();
                Client?.Dispose();
            }
        }
    }
}
