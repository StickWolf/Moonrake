using BaseClientServerDtos;
using BaseClientServerDtos.ToClient;
using NetworkUtils;
using ServerEngine.Characters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerEngine
{
    internal static class AttachedClients
    {
        // Clients[{ClientTrackingId}] = {AttachedClient}
        private static ConcurrentDictionary<Guid, Client> Clients { get; set; } = new ConcurrentDictionary<Guid, Client>();

        // ClientFocusedCharacters[{ClientTrackingId}] = {CurrentlyFocusedCharacterTrackingId}
        private static Dictionary<Guid, Guid> ClientFocusedCharacters { get; set; } = new Dictionary<Guid, Guid>();

        private static Thread ListenerThread { get; set; }

        public static void StartListener()
        {
            ListenerThread = new Thread(ListenForClientConnections);
            ListenerThread.Name = "WaitForClientListener";
            ListenerThread.Start();
        }

        public static void ListenForClientConnections()
        {
            var ipAddress = IPAddress.Parse("127.0.0.1"); // TODO: make these configurable
            TcpListener listener = null;
            try
            {
                listener = new TcpListener(ipAddress, 15555);
                listener.Start();
                while (true)
                {
                    // TODO: configure max concurrent connections to be appropriate for the game / machine
                    if (Clients.Count >= 3)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    TcpClient tcpClient = listener.AcceptTcpClient();
                    Client trackerClient = null;
                    try
                    {
                        trackerClient = new Client();
                        trackerClient.ClientHelper = new TcpClientHelper();
                        trackerClient.ClientHelper.SetClient(tcpClient, $"AttachedClient({trackerClient.TrackingId.ToString()})");
                        AttachedClients.AttachClient(trackerClient);
                        // TODO: add an "OnClientDetached" event in the helper and hook an event here that will run the appropraiate detach so the client is removed from the list
                        trackerClient.ClientHelper.StartMessageHandlers();

                        // Send the client a descriptive text message indicating they are successfully connected
                        var successMessage = new DescriptiveTextDto();
                        successMessage.Text = "Welcome to the server."; // TODO: change this to "Welcome to {GameName} server"
                        trackerClient.SendMessage(JsonDtoSerializer.SerializeDto(successMessage));
                    }
                    catch
                    {
                        if (trackerClient != null)
                        {
                            AttachedClients.DetachClient(trackerClient);
                        }
                    }
                }
            }
            // TODO: catch
            finally
            {
                listener?.Stop();
            }
        }

        public static void AttachClient(Client client)
        {
            while (!Clients.TryAdd(client.TrackingId, client))
            {
                Thread.Sleep(1000);
            }
        }

        public static void DetachClient(Client client)
        {
            client.ClientHelper?.Dispose();

            // Remove the client's focus on any character they are tracking if we have a current game loaded
            SetClientFocusedCharacter(client.TrackingId, Guid.Empty);

            if (Clients.ContainsKey(client.TrackingId))
            {
                while (!Clients.TryRemove(client.TrackingId, out Client removed))
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public static void DetachAllClients()
        {
            Clients.Clear();
            ClientFocusedCharacters.Clear();
        }

        public static Client GetClient(Guid clientTrackingId)
        {
            if (Clients.ContainsKey(clientTrackingId))
            {
                return Clients[clientTrackingId];
            }
            return null;
        }

        public static void SetClientFocusedCharacter(Guid clientTrackingId, Guid characterTrackingId)
        {
            // Remove any other client focus on this character first
            if (ClientFocusedCharacters.ContainsValue(characterTrackingId))
            {
                var otherClientsTrackingThisChar = ClientFocusedCharacters.Where(c => c.Value == characterTrackingId && c.Key != clientTrackingId);
                foreach (var other in otherClientsTrackingThisChar)
                {
                    ClientFocusedCharacters.Remove(other.Key);
                }
            }

            // If the character focus should be removed
            if (characterTrackingId == Guid.Empty)
            {
                if (ClientFocusedCharacters.ContainsKey(clientTrackingId))
                {
                    ClientFocusedCharacters.Remove(clientTrackingId);
                }
            }
            else
            {
                ClientFocusedCharacters[clientTrackingId] = characterTrackingId;
            }
        }

        public static Character GetClientFocusedCharacter(Guid clientTrackingId)
        {
            if (!ClientFocusedCharacters.ContainsKey(clientTrackingId))
            {
                return null;
            }
            var characterTrackingId = ClientFocusedCharacters[clientTrackingId];
            if (GameState.CurrentGameState != null)
            {
                return GameState.CurrentGameState.GetCharacter(characterTrackingId);
            }
            return null;
        }

        public static Client GetCharacterFocusedClient(Guid characterTrackingId)
        {
            if (!ClientFocusedCharacters.ContainsValue(characterTrackingId))
            {
                return null;
            }
            var clientTrackingId = ClientFocusedCharacters.First(c => c.Value == characterTrackingId).Key;
            return GetClient(clientTrackingId);
        }
    }
}
