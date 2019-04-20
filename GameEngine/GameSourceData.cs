using GameEngine.Locations;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GameEngine
{
    /// <summary>
    /// This class provides methods that help create a GameSourceData
    /// </summary>
    public abstract class GameSourceData
    {
        public string DefaultPlayerName { get; protected set; }

        public string GameIntroductionText { get; protected set; }

        public string GameEndingText { get; protected set; }

        private Dictionary<string, Character> Characters { get; set; } = new Dictionary<string, Character>();

        private Dictionary<string, Location> Locations { get; set; } = new Dictionary<string, Location>();

        public List<Portal> Portals { get; private set; } = new List<Portal>();

        public Dictionary<string, string> DefaultGameVars { get; private set; } = new Dictionary<string, string>();

        // Items[{ItemTrackingName}] = {Item}
        private Dictionary<string, Item> Items { get; set; } = new Dictionary<string, Item>();

        // DefaultCharacterItems[{CharacterName}][{ItemTrackingName}] = {Count}
        public Dictionary<string, Dictionary<string, int>> DefaultCharacterItems = new Dictionary<string, Dictionary<string, int>>();

        // DefaultCharacterItems[{LocationName}][{ItemTrackingName}] = {Count}
        public Dictionary<string, Dictionary<string, int>> DefaultLocationItems { get; set; } = new Dictionary<string, Dictionary<string, int>>();

        private Dictionary<string, TradeSet> TradeSets { get; set; } = new Dictionary<string, TradeSet>();

        private Dictionary<string, TradePost> TradePosts { get; set; } = new Dictionary<string, TradePost>();

        public Dictionary<string, string> DefaultTradePostLocations { get; private set; } = new Dictionary<string, string>();

        public Dictionary<string, string> DefaultCharacterLocations { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Helper method to add a location
        /// </summary>
        /// <param name="location">The location to add.</param>
        /// <returns>The location name</returns>
        public string AddLocation(Location location)
        {
            Debug.Assert(!Locations.ContainsKey(location.Name), $"A location with the same name '{location.Name}' is being added twice to the game data. Check the code to make sure this doesn't happen.");

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
            Debug.Assert(!DefaultGameVars.ContainsKey(gameVarName), $"A default game var with the same name '{gameVarName}' is being added twice to the game data. Check the code to make sure this doesn't happen.");

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
            Debug.Assert(!Items.ContainsKey(item.TrackingName), $"An item with the same tracking name '{item.TrackingName}' is being added twice to the game data. Check the code to make sure this doesn't happen.");

            Items[item.TrackingName] = item;
            return item.TrackingName;
        }

        /// <summary>
        /// Gets an item if it exists
        /// </summary>
        /// <param name="itemTrackingName">The tracking item name to try and get</param>
        /// <param name="item">The item if it exists</param>
        /// <returns>True/False depending on if the item exists</returns>
        public bool TryGetItem(string itemTrackingName, out Item item)
        {
            if (Items.ContainsKey(itemTrackingName))
            {
                item = Items[itemTrackingName];
                return true;
            }
            item = null;
            return false;
        }

        /// <summary>
        /// Gets an item if it exists
        /// </summary>
        /// <param name="itemTrackingName">The tracking name of the item to get</param>
        /// <returns>The item or null if it doesn't exist</returns>
        public Item GetItem(string itemTrackingName)
        {
            return Items.ContainsKey(itemTrackingName) ? Items[itemTrackingName] : null;
        }

        /// <summary>
        /// Adds an item that a character will be holding when the game starts
        /// </summary>
        /// <param name="characterName">The character to add the item to</param>
        /// <param name="itemTrackingName">The tracking name of the item</param>
        /// <param name="itemCount">How many of the item to add</param>
        public void AddDefaultCharacterItem(string characterName, string itemTrackingName, int itemCount)
        {
            if (!DefaultCharacterItems.ContainsKey(characterName))
            {
                DefaultCharacterItems[characterName] = new Dictionary<string, int>();
            }

            Debug.Assert(!DefaultCharacterItems[characterName].ContainsKey(itemTrackingName), $"Default character item '{itemTrackingName}' for character '{characterName}' has already been set. Check the code and make sure this item is only set 1 time for this character.");
            DefaultCharacterItems[characterName][itemTrackingName] = itemCount;
        }

        public void AddDefaultLocationItem(string locationName, string itemTrackingName, int itemCount)
        {
            if (!DefaultLocationItems.ContainsKey(locationName))
            {
                DefaultLocationItems[locationName] = new Dictionary<string, int>();
            }

            Debug.Assert(!DefaultLocationItems[locationName].ContainsKey(itemTrackingName), $"Default location item '{itemTrackingName}' for location '{locationName}' has already been set. Check the code and make sure this item is only set 1 time for this location.");
            DefaultLocationItems[locationName][itemTrackingName] = itemCount;
        }

        public void AddDefaultCharacterLocation(string characterName, string locationName)
        {
            Debug.Assert(!DefaultCharacterLocations.ContainsKey(characterName), $"A default character location has already been added for character '{characterName}'. Check the code to make sure it only gets added 1 time.");

            DefaultCharacterLocations.Add(characterName, locationName);
        }

        /// <summary>
        /// Helper method to add a character
        /// </summary>
        /// <param name="character">The character to add</param>
        /// <returns>The name of the character</returns>
        public string AddCharacter(Character character)
        {
            Debug.Assert(!Characters.ContainsKey(character.Name), $"A character with the name '{character.Name}' has already been added. Check the code to make sure it only gets added 1 time.");

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
        /// <param name="tradesetName">The name of the trade set to add</param>
        /// <param name="itemRecipes">A list of one or more item recipes to add</param>
        /// <returns>The name of the trade set</returns>
        public string AddTradeSet(string tradesetName, params ItemRecipe[] itemRecipes)
        {
            Debug.Assert(!TradeSets.ContainsKey(tradesetName), $"A tradeset with the name '{tradesetName}' has already been added. Check the code to make sure it only gets added 1 time.");

            var tradeSet = new TradeSet(tradesetName, itemRecipes);
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

        /// <summary>
        /// Helper method to add a trade post
        /// </summary>
        /// <param name="tradePostName">The name of the trade post to add</param>
        /// <param name="tradeSetNames">A list of one or more trade sets to add</param>
        /// <returns>The name of the trade set</returns>
        public string AddTradePost(string tradePostName, params string[] tradeSetNames)
        {
            Debug.Assert(!TradePosts.ContainsKey(tradePostName), $"A tradepost with the name '{tradePostName}' has already been added. Check the code to make sure it only gets added 1 time.");

            var tradePost = new TradePost(tradePostName, tradeSetNames);
            TradePosts[tradePost.Name] = tradePost;
            return tradePost.Name;
        }

        /// <summary>
        /// Gets a trade post if it exists
        /// </summary>
        /// <param name="tradePostName">The name of the trade set to try and get</param>
        /// <param name="tradePost">The trade post if it exists</param>
        /// <returns>True/False depending on if the trade post exists</returns>
        public bool TryGetTradePost(string tradePostName, out TradePost tradePost)
        {
            if (TradeSets.ContainsKey(tradePostName))
            {
                tradePost = TradePosts[tradePostName];
                return true;
            }
            tradePost = null;
            return false;
        }
    }
}
