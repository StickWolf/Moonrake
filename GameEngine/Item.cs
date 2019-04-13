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

        /// <summary>
        /// Gets a description of what the item looks like.
        /// </summary>
        /// <param name="count">The number of items that are present</param>
        /// <param name="gameData">The gamedata for the game</param>
        /// <returns>A description</returns>
        public virtual string GetDescription(int count, GameSourceData gameData, GameState currentGameState)
        {
            if (count == 1)
            {
                return $"a {Name}";
            }

            return $"{count} {Name}s";
        }
    }
}
