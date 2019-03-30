using System.Collections.Generic;

namespace GameEngine
{
    public class TradeSet
    {
        public List<ItemRecipe> Recipes { get; private set; } = new List<ItemRecipe>();
        public string Name { get; private set; }

        public TradeSet(string name, params ItemRecipe[] recipes)
        {
            Name = name;
            Recipes.AddRange(recipes);
        }
    }
}
