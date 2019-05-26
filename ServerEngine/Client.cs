﻿using Amqp;
using Newtonsoft.Json;
using ServerEngine.MessageBroker;
using System;
using System.Collections.Generic;

namespace ServerEngine
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Client : TrackableInstance
    {
        /// <summary>
        /// The account that this client is currently logged into
        /// </summary>
        [JsonIgnore]
        public Account AttachedAccount { get; set; }

        [JsonIgnore]
        private ServerToClientLinkEndpoint ServerToClientEndpoint { get; set; }

        [JsonIgnore]
        private ServerLinkEndpoint ClientToServerEndpoint { get; set; }

        [JsonIgnore]
        private Connection ClientConnection { get; set; }

        internal void SetConnection(Connection connection)
        {
            this.ClientConnection = connection;
            this.ClientConnection.Closed += ClientConnection_Closed;
            System.Console.WriteLine($"Client connection discovered. Client Tracking Id: {this.TrackingId}");
        }

        public bool EqualsConnection(Connection connection)
        {
            if (connection == null || this.ClientConnection == null)
            {
                return false;
            }
            return ClientConnection.Equals(connection);
        }

        private void ClientConnection_Closed(IAmqpObject sender, Amqp.Framing.Error error)
        {
            System.Console.WriteLine($"Client connection closed. Client Tracking Id: {this.TrackingId}");
            AttachedClients.DetachClient(this);
        }

        internal void SetServerToClientLinkEndpoint(ServerToClientLinkEndpoint endpoint)
        {
            this.ServerToClientEndpoint = endpoint;
            endpoint.Link.Closed += StcLink_Closed;
        }

        internal void SetClientToServerLinkEndpoint(ServerLinkEndpoint endpoint)
        {
            this.ClientToServerEndpoint = endpoint;
            endpoint.Link.Closed += CtsLink_Closed;
        }

        private void StcLink_Closed(IAmqpObject sender, Amqp.Framing.Error error)
        {
            System.Console.WriteLine($"STC link closed. LinkName: {ServerToClientEndpoint.Link.Name}");
        }

        private void CtsLink_Closed(IAmqpObject sender, Amqp.Framing.Error error)
        {
            System.Console.WriteLine($"CTS link closed. LinkName: {ServerToClientEndpoint.Link.Name}");
        }

        public bool IsConnected()
        {
            if (
                ServerToClientEndpoint.Link != null && !ServerToClientEndpoint.Link.IsClosed &&
                ClientToServerEndpoint.Link != null && !ClientToServerEndpoint.Link.IsClosed
                )
            {
                return true;
            }

            return false;
        }

        public void Disconnect(string reason)
        {
            ServerToClientEndpoint.Disconnect(reason);
            ClientToServerEndpoint.Disconnect(reason);
        }

        public void SendMessage(string text, bool newLine = true)
        {
            if (ServerToClientEndpoint != null)
            {
                ServerToClientEndpoint.SendMessage(text, "TODO");
            }
            else
            {
                // TODO: keeping for now until the server/client are fully separated.
                if (newLine)
                {
                    System.Console.WriteLine(text.AddLineReturns(true));
                }
                else
                {
                    System.Console.Write(text);
                }
            }
        }

        /// <summary>
        /// Sends a blank line
        /// </summary>
        public void SendMessage()
        {
            SendMessage(string.Empty);
        }

        /// <summary>
        /// Writes out a list of items and lets the user choose 1 of them.
        /// </summary>
        /// <param name="choices">All the available choices</param>
        /// <param name="prompt">The text to display above the choices</param>
        /// <param name="includeCancel">Include the cancel option</param>
        /// <returns>The chosen item</returns>
        public string Choose(string prompt, List<string> choices, bool includeCancel)
        {
            Dictionary<string, string> choicesDictionary = new Dictionary<string, string>();
            foreach (var choice in choices)
            {
                choicesDictionary.Add(choice, choice);
            }
            return Choose(prompt, choicesDictionary, includeCancel);
        }

        /// <summary>
        /// Writes out a list of items and lets the user choose 1 of them.
        /// </summary>
        /// <param name="choices">All the available choices</param>
        /// <param name="prompt">The text to display above the choices</param>
        /// <returns>The chosen key or default(T) if cancelled</returns>
        public T Choose<T>(string prompt, Dictionary<T, string> choices, bool includeCancel)
        {
            SendMessage(prompt);

            Dictionary<int, T> numberedKeys = new Dictionary<int, T>();
            int itemIndex = 1;
            foreach (var choice in choices)
            {
                SendMessage(string.Empty);
                SendMessage($"{itemIndex}. {choice.Value}");
                numberedKeys.Add(itemIndex++, choice.Key);
            }
            if (includeCancel)
            {
                SendMessage(string.Empty);
                SendMessage($"{itemIndex}. Cancel");
            }

            SendMessage("-----------------------------------------");

            while (true)
            {
                SendMessage("> ", false);
                if (int.TryParse(Console.ReadLine(), out int userChoiceIndex)) // TODO: The choose functionality needs to be re-written as a client ability
                {
                    if (userChoiceIndex >= 1 && userChoiceIndex < itemIndex)
                    {
                        return numberedKeys[userChoiceIndex];
                    }
                    else if (userChoiceIndex == itemIndex)
                    {
                        return default(T);
                    }
                }
                SendMessage("Please pick one of the numbers above.");
            }
        }
    }
}
