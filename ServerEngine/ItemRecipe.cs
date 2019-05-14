using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ServerEngine
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ItemRecipe
    {
        [JsonProperty]
        public Guid ItemResultTrackingId { get; private set; }

        [JsonProperty]
        public Dictionary<Guid, int> Ingredients { get; private set; } = new Dictionary<Guid, int>();

        public ItemRecipe(Guid itemResultTrackingId, params ItemRecipeIngredient[] ingredients)
        {
            ItemResultTrackingId = itemResultTrackingId;
            
            foreach (var ingredient in ingredients)
            {
                Ingredients[ingredient.ItemIngredientTrackingId] = ingredient.Amount;
            }
        }
    }
}
