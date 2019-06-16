using Amqp;
using ServerEngine.Characters;
using ServerEngine.GrainInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerEngine
{
    internal static class AttachedClients
    {
        private static readonly object addRemoveClientLock = new object();

        // Clients[{ClientTrackingId}] = {AttachedClient}
        private static Dictionary<Guid, AttachedClient> Clients { get; set; } = new Dictionary<Guid, AttachedClient>();

        // ClientFocusedCharacters[{ClientTrackingId}] = {CurrentlyFocusedCharacterTrackingId}
        private static Dictionary<Guid, Guid> ClientFocusedCharacters { get; set; } = new Dictionary<Guid, Guid>();

        internal static AttachedClient GetClientFromConnection(Connection connection)
        {
            lock (addRemoveClientLock)
            {
                var client = Clients.Values.FirstOrDefault(c => c.EqualsConnection(connection));
                if (client == null)
                {
                    client = new AttachedClient();
                    client.SetConnection(connection);
                    AttachClient(client);
                }
                return client;
            }
        }

        internal static void AttachClient(AttachedClient client)
        {
            lock (addRemoveClientLock)
            {
                if (!Clients.ContainsKey(client.TrackingId))
                {
                    Clients.Add(client.TrackingId, client);
                }
            }
        }

        internal static void DetachClient(AttachedClient client, string reason)
        {
            if (client == null)
            {
                return;
            }

            lock (addRemoveClientLock)
            {
                if (client.IsConnected())
                {
                    client.Disconnect(reason);
                }

                // Remove the client
                if (Clients.ContainsKey(client.TrackingId))
                {
                    Clients.Remove(client.TrackingId);
                }

                // Remove the client's focus on the character they may be tracking
                if (ClientFocusedCharacters.ContainsKey(client.TrackingId))
                {
                    ClientFocusedCharacters.Remove(client.TrackingId);
                }
            }
        }

        internal static void DetachAllClients(string reason)
        {
            lock (addRemoveClientLock)
            {
                while (Clients.Count > 0)
                {
                    var attachedClient = Clients.Values.FirstOrDefault();
                    DetachClient(attachedClient, reason);
                }
            }
        }

        internal static AttachedClient GetClient(Guid clientTrackingId)
        {
            lock (addRemoveClientLock)
            {
                if (Clients.ContainsKey(clientTrackingId))
                {
                    return Clients[clientTrackingId];
                }
                return null;
            }
        }

        internal static void RemoveClientFocusOnCharacter(Guid clientTrackingId)
        {
            lock (addRemoveClientLock)
            {
                if (ClientFocusedCharacters.ContainsKey(clientTrackingId))
                {
                    ClientFocusedCharacters.Remove(clientTrackingId);
                }
            }
        }

        internal static void SetClientFocusedCharacter(Guid clientTrackingId, Guid characterTrackingId)
        {
            lock (addRemoveClientLock)
            {
                // Remove all clients focus on this character
                do
                {
                    var otherClientTrackingThisChar = ClientFocusedCharacters.FirstOrDefault(c => c.Value == characterTrackingId);
                    if (otherClientTrackingThisChar.Key != Guid.Empty)
                    {
                        ClientFocusedCharacters.Remove(otherClientTrackingThisChar.Key);
                    }

                } while (ClientFocusedCharacters.Where(c => c.Value == characterTrackingId).Count() > 0) ;

                ClientFocusedCharacters[clientTrackingId] = characterTrackingId;
            }
        }

        public static Character GetClientFocusedCharacter(Guid clientTrackingId)
        {
            lock (addRemoveClientLock)
            {
                if (!ClientFocusedCharacters.ContainsKey(clientTrackingId))
                {
                    return null;
                }
                var characterTrackingId = ClientFocusedCharacters[clientTrackingId];
                return GameState.CurrentGameState?.GetCharacter(characterTrackingId);
            }
        }

        public static AttachedClient GetCharacterFocusedClient(Guid characterTrackingId)
        {
            lock (addRemoveClientLock)
            {
                if (!ClientFocusedCharacters.ContainsValue(characterTrackingId))
                {
                    return null;
                }
                var clientTrackingId = ClientFocusedCharacters.First(c => c.Value == characterTrackingId).Key;
                return GetClient(clientTrackingId);
            }
        }

        public static AttachedClient GetAccountFocusedClient(IAccountGrain account)
        {
            lock (addRemoveClientLock)
            {
                var client = Clients.Values.FirstOrDefault(c => c.AttachedAccount != null && c.AttachedAccount.Equals(account));
                return client;
            }
        }

        public static bool IsAccountLoggedIn(string accountName)
        {
            lock (addRemoveClientLock)
            {
                return Clients.Values.Any(c => c.AttachedAccount != null && c.AttachedAccount.GetUserName().Result.Equals(accountName));
            }
        }
    }
}
