using Newtonsoft.Json;
using System;

namespace GameEngine
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ItemRecipeIngredient
    {
        [JsonProperty]
        public Guid ItemIngredientTrackingId { get; private set; }

        [JsonProperty]
        public int Amount { get; private set; }

        public ItemRecipeIngredient(Guid itemIngredientTrackingId, int amount)
        {
            ItemIngredientTrackingId = itemIngredientTrackingId;
            Amount = amount;
        }
    }
}
