using Newtonsoft.Json;
using System;

namespace ServerEngine.Locations
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PortalDestinationDetails
    {
        /// <summary>
        /// The destination of where the portal leads to.
        /// Null if the portal is closed.
        /// </summary>
        [JsonProperty]
        public Guid DestinationTrackingId { get; set; }

        /// <summary>
        /// A description of what the portal looks like.
        /// </summary>
        [JsonProperty]
        public string Description { get; set; }
    }
}
