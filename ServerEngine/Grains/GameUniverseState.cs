using ServerEngine.Characters;
using ServerEngine.Characters.Behaviors;
using ServerEngine.Commands.GameCommands;
using ServerEngine.GrainInterfaces;
using ServerEngine.Locations;
using System;
using System.Collections.Generic;

namespace ServerEngine.Grains
{
    public class GameUniverseState
    {
        public object Custom { get; set; }

        public string GameIntroductionText { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.Now;

        public Dictionary<string, IAccountGrain> Accounts { get; set; } = new Dictionary<string, IAccountGrain>();

        // GameVars[{GameVarName}] = {GameVarValue}
        public Dictionary<string, string> GameVars { get; set; } = new Dictionary<string, string>();

        public DateTime LastAccountCreationTime { get; set; } = DateTime.MinValue;

        // Only 1 account can be created per 10 minutes
        // TODO: Temp limiters for testing, either expose as an option to be set by gamedata or come up with something better
        public TimeSpan LastAccountCreationTimeSpanCooldown = TimeSpan.FromMinutes(10);

        public List<IGameCommand> GameCommands { get; set; } = new List<IGameCommand>();

        public Dictionary<string, ITurnBehavior> TurnBehaviors { get; set; } = new Dictionary<string, ITurnBehavior>();

        // TradeSets[{TradeSetTrackingId}] = {TradeSet}
        public Dictionary<Guid, TradeSet> TradeSets { get; set; } = new Dictionary<Guid, TradeSet>();

        // TradePosts[{TradePostTrackingId}] = {TradePost}
        public Dictionary<Guid, TradePost> TradePosts { get; set; } = new Dictionary<Guid, TradePost>();

        // CurrentTradePostLocations[{TradePostTrackingId}] = {LocationTrackingId}
        public Dictionary<Guid, Guid> CurrentTradePostLocations { get; set; } = new Dictionary<Guid, Guid>();

        // CharacterLocations[{CharacterTrackingId}] = {LocationTrackingId}
        public Dictionary<Guid, Guid> CharacterLocations { get; set; } = new Dictionary<Guid, Guid>();

        // Characters[{CharacterTrackingId}] = {Character}
        public Dictionary<Guid, Character> Characters { get; set; } = new Dictionary<Guid, Character>();

        // Locations[{LocationTrackingId}] = {Location}
        public Dictionary<Guid, Location> Locations { get; set; } = new Dictionary<Guid, Location>();

        // Items[{ItemTrackingId}] = {Item}
        public Dictionary<Guid, Item> Items { get; set; } = new Dictionary<Guid, Item>();

        // Portals[{PortalTrackingId}] = {Portal}
        public Dictionary<Guid, Portal> Portals { get; set; } = new Dictionary<Guid, Portal>();

        // CharacterItems[{CharacterTrackingId}][{ItemTrackingId}] = {ItemCount}
        public Dictionary<Guid, Dictionary<Guid, int>> CharactersItems { get; set; } = new Dictionary<Guid, Dictionary<Guid, int>>();

        // LocationItems[{LocationTrackingId}][{ItemTrackingId}] = {ItemCount}
        public Dictionary<Guid, Dictionary<Guid, int>> LocationItems { get; set; } = new Dictionary<Guid, Dictionary<Guid, int>>();
    }
}
