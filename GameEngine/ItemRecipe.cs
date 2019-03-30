using System.Collections.Generic;

namespace GameEngine
{
    public class ItemRecipe
    {
        public string ItemResult { get; set; }
        public Dictionary<string, int> Ingredients { get; set; } = new Dictionary<string, int>();
    }
}
