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

        private Dictionary<Guid, Character> Characters { get; set; } = new Dictionary<Guid, Character>();
        public Dictionary<Guid, Character> TODO_Delete_And_Use_GameState_Instead_Characters { get { return Characters; } }

        private Dictionary<Guid, Location> Locations { get; set; } = new Dictionary<Guid, Location>();
        public Dictionary<Guid, Location> TODO_Delete_And_Use_GameState_Instead_Locations { get { return Locations; } }

        private Dictionary<Guid, Portal> Portals { get; set; } = new Dictionary<Guid, Portal>();
        public Dictionary<Guid, Portal> TODO_Delete_And_Use_GameState_Instead_Portals { get { return Portals; } }

        private Dictionary<Guid, TradeSet> TradeSets { get; set; } = new Dictionary<Guid, TradeSet>();
        public Dictionary<Guid, TradeSet> TODO_Delete_And_Use_GameState_Instead_TradeSets { get { return TradeSets; } }

        private Dictionary<Guid, TradePost> TradePosts { get; set; } = new Dictionary<Guid, TradePost>();
        public Dictionary<Guid, TradePost> TODO_Delete_And_Use_GameState_Instead_TradePosts { get { return TradePosts; } }

        private Dictionary<Guid, Guid> DefaultTradePostLocations { get; set; } = new Dictionary<Guid, Guid>();
        public Dictionary<Guid, Guid> TODO_Delete_And_Use_GameState_Instead_DefaultTradePostLocations { get { return DefaultTradePostLocations; } }

        private Dictionary<Guid, Guid> DefaultCharacterLocations { get; set; } = new Dictionary<Guid, Guid>();
        public Dictionary<Guid, Guid> TODO_Delete_And_Use_GameState_Instead_DefaultCharacterLocations { get { return DefaultCharacterLocations; } }

        private Dictionary<string, string> DefaultGameVars { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> TODO_Delete_And_Use_GameState_Instead_DefaultGameVars { get { return DefaultGameVars; } }

        private Dictionary<Guid, Item> Items { get; set; } = new Dictionary<Guid, Item>();
        public Dictionary<Guid, Item> TODO_Delete_And_Use_GameState_Instead_Items { get { return Items; } }

        private Dictionary<Guid, Dictionary<Guid, int>> DefaultCharacterItems { get; set; } = new Dictionary<Guid, Dictionary<Guid, int>>();
        public Dictionary<Guid, Dictionary<Guid, int>> TODO_Delete_And_Use_GameState_Instead_DefaultCharacterItems { get { return DefaultCharacterItems; } }

        private Dictionary<Guid, Dictionary<Guid, int>> DefaultLocationItems { get; set; } = new Dictionary<Guid, Dictionary<Guid, int>>();
        public Dictionary<Guid, Dictionary<Guid, int>> TODO_Delete_And_Use_GameState_Instead_DefaultLocationItems { get { return DefaultLocationItems; } }



        public Guid AddLocation(Location location)
        {
            Locations[location.TrackingId] = location;
            return location.TrackingId;
        }

        public Guid AddPortal(params PortalRule[] destinationRules)
        {
            var portal = new Portal(destinationRules.ToList());
            Portals[portal.TrackingId] = portal;
            return portal.TrackingId;
        }

        public string AddDefaultGameVar(string gameVarName, string gameVarValue)
        {
            Debug.Assert(!DefaultGameVars.ContainsKey(gameVarName), $"A default game var with the same name '{gameVarName}' is being added twice to the game data. Check the code to make sure this doesn't happen.");

            DefaultGameVars[gameVarName] = gameVarValue;
            return gameVarName;
        }

        public Guid AddItem(Item item)
        {
            Items[item.TrackingId] = item;
            return item.TrackingId;
        }

        public void AddDefaultCharacterItem(Guid characterTrackingId, Guid itemTrackingId, int itemCount)
        {
            if (!DefaultCharacterItems.ContainsKey(characterTrackingId))
            {
                DefaultCharacterItems[characterTrackingId] = new Dictionary<Guid, int>();
            }
            DefaultCharacterItems[characterTrackingId][itemTrackingId] = itemCount;
        }

        public void AddDefaultLocationItem(Guid locationTrackingId, Guid itemTrackingId, int itemCount)
        {
            if (!DefaultLocationItems.ContainsKey(locationTrackingId))
            {
                DefaultLocationItems[locationTrackingId] = new Dictionary<Guid, int>();
            }

            Debug.Assert(!DefaultLocationItems[locationTrackingId].ContainsKey(itemTrackingId), $"Default location item '{itemTrackingId}' for location '{locationTrackingId}' has already been set. Check the code and make sure this item is only set 1 time for this location.");
            DefaultLocationItems[locationTrackingId][itemTrackingId] = itemCount;
        }

        public Guid AddCharacter(Character character, Guid locationTrackingId)
        {
            Characters[character.TrackingId] = character;
            DefaultCharacterLocations[character.TrackingId] = locationTrackingId;
            return character.TrackingId;
        }

        public Guid AddTradeSet(string tradesetName, params ItemRecipe[] itemRecipes)
        {
            var tradeSet = new TradeSet(tradesetName, itemRecipes);
            TradeSets[tradeSet.TrackingId] = tradeSet;
            return tradeSet.TrackingId;
        }

        public Guid AddTradePost(Guid locationTrackingId, string tradePostName, params Guid[] tradeSetTrackingIds)
        {
            var tradePost = new TradePost(tradePostName, tradeSetTrackingIds);
            TradePosts[tradePost.TrackingId] = tradePost;
            DefaultTradePostLocations[tradePost.TrackingId] = locationTrackingId;
            return tradePost.TrackingId;
        }
    }
}
