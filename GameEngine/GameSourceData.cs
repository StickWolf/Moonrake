using GameEngine.Characters;
using GameEngine.Locations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GameEngine
{
    public abstract class GameSourceData
    {
        public string GameIntroductionText { get; protected set; }

        public string GameEndingText { get; protected set; }

        public Dictionary<Guid, Character> Characters { get; set; } = new Dictionary<Guid, Character>();

        private Dictionary<string, Location> Locations { get; set; } = new Dictionary<string, Location>();

        public List<Portal> Portals { get; private set; } = new List<Portal>();

        public Dictionary<string, string> DefaultGameVars { get; private set; } = new Dictionary<string, string>();

        private Dictionary<string, Item> Items { get; set; } = new Dictionary<string, Item>();

        public Dictionary<Guid, Dictionary<string, int>> DefaultCharacterItems = new Dictionary<Guid, Dictionary<string, int>>();

        public Dictionary<string, Dictionary<string, int>> DefaultLocationItems { get; set; } = new Dictionary<string, Dictionary<string, int>>();

        private Dictionary<string, TradeSet> TradeSets { get; set; } = new Dictionary<string, TradeSet>();

        private Dictionary<string, TradePost> TradePosts { get; set; } = new Dictionary<string, TradePost>();

        public Dictionary<string, string> DefaultTradePostLocations { get; private set; } = new Dictionary<string, string>();

        public Dictionary<Guid, string> DefaultCharacterLocations { get; set; } = new Dictionary<Guid, string>();

        public string AddLocation(Location location)
        {
            Debug.Assert(!Locations.ContainsKey(location.Name), $"A location with the same name '{location.Name}' is being added twice to the game data. Check the code to make sure this doesn't happen.");

            Locations[location.Name] = location;
            return location.Name;
        }

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

        public Location GetLocation(string locationName)
        {
            return Locations.ContainsKey(locationName) ? Locations[locationName] : null;
        }

        public void AddPortal(params PortalRule[] destinationRules)
        {
            Portals.Add(new Portal(destinationRules.ToList()));
        }

        public string AddDefaultGameVar(string gameVarName, string gameVarValue)
        {
            Debug.Assert(!DefaultGameVars.ContainsKey(gameVarName), $"A default game var with the same name '{gameVarName}' is being added twice to the game data. Check the code to make sure this doesn't happen.");

            DefaultGameVars[gameVarName] = gameVarValue;
            return gameVarName;
        }
        
        public string AddItem(Item item)
        {
            Debug.Assert(!Items.ContainsKey(item.TrackingName), $"An item with the same tracking name '{item.TrackingName}' is being added twice to the game data. Check the code to make sure this doesn't happen.");

            Items[item.TrackingName] = item;
            return item.TrackingName;
        }

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

        public Item GetItem(string itemTrackingName)
        {
            return Items.ContainsKey(itemTrackingName) ? Items[itemTrackingName] : null;
        }

        public void AddDefaultCharacterItem(Guid characterTrackingId, string itemTrackingName, int itemCount)
        {
            if (!DefaultCharacterItems.ContainsKey(characterTrackingId))
            {
                DefaultCharacterItems[characterTrackingId] = new Dictionary<string, int>();
            }
            DefaultCharacterItems[characterTrackingId][itemTrackingName] = itemCount;
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

        public Guid AddCharacter(Character character, string locationName)
        {
            Characters[character.TrackingId] = character;
            DefaultCharacterLocations[character.TrackingId] = locationName;
            return character.TrackingId;
        }

        public string AddTradeSet(string tradesetName, params ItemRecipe[] itemRecipes)
        {
            Debug.Assert(!TradeSets.ContainsKey(tradesetName), $"A tradeset with the name '{tradesetName}' has already been added. Check the code to make sure it only gets added 1 time.");

            var tradeSet = new TradeSet(tradesetName, itemRecipes);
            TradeSets[tradeSet.Name] = tradeSet;
            return tradeSet.Name;
        }

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

        public string AddTradePost(string tradePostName, params string[] tradeSetNames)
        {
            Debug.Assert(!TradePosts.ContainsKey(tradePostName), $"A tradepost with the name '{tradePostName}' has already been added. Check the code to make sure it only gets added 1 time.");

            var tradePost = new TradePost(tradePostName, tradeSetNames);
            TradePosts[tradePost.Name] = tradePost;
            return tradePost.Name;
        }

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
