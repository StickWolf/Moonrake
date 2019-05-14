using ServerEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerEngine
{
    internal static class AttachedClients
    {
        // Clients[{ClientTrackingId}] = {AttachedClient}
        private static Dictionary<Guid, Client> Clients { get; set; } = new Dictionary<Guid, Client>();

        // ClientFocusedCharacters[{ClientTrackingId}] = {CurrentlyFocusedCharacterTrackingId}
        private static Dictionary<Guid, Guid> ClientFocusedCharacters { get; set; } = new Dictionary<Guid, Guid>();

        public static void AttachClient(Client client)
        {
            Clients[client.TrackingId] = client;
        }

        public static void DetachClient(Client client)
        {
            // Remove the client's focus on any character they are tracking if we have a current game loaded
            SetClientFocusedCharacter(client.TrackingId, Guid.Empty);

            if (Clients.ContainsKey(client.TrackingId))
            {
                Clients.Remove(client.TrackingId);
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
