using Newtonsoft.Json;
using System;

namespace GameEngine
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TrackableInstance
    {
        [JsonProperty]
        public Guid TrackingId { get; set; } = Guid.NewGuid();
    }
}
