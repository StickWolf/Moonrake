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

        public Dictionary<string, Dictionary<string, int>> DefaultCharacterItems = new Dictionary<string, Dictionary<string, int>>();

        public Dictionary<string, Dictionary<string, int>> DefaultLocationItems { get; set; } = new Dictionary<string, Dictionary<string, int>>();

        private Dictionary<string, TradeSet> TradeSets { get; set; } = new Dictionary<string, TradeSet>();

        private Dictionary<string, TradePost> TradePosts { get; set; } = new Dictionary<string, TradePost>();

        public Dictionary<string, string> DefaultTradePostLocations { get; private set; } = new Dictionary<string, string>();

        // TODO: Decide how to represent eventing systems like in the example below
        //      e.g.
        //         1. The Furnace Room initially has no location items in it.
        //         2. The player pulls a lever on the wall. This triggers an "Event", there can be many different things that happen
        //            during an event, however the only thing that this event does is:
        //              AddLocationItem("Furnace Room", "Key", 1, "You see a shiny key fall from a now open crack in the wall", KeyNotFoundCondition)
        //              This particular even shows the message and adds the location item to the location, but only if the KeyNotFoundCondition is true
        //              (whatever that is)
        //      etc. Events would be able to do a number of other things based on game var conditions.
        //      Another example would be that the player could use the key at a certain portal (door), which would trigger an event and the event would
        //          ModifyGameVar("FurnaceRoomDoorOpen", "true", "You hear a loud creaking sounds as the door slowly budges open")
        //      Portal rules then could reference this game var to determine if the portal is open or not.

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
        /// Adds an item that a character will be holding when the game starts
        /// </summary>
        /// <param name="characterName">The character to add the item to</param>
        /// <param name="itemName">The name of the item</param>
        /// <param name="itemCount">How many of the item to add</param>
        public void AddDefaultCharacterItem(string characterName, string itemName, int itemCount)
        {
            if (!DefaultCharacterItems.ContainsKey(characterName))
            {
                DefaultCharacterItems[characterName] = new Dictionary<string, int>();
            }

            DefaultCharacterItems[characterName][itemName] = itemCount;
        }

        public void AddDefaultLocationItem(string locationName, string itemName, int itemCount)
        {
            if (!DefaultLocationItems.ContainsKey(locationName))
            {
                DefaultLocationItems[locationName] = new Dictionary<string, int>();
            }
            DefaultLocationItems[locationName][itemName] = itemCount;
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
        /// <param name="tradesetName">The name of the trade set to add</param>
        /// <param name="itemRecipes">A list of one or more item recipes to add</param>
        /// <returns>The name of the trade set</returns>
        public string AddTradeSet(string tradesetName, params ItemRecipe[] itemRecipes)
        {
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
