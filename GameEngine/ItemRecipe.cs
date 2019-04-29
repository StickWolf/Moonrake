using System;
using System.Collections.Generic;

namespace GameEngine
{
    public class ItemRecipe
    {
        public Guid ItemResultTrackingId { get; private set; }
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
