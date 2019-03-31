using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Item
    {
        /// <summary>
        /// Name of the item
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Tells if the item is unique (there can be only 1), or not, in which case there could be many.
        /// The game will take care of assuring that only 1 is held by any character at a time.
        /// </summary>
        public bool IsUnique { get; set;}

        public Item(string name)
        {
            Name = name;
        }

        // TODO: Implement location items
        //      Location items are items that are simply on the floor of a specified location. They can get there
        //      by default when the game starts or by a character dropping an item and leaving it there. Location
        //      items are different than tradeposts in that you don't have to trade for them to get them, instead
        //      all you need to do is pick it up.
        //      After location items are implemented, the drop and pickup command can also be implemented.
    }
}
