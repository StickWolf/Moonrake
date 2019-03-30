using System.Collections.Generic;

namespace GameEngine
{
    public class ItemRecipe
    {
        public string ItemResult { get; private set; }
        public Dictionary<string, int> Ingredients { get; private set; } = new Dictionary<string, int>();

        public ItemRecipe(string itemResult, params ItemRecipeIngredient[] ingredients)
        {
            ItemResult = itemResult;
            
            foreach (var ingredient in ingredients)
            {
                Ingredients[ingredient.IngredientName] = ingredient.Amount;
            }
        }
    }
}
