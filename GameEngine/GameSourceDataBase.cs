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

        private Dictionary<string, Character> Characters { get; set; } = new Dictionary<string, Character>();

        private Dictionary<string, Location> Locations { get; set; } = new Dictionary<string, Location>();

        public string StartingLocationName { get; protected set; }

        public List<Portal> Portals { get; private set; } = new List<Portal>();

        public Dictionary<string, string> DefaultGameVars { get; private set; } = new Dictionary<string, string>();

        private Dictionary<string, Item> Items { get; set; } = new Dictionary<string, Item>();

        private Dictionary<string, TradeSet> TradeSets { get; set; } = new Dictionary<string, TradeSet>();

        /// <summary>
        /// Helper method to add a location
        /// </summary>
        /// <param name="location">The location to add.</param>
        /// <returns>The location name</returns>
        public string AddLocation(Location location)
        {
            Locations[location.Name] = location;
            return location.Name;
        }

        /// <summary>
        /// Gets a location if it exists
        /// </summary>
        /// <param name="locationName">The name of the location to get</param>
        /// <param name="location">The location if it exists</param>
        /// <returns>True or false if the location exists</returns>
        public bool TryGetLocation(string locationName, out Location location)
        {
            if (Locations.ContainsKey(locationName))
            {
                location = Locations[locationName];
                return true;
            }
            location = null;
            return false;
        }

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

        /// <summary>
        /// Gets an item if it exists
        /// </summary>
        /// <param name="itemName">The item name to try and get</param>
        /// <param name="item">The item if it exists</param>
        /// <returns>True/False depending on if the item exists</returns>
        public bool TryGetItem(string itemName, out Item item)
        {
            if (Items.ContainsKey(itemName))
            {
                item = Items[itemName];
                return true;
            }
            item = null;
            return false;
        }

        /// <summary>
        /// Helper method to add a character
        /// </summary>
        /// <param name="character">The character to add</param>
        /// <returns>The name of the character</returns>
        public string AddCharacter(Character character)
        {
            Characters[character.Name] = character;
            return character.Name;
        }

        /// <summary>
        /// Gets a character if it exists
        /// </summary>
        /// <param name="characterName">The name of the character to try and get</param>
        /// <param name="character">The character if it exists</param>
        /// <returns>True/False depending on if the character exists</returns>
        public bool TryGetCharacter(string characterName, out Character character)
        {
            if (Characters.ContainsKey(characterName))
            {
                character = Characters[characterName];
                return true;
            }
            character = null;
            return false;
        }

        /// <summary>
        /// Helper method to add a trade set
        /// </summary>
        /// <param name="tradeSet">The trade set to add</param>
        /// <returns>The name of the trade set</returns>
        public string AddTradeSet(TradeSet tradeSet)
        {
            TradeSets[tradeSet.Name] = tradeSet;
            return tradeSet.Name;
        }

        /// <summary>
        /// Gets a trade set if it exists
        /// </summary>
        /// <param name="tradeSetName">The name of the trade set to try and get</param>
        /// <param name="tradeSet">The trade set if it exists</param>
        /// <returns>True/False depending on if the trade set exists</returns>
        public bool TryGetTradeSet(string tradeSetName, out TradeSet tradeSet)
        {
            if (TradeSets.ContainsKey(tradeSetName))
            {
                tradeSet = TradeSets[tradeSetName];
                return true;
            }
            tradeSet = null;
            return false;
        }
    }
}
