using System.Collections.Generic;

namespace GameEngine
{
    public class TradePost
    {
        /// <summary>
        /// The name of the trade post.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// All the trade sets that are available at this trade post
        /// </summary>
        public List<string> TradeSets = new List<string>();

        public TradePost(string tradePostName, params string[] tradeSets)
        {
            Name = tradePostName;
        }
    }
}
