using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class TradePost : TrackableInstance
    {
        /// <summary>
        /// The name of the trade post.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// All the trade sets that are available at this trade post
        /// </summary>
        public List<Guid> TradeSetTrackingIds = new List<Guid>();

        public TradePost(string tradePostName, params Guid[] tradeSetTrackingIds)
        {
            Name = tradePostName;
            TradeSetTrackingIds.AddRange(tradeSetTrackingIds);
        }
    }
}
