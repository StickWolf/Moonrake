using Amqp;
using Amqp.Framing;
using Amqp.Listener;
using BaseClientServerDtos;
using BaseClientServerDtos.ToServer;
using Newtonsoft.Json;
using ServerEngine.Commands.Internal;
using ServerEngine.Commands.Public;
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
            System.Console.WriteLine($"Client connection discovered.\r\n   Client Tracking Id: {this.TrackingId}");

            // TODO: Remove after client/server accounts are fully functional.
            // TODO: For localhost testing only!
            // TODO: The attached account should be assigned only after proper auth
            AttachedAccount = GameState.CurrentGameState.GetAccount("ServerUser");

            // TODO: For now until we have real clients, we auto-create a new player character (if needed) here
            // TODO: This simulates a client connecting to an account and adding a new character to their account.
            if (AttachedAccount.Characters.Count == 0)
            {
                InternalCommandHelper.TryRunServerCommand("createnewplayer", new List<string>(), this); // TODO: pass player name as the first parameter in order to name your player
            }
            AttachedClients.SetClientFocusedCharacter(TrackingId, AttachedAccount.Characters[0]);

            // TODO: these messages won't be sent because the links are not established yet
            // TODO: when a newly created character logs into the game for the first time, these actions should be run
            //if (newPlayerCreated)
            //{
            //    var currentPlayerCharacter = AttachedClients.GetClientFocusedCharacter(this.TrackingId);
            //    currentPlayerCharacter.SendDescriptiveTextDtoMessage(GameState.CurrentGameState.GameIntroductionText);
            //    currentPlayerCharacter.ExecuteLookCommand();
            //}
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
            System.Console.WriteLine($"Client connection closed.\r\n   Client Tracking Id: {this.TrackingId}\r\n   Details: {error?.Description}");
            AttachedClients.DetachClient(this);
        }

        internal void SetServerToClientLinkEndpoint(ServerToClientLinkEndpoint endpoint)
        {
            System.Console.WriteLine($"STC link attached.\r\n   LinkName: {endpoint.Link.Name}");
            this.ServerToClientEndpoint = endpoint;
            endpoint.Link.Closed += StcLink_Closed;
        }

        internal void SetClientToServerLinkEndpoint(ClientToServerLinkEndpoint endpoint)
        {
            System.Console.WriteLine($"CTS link attached.\r\n   LinkName: {endpoint.Link.Name}");
            this.ClientToServerEndpoint = endpoint;
            endpoint.Link.Closed += CtsLink_Closed;
            endpoint.OnMessageAction = CtsLink_OnMessage;
        }

        private void CtsLink_OnMessage(MessageContext context)
        {
            // Ack that we got the message.
            //link.Accept(message);
            context.Complete();

            // Ignore messages that don't have a body
            if (context.Message?.Body == null)
            {
                return;
            }

            // Ignore messages that are not a serialized FiniteDto
            var jsonBody = context.Message.Body.ToString();
            var dtoName = JsonDtoSerializer.GetDtoName(jsonBody);
            if (string.IsNullOrWhiteSpace(dtoName))
            {
                return;
            }

            // TODO: recode this to not be a switch (instead a map)
            switch (dtoName)
            {
                case "GenericServerCommandDto":
                    var dto = JsonDtoSerializer.DeserializeAs<GenericServerCommandDto>(jsonBody);

                    // TODO: what about the ability to run internal commands ? what validates which users can run which commands?

                    var thisPlayer = AttachedClients.GetClientFocusedCharacter(this.TrackingId);
                    if (thisPlayer != null)
                    {
                        PublicCommandHelper.TryRunPublicCommand(dto.Command, dto.ExtraWords, thisPlayer);
                    }
                    break;
            }
        }

        private void StcLink_Closed(IAmqpObject sender, Error error)
        {
            if (sender is ListenerLink)
            {
                var listenerLink = sender as ListenerLink;
                System.Console.WriteLine($"STC link closed.\r\n   LinkName: {listenerLink.Name}\r\n   Details: {error?.Description}");
            }
        }

        private void CtsLink_Closed(IAmqpObject sender, Error error)
        {
            if (sender is ListenerLink)
            {
                var listenerLink = sender as ListenerLink;
                System.Console.WriteLine($"CTS link closed.\r\n   LinkName: {listenerLink.Name}\r\n   Details: {error?.Description}");
            }
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

        public void SendDtoMessage(FiniteDto dto)
        {
            var serialized = JsonDtoSerializer.SerializeDto(dto);

            if (ServerToClientEndpoint != null)
            {
                ServerToClientEndpoint.SendMessage(serialized);
            }
            else
            {
                System.Console.WriteLine(serialized);
            }
        }

        // TODO: rewrite
        ///// <summary>
        ///// Writes out a list of items and lets the user choose 1 of them.
        ///// </summary>
        ///// <param name="choices">All the available choices</param>
        ///// <param name="prompt">The text to display above the choices</param>
        ///// <param name="includeCancel">Include the cancel option</param>
        ///// <returns>The chosen item</returns>
        //public string Choose(string prompt, List<string> choices, bool includeCancel)
        //{
        //    Dictionary<string, string> choicesDictionary = new Dictionary<string, string>();
        //    foreach (var choice in choices)
        //    {
        //        choicesDictionary.Add(choice, choice);
        //    }
        //    return Choose(prompt, choicesDictionary, includeCancel);
        //}

        ///// <summary>
        ///// Writes out a list of items and lets the user choose 1 of them.
        ///// </summary>
        ///// <param name="choices">All the available choices</param>
        ///// <param name="prompt">The text to display above the choices</param>
        ///// <returns>The chosen key or default(T) if cancelled</returns>
        //public T Choose<T>(string prompt, Dictionary<T, string> choices, bool includeCancel)
        //{
        //    SendMessage(prompt);

        //    Dictionary<int, T> numberedKeys = new Dictionary<int, T>();
        //    int itemIndex = 1;
        //    foreach (var choice in choices)
        //    {
        //        SendMessage(string.Empty);
        //        SendMessage($"{itemIndex}. {choice.Value}");
        //        numberedKeys.Add(itemIndex++, choice.Key);
        //    }
        //    if (includeCancel)
        //    {
        //        SendMessage(string.Empty);
        //        SendMessage($"{itemIndex}. Cancel");
        //    }

        //    SendMessage("-----------------------------------------");

        //    while (true)
        //    {
        //        SendMessage("> ", false);
        //        if (int.TryParse(Console.ReadLine(), out int userChoiceIndex)) // TODO: The choose functionality needs to be re-written as a client ability
        //        {
        //            if (userChoiceIndex >= 1 && userChoiceIndex < itemIndex)
        //            {
        //                return numberedKeys[userChoiceIndex];
        //            }
        //            else if (userChoiceIndex == itemIndex)
        //            {
        //                return default(T);
        //            }
        //        }
        //        SendMessage("Please pick one of the numbers above.");
        //    }
        //}
    }
}
