using GameEngine.Characters;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace GameEngine.Locations
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Location : TrackableInstance
    {
        /// <summary>
        /// This is the name of the location and must be unique within all locations.
        /// </summary>
        [JsonProperty]
        public string LocationName { get; private set; }

        /// <summary>
        /// A description of what this location looks like when the user is at the location.
        /// </summary>
        [JsonProperty]
        public string LocalDescription { get; private set; }

        /// <summary>
        /// A description off what this location looks like when the user is
        /// looking ant the location from a remote location.
        /// </summary>
        [JsonProperty]
        public string RemoteDescription { get; private set; }

        /// <summary>
        /// Constructs a new Location
        /// </summary>
        /// <param name="remoteDescription">A description of what the location looks like from a remote location.</param>
        /// <param name="localDescription">A description of what the location looks like when the user is in it.</param>
        public Location(string locationName, string remoteDescription, string localDescription)
        {
            LocationName = locationName;
            LocalDescription = localDescription;
            RemoteDescription = remoteDescription;
        }

        /// <summary>
        /// Sends a message to the location that is visible to all characters in the location
        /// </summary>
        /// <param name="text">The text to send</param>
        /// <param name="fromCharacter">
        /// If the message is being sent from a character then specify that here.
        /// Otherwise use Guid.Empty
        /// This will assure that the sender does not also get the message
        /// </param>
        public void SendMessage(string text, Character fromCharacter)
        {
            var playerCharacters = GameState.CurrentGameState.GetPlayerCharacters();
            var playerLocations = GameState.CurrentGameState.GetPlayerCharacterLocations();
            var location = GameState.CurrentGameState.GetLocation(TrackingId);
            if (playerLocations.Contains(location) &&
                (fromCharacter == null || fromCharacter.TrackingId != playerCharacters[0].TrackingId))
            {
                Console.WriteLine(text); // SendMessage
            }
            else
            {
                // TODO: add an admin command that lets you see these "Inaudible" messages
                //Console.WriteLine($"{{SendMessageToLocation}} \"{this.LocationName}\" : {text}"); // SendMessage
            }
        }
    }
}