using Newtonsoft.Json;
using System.Collections.Generic;

namespace ServerEngine
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TradeSet : TrackableInstance
    {
        [JsonProperty]
        public List<ItemRecipe> Recipes { get; private set; } = new List<ItemRecipe>();

        [JsonProperty]
        public string Name { get; private set; }

        public TradeSet(string name, params ItemRecipe[] recipes)
        {
            Name = name;
            Recipes.AddRange(recipes);
        }
    }
}
