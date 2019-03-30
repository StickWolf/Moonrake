using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class TradeSet
    {
        public List<ItemRecipe> Recipes { get; set; } = new List<ItemRecipe>();
        public string Name { get; set; }
    }
}
