using Amqp;
using Amqp.Framing;
using Amqp.Listener;
using BaseClientServerDtos;
using BaseClientServerDtos.ToClient;
using BaseClientServerDtos.ToServer;
using Newtonsoft.Json;
using ServerEngine.Commands;
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

        private object autoattachDevSysopLock = new object();

        internal void SetConnection(Connection connection)
        {
            this.ClientConnection = connection;
            this.ClientConnection.Closed += ClientConnection_Closed;
            System.Console.WriteLine($"Client connection discovered.\r\n   Client Tracking Id: {this.TrackingId}");

            // TODO: remove hack after this can be done from the client proper
            AutoAttachDevSysopClient();

            // TODO: these messages won't be sent because the links are not established yet
            // TODO: when a newly created character logs into the game for the first time, these actions should be run
            //if (newPlayerCreated)
            //{
            //    var currentPlayerCharacter = AttachedClients.GetClientFocusedCharacter(this.TrackingId);
            //    currentPlayerCharacter.SendDescriptiveTextDtoMessage(GameState.CurrentGameState.GameIntroductionText);
            //    currentPlayerCharacter.ExecuteLookCommand();
            //}
        }

        /// <summary>
        /// Auto creates accounts with 1 player each every time a new client logs in concurrently
        /// Previously created accounts are reused if clients disconnect and reconnect
        /// </summary>
        private void AutoAttachDevSysopClient()
        {
            lock (autoattachDevSysopLock)
            {
                int attemptId = 0;
                while (attemptId < 10)
                {
                    attemptId++;

                    // TODO: Remove after client/server accounts are fully functional.
                    // TODO: For localhost testing only!
                    // TODO: The attached account should be assigned only after proper auth
                    var accountName = $"ServerUser{attemptId}";
                    var potentialAccount = GameState.CurrentGameState.GetAccount(accountName);
                    if (potentialAccount == null)
                    {
                        potentialAccount = GameState.CurrentGameState.CreateAccount(accountName);
                    }

                    // TODO: For now until we have real clients, we auto-create a new player character (if needed) here
                    // TODO: This simulates a client connecting to an account and adding a new character to their account.
                    if (potentialAccount.Characters.Count == 0)
                    {
                        CommandRunner.TryRunCommandFromAccount("createnewplayer", new List<string>(), potentialAccount); // TODO: pass player name as the first parameter in order to name your player
                    }

                    var firstCharacter = potentialAccount.Characters[0];
                    var existingClient = AttachedClients.GetCharacterFocusedClient(firstCharacter);
                    if (existingClient == null)
                    {
                        AttachedAccount = potentialAccount;
                        AttachedClients.SetClientFocusedCharacter(TrackingId, firstCharacter);
                        break;
                    }
                }
            }
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
            AttachedClients.DetachClient(this, error?.Description);
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
                // This Dto takes care of running all server commands if the client is
                // in the right context and has the right permissions
                case nameof(ServerCommandDto):
                    var dto = JsonDtoSerializer.DeserializeAs<ServerCommandDto>(jsonBody);
                    if (!CommandRunner.TryRunCommandFromClient(dto.Command, dto.ExtraWords, this))
                    {
                        SendDescriptiveTextDtoMessage($"Unknown command");
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

        private void SendDescriptiveTextDtoMessage(string text)
        {
            var textMsgDto = new DescriptiveTextDto(text);
            SendDtoMessage(textMsgDto);
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
    }
}
