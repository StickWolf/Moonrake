﻿using ServerEngine.Characters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using BaseClientServerDtos;
using BaseClientServerDtos.ToClient;
using ServerEngine.GrainSiloAndClient;

namespace ServerEngine.Locations
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
        public void SendDtoMessage(FiniteDto dto, Character fromCharacter)
        {
            var charactersInLocation = GrainClusterClient.Universe.GetCharactersInLocation(this.TrackingId).Result
                .Where(c => fromCharacter == null || c.TrackingId != fromCharacter.TrackingId);
            foreach (var charInLoc in charactersInLocation)
            {
                charInLoc.SendDtoMessage(dto);
            }
        }

        public void SendDescriptiveTextDtoMessage(string text, Character fromCharacter)
        {
            var dto = new DescriptiveTextDto(text);
            SendDtoMessage(dto, fromCharacter);
        }

        /// <summary>
        /// Gets all items at the location that are useable
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<Item, int>> GetUseableItems()
        {
            var useableLocationItems = GrainClusterClient.Universe.GetLocationItems(this.TrackingId).Result
                .Where(i => i.Key.IsVisible) // Only allow interaction with visible items
                .Where(i => i.Key.IsUseableFrom == ItemUseableFrom.All || i.Key.IsUseableFrom == ItemUseableFrom.Location) // Only choose items that can be used
                .Select(i => new KeyValuePair<Item, int>(i.Key, i.Value))
                .ToList();
            return useableLocationItems;
        }
    }
}