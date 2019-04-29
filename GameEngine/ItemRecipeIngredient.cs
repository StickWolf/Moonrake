using System;

namespace GameEngine
{
    public class ItemRecipeIngredient
    {
        public Guid ItemIngredientTrackingId { get; private set; }
        public int Amount { get; private set; }

        public ItemRecipeIngredient(Guid itemIngredientTrackingId, int amount)
        {
            ItemIngredientTrackingId = itemIngredientTrackingId;
            Amount = amount;
        }
    }
}
