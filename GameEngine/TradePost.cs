using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GameEngine
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TradePost : TrackableInstance
    {
        /// <summary>
        /// The name of the trade post.
        /// </summary>
        [JsonProperty]
        public string Name { get; private set; }

        /// <summary>
        /// All the trade sets that are available at this trade post
        /// </summary>
        [JsonProperty]
        public List<Guid> TradeSetTrackingIds = new List<Guid>();

        public TradePost(string tradePostName, params Guid[] tradeSetTrackingIds)
        {
            Name = tradePostName;
            TradeSetTrackingIds.AddRange(tradeSetTrackingIds);
        }
    }
}
