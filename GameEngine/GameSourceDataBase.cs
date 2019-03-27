using GameEngine.Locations;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    /// <summary>
    /// This class provides methods that help create a GameSourceData
    /// </summary>
    public abstract class GameSourceDataBase
    {
        public string DefaultPlayerName { get; protected set; }

        public string GameIntroductionText { get; protected set; }

        public List<Character> Characters { get; private set; } = new List<Character>();

        public List<Location> Locations { get; private set; } = new List<Location>();

        public string StartingLocationName { get; protected set; }

        public List<Portal> Portals { get; private set; } = new List<Portal>();

        public Dictionary<string, string> DefaultGameVars { get; private set; } = new Dictionary<string, string>();

        public Dictionary<string, Item> Items { get; private set; } = new Dictionary<string, Item>();

        /// <summary>
        /// Helper method that can be used to add a portal more elegantly
        /// </summary>
        /// <param name="destinationRules">An array of portal rules to apply in order to the portal</param>
        public void AddPortal(params PortalRule[] destinationRules)
        {
            Portals.Add(new Portal(destinationRules.ToList()));
        }

        /// <summary>
        /// Helper to add a default game variable value
        /// </summary>
        /// <param name="gameVarName">The name of the game variable to add</param>
        /// <param name="gameVarValue">The value of the what the variable should be set to by default</param>
        /// <returns>The game variable name</returns>
        public string AddDefaultGameVar(string gameVarName, string gameVarValue)
        {
            DefaultGameVars[gameVarName] = gameVarValue;
            return gameVarName;
        }
        
        /// <summary>
        /// Helper to add items
        /// </summary>
        /// <param name="item">the item to add</param>
        /// <returns>the name of the item</returns>
        public string AddItem(Item item)
        {
            Items[item.Name] = item;
            return item.Name;
        }
    }
}
