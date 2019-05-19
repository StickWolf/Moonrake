using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace NetworkUtils
{
    public class TcpClientHelper : IDisposable
    {
        private string Name { get; set; }
        private TcpClient Client { get; set; }
        private StreamReader Reader { get; set; }
        private StreamWriter Writer { get; set; }
        public bool StayConnected { get; set; } = true;
        private Thread ReaderThread { get; set; }
        private Thread WriterThread { get; set; }
        private ConcurrentQueue<string> OutgoingMessageQueue { get; set; } = new ConcurrentQueue<string>();
        private ConcurrentQueue<string> IncomingMessageQueue { get; set; } = new ConcurrentQueue<string>();

        public void SetClient(string hostName, int port, string name)
        {
            var tcpClient = new TcpClient(hostName, port); // TODO: handle exceptions here, like failure to connect
            SetClient(tcpClient, name);
        }

        public void SetClient(TcpClient tcpClient, string name)
        {
            Name = name;
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
            ReaderThread.Name = $"Read-{Name}";
            ReaderThread.Start();

            WriterThread = new Thread(HandleOutboundMessages);
            WriterThread.Name = $"Write-{Name}";
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
                    string inboundMessage = Reader.ReadLine();

                    // Inbound message will be null if the client disconnects
                    if (inboundMessage == null)
                    {
                        StayConnected = false;
                        continue;
                    }

                    IncomingMessageQueue.Enqueue(inboundMessage);
                }
                catch (Exception ex)
                {
                    StayConnected = false; // TODO: logging about error
                }
            }
        }

        /// <summary>
        /// Sends the specified message
        /// </summary>
        /// <param name="message">The message to send</param>
        public void SendMessage(string message)
        {
            // Remove any line returns so the entire message will be receieved as 1 message
            message = message.Replace("\n", string.Empty).Replace("\r", string.Empty);
            OutgoingMessageQueue.Enqueue(message);
        }

        /// <summary>
        /// Gets the next message.
        /// </summary>
        /// <returns>Null if there are no messages</returns>
        public string ReceiveMessage()
        {
            while (true)
            {
                if (IncomingMessageQueue.IsEmpty)
                {
                    return null;
                }
                else if (IncomingMessageQueue.TryDequeue(out string message))
                {
                    return message;
                }
            }
        }

        public void Dispose()
        {
            Reader?.Close();
            Writer?.Close();
            Client?.Close();
            Reader?.Dispose();
            Writer?.Dispose();
            Client?.Dispose();
        }
    }
}
