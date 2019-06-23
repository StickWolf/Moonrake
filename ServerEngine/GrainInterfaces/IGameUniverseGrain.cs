using Orleans;
using ServerEngine.Characters;
using ServerEngine.Characters.Behaviors;
using ServerEngine.Commands.GameCommands;
using ServerEngine.Locations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerEngine.GrainInterfaces
{
    public interface IGameUniverseGrain : IGrainWithStringKey
    {
        /// <summary>
        /// Gets a string that represents details about the game universe.
        /// These details contain information such as when the universe was created, how many accounts it has, etc.
        /// </summary>
        /// <returns></returns>
        Task<string> GetStats();

        Task<IAccountGrain> CreateAccount(string userName, string password);

        Task<IAccountGrain> GetAccount(string userName);

        Task<IAccountGrain> GetSysopAccount();

        Task<string> GetGameIntroductionText();

        Task SetGameIntroductionText(string value);

        /// <summary>
        /// Gets a game variable by its full name
        /// </summary>
        /// <param name="gameVariableName">The name of the game variable to get</param>
        /// <returns>The value or null if it's not set.</returns>
        Task<string> GetGameVarValue(string gameVariableName);

        /// <summary>
        /// Sets the game variable to the specified value
        /// </summary>
        /// <param name="gameVariableName">The game variable to set</param>
        /// <param name="value">The value to set the game variable to</param>
        Task<string> SetGameVarValue(string gameVariableName, string value);

        Task<Guid> AddTradeSet(TradeSet tradeSet);

        Task<Guid> AddTradeSet(string tradesetName, params ItemRecipe[] itemRecipes);

        Task<TradeSet> GetTradeSet(Guid tradeSetTrackingId);

        Task AddGameCommand(IGameCommand command);

        Task<IGameCommand> GetGameCommand(string commandName, List<string> accountPermissions);

        Task AddTurnBehavior(string behaviorName, ITurnBehavior behavior);

        Task<ITurnBehavior> GetTurnBehavior(string behaviorName);

        Task<Guid> AddTradePost(Guid locationTrackingId, string tradePostName, params Guid[] tradeSetTrackingIds);

        Task<Guid> AddTradePost(TradePost tradePost, Guid locationTrackingId);

        Task<TradePost> GetTradePost(Guid tradePostTrackingId);

        /// <summary>
        /// Adds/sets a tradepost to be at the specified location
        /// </summary>
        /// <param name="tradePostTrackingId">The name of the trade post</param>
        /// <param name="locationTrackingId">The location the trade post is at</param>
        Task SetTradePostLocation(Guid tradePostTrackingId, Guid locationTrackingId);

        /// <summary>
        /// Gets all the trade posts at the given location
        /// </summary>
        /// <param name="locationTrackingId">The location to look at</param>
        /// <returns>All the trade posts at the given location</returns>
        Task<List<TradePost>> GetTradePostsAtLocation(Guid locationTrackingId);

        Task<List<Character>> GetCharactersInLocation(Guid locationTrackingId);

        /// <summary>
        /// Gets the location of the specified character
        /// </summary>
        /// <param name="characterTrackingId">The character name</param>
        /// <returns>Character location or null</returns>
        Task<Location> GetCharacterLocation(Guid characterTrackingId);

        /// <summary>
        /// Sets the location of the specified character
        /// </summary>
        /// <param name="characterTrackingId">The Name of the character</param>
        /// <param name="locationTrackingId">The location to place the character at</param>
        Task SetCharacterLocation(Guid characterTrackingId, Guid locationTrackingId);

        Task<Guid> AddCharacter(Character character, Guid locationTrackingId);

        Task<Character> GetCharacter(Guid characterTrackingId);

        Task<Dictionary<Item, int>> GetCharacterItems(Guid characterTrackingId);

        Task<Dictionary<Item, int>> GetLocationItems(Guid locationTrackingId);

        Task<Character> GetNextTurnNPC();

        Task<Guid> AddItem(Item item);

        Task<Item> GetItem(Guid itemTrackingId);

        Task<bool> IsPlayerCharacterNameInUse(string characterName);

        Task<Guid> AddPortal(params PortalRule[] destinationRules);

        Task<Guid> AddPortal(Portal portal);

        Task<Portal> GetPortal(Guid portalTrackingId);

        Task<List<Portal>> GetPortalsInLocation(Guid originLocationTrackingId);

        Task RemoveItemEveryWhere(Guid itemTrackingId);

        Task<bool> TryAddCharacterItemCount(Guid characterTrackingId, Guid itemTrackingId, int count);

        Task<bool> TryAddLocationItemCount(Guid locationTrackingId, Guid itemTrackingId, int count);

        /// Clones the specified item so that there are 2 instances of it in the Items.
        /// The clone won't have any references to it and is free to mutate directly after cloning.
        /// The clone will have a new tracking Id assigned.
        /// </summary>
        /// <param name="sourceItemTrackingId">The source item tracking id to clone</param>
        /// <returns>The cloned item</returns>
        Task ConvertItemToClone(Guid sourceItemTrackingId);

        Task DedupeItems();

        Task<Guid> AddLocation(Location location);

        Task<Location> GetLocation(Guid locationTrackingId);

        /// <summary>
        /// Gets a list of locations that are currently available to move into from the specified location
        /// </summary>
        /// <param name="originLocationTrackingId">The origin location tracking id</param>
        /// <returns>A list of open portals</returns>
        Task<List<Location>> GetConnectedLocations(Guid originLocationTrackingId);

        Task SetCustom(object custom);

        Task<object> GetCustom();
    }
}
