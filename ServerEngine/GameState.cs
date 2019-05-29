﻿using Newtonsoft.Json;
using ServerEngine.Characters;
using ServerEngine.Characters.Behaviors;
using ServerEngine.Commands.GameCommands;
using ServerEngine.Locations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ServerEngine
{
    /// <summary>
    /// The GameState class is meant to represent the current state of the game.
    /// This class holds any data that should be saved and reloaded for saved games.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class GameState
    {
        [JsonProperty]
        public string GameIntroductionText { get; set; }

        [JsonProperty]
        public object Custom { get; set; }

        [JsonProperty]
        private List<IGameCommand> GameCommands { get; set; } = new List<IGameCommand>();

        // Characters[{CharacterTrackingId}] = {Character}
        [JsonProperty]
        private Dictionary<Guid, Character> Characters { get; set; } = new Dictionary<Guid, Character>();
        private object characterLock = new object();

        // Locations[{LocationTrackingId}] = {Location}
        [JsonProperty]
        private Dictionary<Guid, Location> Locations { get; set; } = new Dictionary<Guid, Location>();
        private object locationLock = new object();

        // Items[{ItemTrackingId}] = {Item}
        [JsonProperty]
        private Dictionary<Guid, Item> Items { get; set; } = new Dictionary<Guid, Item>();
        private object itemLock = new object();

        // Portals[{PortalTrackingId}] = {Portal}
        [JsonProperty]
        private Dictionary<Guid, Portal> Portals { get; set; } = new Dictionary<Guid, Portal>();
        private object portalLock = new object();

        // CharacterItems[{CharacterTrackingId}][{ItemTrackingId}] = {ItemCount}
        [JsonProperty]
        private Dictionary<Guid, Dictionary<Guid, int>> CharactersItems { get; set; } = new Dictionary<Guid, Dictionary<Guid, int>>();

        // LocationItems[{LocationTrackingId}][{ItemTrackingId}] = {ItemCount}
        [JsonProperty]
        private Dictionary<Guid, Dictionary<Guid, int>> LocationItems { get; set; } = new Dictionary<Guid, Dictionary<Guid, int>>();

        // CharacterLocations[{CharacterTrackingId}] = {LocationTrackingId}
        [JsonProperty]
        private Dictionary<Guid, Guid> CharacterLocations { get; set; } = new Dictionary<Guid, Guid>();

        // GameVars[{GameVarName}] = {GameVarValue}
        [JsonProperty]
        private Dictionary<string, string> GameVars { get; set; } = new Dictionary<string, string>();
        private object gameVarLock = new object();

        // TradeSets[{TradeSetTrackingId}] = {TradeSet}
        [JsonProperty]
        private Dictionary<Guid, TradeSet> TradeSets { get; set; } = new Dictionary<Guid, TradeSet>();
        private object tradeLock = new object();

        // TradePosts[{TradePostTrackingId}] = {TradePost}
        [JsonProperty]
        private Dictionary<Guid, TradePost> TradePosts { get; set; } = new Dictionary<Guid, TradePost>();

        // CurrentTradePostLocations[{TradePostTrackingId}] = {LocationTrackingId}
        [JsonProperty]
        private Dictionary<Guid, Guid> CurrentTradePostLocations { get; set; } = new Dictionary<Guid, Guid>();

        [JsonProperty]
        private Dictionary<string, ITurnBehavior> TurnBehaviors { get; set; } = new Dictionary<string, ITurnBehavior>();

        [JsonProperty]
        private Dictionary<string, Account> Accounts { get; set; } = new Dictionary<string, Account>();
        private object accountLock = new object();

        // Everything below (that does not have a [JsonProperty]) is excluded from save files

        public static GameState CurrentGameState { get; private set; }
        private static string SaveFileName { get; set; } = "GameSaves.json";

        public static List<string> GetValidSaveSlotNames()
        {
            // Create a new list
            List<string> saveList = new List<string>();

            var savedGamesDictionary = GetGameStates();
            // loop through all the items in the dictionary and add each slotname to the list
            foreach (var slotName in savedGamesDictionary.Keys)
            {
                saveList.Add(slotName);
            }
            // return the list
            return saveList;
        }

        public static void LoadGameState(string slotName)
        {
            var savedGamesDictionary = GetGameStates();
            // get the specified value out of the dictionary using slotName as the key
            CurrentGameState = savedGamesDictionary[slotName];
        }

        private static JsonSerializerSettings GetJsonSerializerSettings()
        {
            var serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                ContractResolver = new JsonPrivateSetResolver()
            };
            return serializerSettings;
        }

        public static void SaveGameState(string slotName)
        {
            var savedGamesDictionary = GetGameStates();

            // add (or update) the gamestate that was passed in into the dictionary we now have using slotName as the key
            savedGamesDictionary[slotName] = CurrentGameState;

            // serialize the dictionary to a string
            string serializedDictionary = JsonConvert.SerializeObject(savedGamesDictionary, Formatting.Indented, GetJsonSerializerSettings());

            // save that serialized string to the game saves file
            File.WriteAllText(SaveFileName, serializedDictionary);
        }

        private static Dictionary<string, GameState> GetGameStates()
        {
            Dictionary<string, GameState> savedGamesDictionary;
            var saveFile = new FileInfo(SaveFileName);

            // If there is an existing saves file
            if (saveFile.Exists)
            {
                // read the save file to a string
                string fileContents = File.ReadAllText(saveFile.FullName);

                // deserialize the string into a dictionary<string, gamestate>
                savedGamesDictionary = JsonConvert.DeserializeObject<Dictionary<string, GameState>>(fileContents, GetJsonSerializerSettings());
            }
            else
            {
                // create a new dictionary<string, gameState>
                savedGamesDictionary = new Dictionary<string, GameState>();
            }

            return savedGamesDictionary;
        }

        public static void CreateNewGameState()
        {
            CurrentGameState = new GameState();
        }

        public void RemoveItemEveryWhere(Guid itemTrackingId)
        {
            lock (characterLock)
            {
                lock (itemLock)
                {
                    lock (locationLock)
                    {
                        // Remove the item from all characters
                        foreach (var characterTrackingId in CharactersItems.Keys)
                        {
                            if (CharactersItems[characterTrackingId].ContainsKey(itemTrackingId))
                            {
                                CharactersItems[characterTrackingId].Remove(itemTrackingId);
                            }
                        }
                        var zeroCharKeys = CharactersItems.Where(kvp => !kvp.Value.Any()).Select(kvp => kvp.Key).ToList();
                        foreach (var key in zeroCharKeys)
                        {
                            CharactersItems.Remove(key);
                        }

                        // Remove the item from all locations
                        foreach (var locationName in LocationItems.Keys)
                        {
                            if (LocationItems[locationName].ContainsKey(itemTrackingId))
                            {
                                LocationItems[locationName].Remove(itemTrackingId);
                            }
                        }
                        var zeroLocationKeys = LocationItems.Where(kvp => !kvp.Value.Any()).Select(kvp => kvp.Key).ToList();
                        foreach (var key in zeroLocationKeys)
                        {
                            LocationItems.Remove(key);
                        }
                    }
                }
            }
        }

        public bool TryAddCharacterItemCount(Guid characterTrackingId, Guid itemTrackingId, int count)
        {
            lock (characterLock)
            {
                lock (itemLock)
                {
                    var item = GetItem(itemTrackingId);
                    if (item == null)
                    {
                        return false;
                    }

                    if (count == 0)
                    {
                        return true;
                    }

                    void setCharacterItemCount(Guid sCharacterTrackingId, Guid sItemTrackingId, int sCount)
                    {
                        if (!CharactersItems.ContainsKey(sCharacterTrackingId))
                        {
                            CharactersItems.Add(sCharacterTrackingId, new Dictionary<Guid, int>());
                        }
                        if (!CharactersItems[sCharacterTrackingId].ContainsKey(sItemTrackingId))
                        {
                            CharactersItems[sCharacterTrackingId].Add(sItemTrackingId, sCount);
                        }
                        else
                        {
                            CharactersItems[sCharacterTrackingId][sItemTrackingId] = sCount;
                        }
                    }

                    void removeCharacterItem(Guid sCharacterTrackingId, Guid sItemTrackingId)
                    {
                        if (!CharactersItems.ContainsKey(sCharacterTrackingId))
                        {
                            return;
                        }
                        if (CharactersItems[sCharacterTrackingId].ContainsKey(sItemTrackingId))
                        {
                            CharactersItems[sCharacterTrackingId].Remove(sItemTrackingId);
                            if (!CharactersItems[sCharacterTrackingId].Any())
                            {
                                CharactersItems.Remove(sCharacterTrackingId);
                            }
                        }
                    }

                    // The goal is to only keep records in the dictionary for counts greater than 0
                    // Even if we do temporarily add keys here, they should be cleaned up before returning if needed
                    var characterItemCount = 0;
                    if (CharactersItems.ContainsKey(characterTrackingId) && CharactersItems[characterTrackingId].ContainsKey(itemTrackingId))
                    {
                        characterItemCount = CharactersItems[characterTrackingId][itemTrackingId];
                    }

                    // Non-unique items can be added and removed from characters without worrying about the count
                    if (!item.IsUnique)
                    {
                        characterItemCount += count;
                        if (characterItemCount > 0)
                        {
                            setCharacterItemCount(characterTrackingId, itemTrackingId, characterItemCount);
                            return true;
                        }
                        else if (characterItemCount == 0)
                        {
                            removeCharacterItem(characterTrackingId, itemTrackingId);
                            return true;
                        }
                        return false;
                    }

                    // The item is unique, and we're removing it.
                    // Since this is a removal we don't need to worry about checking for duplicates being made
                    if (count < 0)
                    {
                        if (characterItemCount > 0)
                        {
                            removeCharacterItem(characterTrackingId, itemTrackingId);
                            return true;
                        }
                        return false;
                    }

                    // The only other option is that this is a unique item being added to the character
                    // First remove it from everywhere else, then add it here.
                    RemoveItemEveryWhere(itemTrackingId);
                    setCharacterItemCount(characterTrackingId, itemTrackingId, 1);
                    return true;
                }
            }
        }

        public bool TryAddLocationItemCount(Guid locationTrackingId, Guid itemTrackingId, int count)
        {
            lock (locationLock)
            {
                lock (itemLock)
                {
                    var item = GetItem(itemTrackingId);
                    if (item == null)
                    {
                        return false;
                    }

                    if (count == 0)
                    {
                        return true;
                    }

                    void setLocationItemCount(Guid sLocationTrackingId, Guid sItemTrackingId, int sCount)
                    {
                        if (!LocationItems.ContainsKey(sLocationTrackingId))
                        {
                            LocationItems.Add(sLocationTrackingId, new Dictionary<Guid, int>());
                        }
                        if (!LocationItems[sLocationTrackingId].ContainsKey(sItemTrackingId))
                        {
                            LocationItems[sLocationTrackingId].Add(sItemTrackingId, sCount);
                        }
                        else
                        {
                            LocationItems[sLocationTrackingId][sItemTrackingId] = sCount;
                        }
                    }

                    void removeLocationItem(Guid sLocationTrackingId, Guid sItemTrackingId)
                    {
                        if (!LocationItems.ContainsKey(sLocationTrackingId))
                        {
                            return;
                        }
                        if (LocationItems[sLocationTrackingId].ContainsKey(sItemTrackingId))
                        {
                            LocationItems[sLocationTrackingId].Remove(sItemTrackingId);
                            if (!LocationItems[sLocationTrackingId].Any())
                            {
                                LocationItems.Remove(sLocationTrackingId);
                            }
                        }
                    }

                    // The goal is to only keep records in the dictionary for counts greater than 0
                    // Even if we do temporarily add keys here, they should be cleaned up before returning if needed
                    var locationItemCount = 0;
                    if (LocationItems.ContainsKey(locationTrackingId) && LocationItems[locationTrackingId].ContainsKey(itemTrackingId))
                    {
                        locationItemCount = LocationItems[locationTrackingId][itemTrackingId];
                    }

                    // Non-unique items can be added and removed from locations without worrying about the count
                    if (!item.IsUnique)
                    {
                        locationItemCount += count;
                        if (locationItemCount > 0)
                        {
                            setLocationItemCount(locationTrackingId, itemTrackingId, locationItemCount);
                            return true;
                        }
                        else if (locationItemCount == 0)
                        {
                            removeLocationItem(locationTrackingId, itemTrackingId);
                            return true;
                        }
                        return false;
                    }

                    // The item is unique, and we're removing it.
                    // Since this is a removal we don't need to worry about checking for duplicates being made
                    if (count < 0)
                    {
                        // Double check to make sure there really is an item here
                        if (locationItemCount > 0)
                        {
                            removeLocationItem(locationTrackingId, itemTrackingId);
                            return true;
                        }
                        return false;
                    }

                    // The only other option is that this is a unique item being added to a location
                    // First remove it from everywhere else, then add it here.
                    RemoveItemEveryWhere(itemTrackingId);
                    setLocationItemCount(locationTrackingId, itemTrackingId, 1);
                    return true;
                }
            }
        }

        /// <summary>
        /// Clones the specified item so that there are 2 instances of it in the Items.
        /// The clone won't have any references to it and is free to mutate directly after cloning.
        /// The clone will have a new tracking Id assigned.
        /// </summary>
        /// <param name="sourceItemTrackingId">The source item tracking id to clone</param>
        /// <returns>The cloned item</returns>
        public void ConvertItemToClone(Guid sourceItemTrackingId)
        {
            lock (itemLock)
            {
                var sourceItem = GetItem(sourceItemTrackingId);
                Items.Remove(sourceItem.TrackingId);

                // Cloning just means serializing and deserializing to a new instance
                var serializerSettings = GetJsonSerializerSettings();
                string serializedItem = JsonConvert.SerializeObject(sourceItem, Formatting.Indented, serializerSettings);
                var clonedItem = JsonConvert.DeserializeObject<Item>(serializedItem, serializerSettings);

                // The source gets the new id and becomes unlinked (not refrenced by location or character item counts)
                // This allows the source item to call this method, then to safely mutate itself.
                sourceItem.TrackingId = Guid.NewGuid();

                AddItem(sourceItem);
                AddItem(clonedItem);
            }
        }

        public void DedupeItems()
        {
            lock (itemLock)
            {
                bool dupeFound = true;
                while (dupeFound)
                {
                    dupeFound = false;
                    foreach (var itemA in Items.Values)
                    {
                        foreach (var itemB in Items.Values)
                        {
                            // If these are actually the same item instance then ignore
                            if (itemA.TrackingId == itemB.TrackingId)
                            {
                                continue;
                            }

                            if (EqualChecker.AreEqual(itemA, itemB))
                            {
                                dupeFound = true;

                                // TODO: Not sure how we would update refs to itemB that are found in other object properties
                                // TODO: Maybe we can raise an event here that can be subscribed to
                                Items.Remove(itemB.TrackingId);
                                StackItems(itemB.TrackingId, itemA.TrackingId);
                                break;
                            }
                        }
                        if (dupeFound)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void StackItems(Guid removeItemTrackingId, Guid receiveItemTrackingId)
        {
            lock (characterLock)
            {
                lock (itemLock)
                {
                    lock (locationLock)
                    {
                        foreach (var characterTrackingId in CharactersItems.Keys)
                        {
                            if (CharactersItems[characterTrackingId] == null)
                            {
                                continue;
                            }

                            if (!CharactersItems[characterTrackingId].ContainsKey(removeItemTrackingId))
                            {
                                continue;
                            }

                            var removedCount = CharactersItems[characterTrackingId][removeItemTrackingId];
                            CharactersItems[characterTrackingId].Remove(removeItemTrackingId);
                            if (CharactersItems[characterTrackingId].ContainsKey(receiveItemTrackingId))
                            {
                                CharactersItems[characterTrackingId][receiveItemTrackingId] += removedCount;
                            }
                            else
                            {
                                CharactersItems[characterTrackingId][receiveItemTrackingId] = removedCount;
                            }
                        }

                        foreach (var locationTrackingId in LocationItems.Keys)
                        {
                            if (LocationItems[locationTrackingId] == null)
                            {
                                continue;
                            }

                            if (!LocationItems[locationTrackingId].ContainsKey(removeItemTrackingId))
                            {
                                continue;
                            }

                            var removedCount = LocationItems[locationTrackingId][removeItemTrackingId];
                            LocationItems[locationTrackingId].Remove(removeItemTrackingId);
                            if (LocationItems[locationTrackingId].ContainsKey(receiveItemTrackingId))
                            {
                                LocationItems[locationTrackingId][receiveItemTrackingId] += removedCount;
                            }
                            else
                            {
                                LocationItems[locationTrackingId][receiveItemTrackingId] = removedCount;
                            }
                        }
                    }
                }
            }
        }

        public Guid AddLocation(Location location)
        {
            lock (locationLock)
            {
                Debug.Assert(!Locations.ContainsKey(location.TrackingId), $"A location with the same tracking id '{location.TrackingId}' has already been added.");
                Locations.Add(location.TrackingId, location);
                return location.TrackingId;
            }
        }

        public Location GetLocation(Guid locationTrackingId)
        {
            lock (locationLock)
            {
                return Locations.ContainsKey(locationTrackingId) ? Locations[locationTrackingId] : null;
            }
        }

        /// <summary>
        /// Gets a list of locations that are currently available to move into from the specified location
        /// </summary>
        /// <param name="originLocationTrackingId">The origin location tracking id</param>
        /// <returns>A list of open portals</returns>
        public List<Location> GetConnectedLocations(Guid originLocationTrackingId)
        {
            lock (locationLock)
            {
                lock (portalLock)
                {
                    var validLocations = Portals.Values
                    .Where(p => p.HasOriginLocation(originLocationTrackingId)) // Portals in the specified location
                    .Select(p => p.GetDestination(originLocationTrackingId)) // Get destination info for each of the portals
                    .Where(d => d.DestinationTrackingId != Guid.Empty) // Exclude destinations that lead nowhere (locked doors, etc)
                    .Select(d => GetLocation(d.DestinationTrackingId)) // Get the actual destination location
                    .ToList();
                    return validLocations;
                }
            }
        }

        public Guid AddPortal(params PortalRule[] destinationRules)
        {
            lock (portalLock)
            {
                var portal = new Portal(destinationRules.ToList());
                return AddPortal(portal);
            }
        }

        public Guid AddPortal(Portal portal)
        {
            lock (portalLock)
            {
                Portals[portal.TrackingId] = portal;
                return portal.TrackingId;
            }
        }

        public Portal GetPortal(Guid portalTrackingId)
        {
            lock (portalLock)
            {
                return Portals.ContainsKey(portalTrackingId) ? Portals[portalTrackingId] : null;
            }
        }

        public List<Portal> GetPortalsInLocation(Guid originLocationTrackingId)
        {
            lock (portalLock)
            {
                var portals = Portals.Values
                    .Where(p => p.HasOriginLocation(originLocationTrackingId))
                    .ToList();
                return portals;
            }
        }

        public Guid AddItem(Item item)
        {
            lock (itemLock)
            {
                Items[item.TrackingId] = item;
                return item.TrackingId;
            }
        }

        public Item GetItem(Guid itemTrackingId)
        {
            lock (itemLock)
            {
                return Items.ContainsKey(itemTrackingId) ? Items[itemTrackingId] : null;
            }
        }

        public Guid AddCharacter(Character character, Guid locationTrackingId)
        {
            lock (characterLock)
            {
                lock (locationLock)
                {
                    Debug.Assert(!Characters.ContainsKey(character.TrackingId), $"A character with the same tracking id '{character.TrackingId}' has already been added.");
                    Characters.Add(character.TrackingId, character);
                    CharacterLocations[character.TrackingId] = locationTrackingId;
                    return character.TrackingId;
                }
            }
        }

        public Character GetCharacter(Guid characterTrackingId)
        {
            lock (characterLock)
            {
                if (Characters.ContainsKey(characterTrackingId))
                {
                    return Characters[characterTrackingId];
                }
                return null;
            }
        }

        public Dictionary<Item, int> GetCharacterItems(Guid characterTrackingId)
        {
            lock (characterLock)
            {
                lock (itemLock)
                {
                    if (CharactersItems.ContainsKey(characterTrackingId))
                    {
                        var characterItems = CharactersItems[characterTrackingId]
                            .ToDictionary(kvp => GetItem(kvp.Key), kvp => kvp.Value);
                        return characterItems;
                    }
                    else
                    {
                        return new Dictionary<Item, int>();
                    }
                }
            }
        }

        public Dictionary<Item, int> GetLocationItems(Guid locationTrackingId)
        {
            lock (itemLock)
            {
                lock (locationLock)
                {
                    if (LocationItems.ContainsKey(locationTrackingId))
                    {
                        var locationItems = LocationItems[locationTrackingId]
                            .ToDictionary(kvp => GetItem(kvp.Key), kvp => kvp.Value);
                        return locationItems;
                    }
                    else
                    {
                        return new Dictionary<Item, int>();
                    }
                }
            }
        }

        public List<Character> GetAllCharactersPresentInWorld()
        {
            lock (characterLock)
            {
                var presentCharacters = Characters.Values
                    .Where(c => c.IsPresentInWorld())
                    .ToList();
                return presentCharacters;
            }
        }

        public List<Character> GetCharactersInLocation(Guid locationTrackingId)
        {
            lock (characterLock)
            {
                lock (locationLock)
                {
                    var characters = CharacterLocations
                        .Where(kvp => kvp.Value == locationTrackingId) // Where location is the one passed in
                        .Select(kvp => GetCharacter(kvp.Key))
                        .Where(c => c.IsPresentInWorld());

                    return characters.ToList();
                }
            }
        }

        /// <summary>
        /// Gets the location of the specified character
        /// </summary>
        /// <param name="characterTrackingId">The character name</param>
        /// <returns>Character location or null</returns>
        public Location GetCharacterLocation(Guid characterTrackingId) // TODO: any way to make it so this is only used by Character.GetLocation?
        {
            lock (characterLock)
            {
                lock (locationLock)
                {
                    if (CharacterLocations.ContainsKey(characterTrackingId))
                    {
                        var locationTrackingId = CharacterLocations[characterTrackingId];
                        return Locations[locationTrackingId];
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// Sets the location of the specified character
        /// </summary>
        /// <param name="characterTrackingId">The Name of the character</param>
        /// <param name="locationTrackingId">The location to place the character at</param>
        public void SetCharacterLocation(Guid characterTrackingId, Guid locationTrackingId)
        {
            lock (characterLock)
            {
                lock (locationLock)
                {
                    CharacterLocations[characterTrackingId] = locationTrackingId;
                }
            }
        }

        /// <summary>
        /// Gets a game variable by its full name
        /// </summary>
        /// <param name="gameVariableName">The name of the game variable to get</param>
        /// <returns>The value or null if it's not set.</returns>
        public string GetGameVarValue(string gameVariableName)
        {
            lock (gameVarLock)
            {
                if (GameVars.ContainsKey(gameVariableName))
                {
                    return GameVars[gameVariableName];
                }
                return null;
            }
        }

        /// <summary>
        /// Sets the game variable to the specified value
        /// </summary>
        /// <param name="gameVariableName">The game variable to set</param>
        /// <param name="value">The value to set the game variable to</param>
        public string SetGameVarValue(string gameVariableName, string value)
        {
            lock (gameVarLock)
            {
                GameVars[gameVariableName] = value;
                return gameVariableName;
            }
        }

        public Guid AddTradeSet(string tradesetName, params ItemRecipe[] itemRecipes)
        {
            lock (tradeLock)
            {
                var tradeSet = new TradeSet(tradesetName, itemRecipes);
                return AddTradeSet(tradeSet);
            }
        }

        public Guid AddTradeSet(TradeSet tradeSet)
        {
            lock (tradeLock)
            {
                TradeSets[tradeSet.TrackingId] = tradeSet;
                return tradeSet.TrackingId;
            }
        }

        public TradeSet GetTradeSet(Guid tradeSetTrackingId)
        {
            lock (tradeLock)
            {
                if (TradeSets.ContainsKey(tradeSetTrackingId))
                {
                    return TradeSets[tradeSetTrackingId];
                }
                return null;
            }
        }

        public Guid AddTradePost(Guid locationTrackingId, string tradePostName, params Guid[] tradeSetTrackingIds)
        {
            lock (locationLock)
            {
                lock (tradeLock)
                {
                    var tradePost = new TradePost(tradePostName, tradeSetTrackingIds);
                    return AddTradePost(tradePost, locationTrackingId);
                }
            }
        }

        public Guid AddTradePost(TradePost tradePost, Guid locationTrackingId)
        {
            lock (locationLock)
            {
                lock (tradeLock)
                {
                    TradePosts[tradePost.TrackingId] = tradePost;
                    CurrentTradePostLocations[tradePost.TrackingId] = locationTrackingId;
                    return tradePost.TrackingId;
                }
            }
        }

        public TradePost GetTradePost(Guid tradePostTrackingId)
        {
            lock (tradeLock)
            {
                if (TradePosts.ContainsKey(tradePostTrackingId))
                {
                    return TradePosts[tradePostTrackingId];
                }
                return null;
            }
        }

        /// <summary>
        /// Adds/sets a tradepost to be at the specified location
        /// </summary>
        /// <param name="tradePostTrackingId">The name of the trade post</param>
        /// <param name="locationTrackingId">The location the trade post is at</param>
        public void SetTradePostLocation(Guid tradePostTrackingId, Guid locationTrackingId)
        {
            lock (locationLock)
            {
                lock (tradeLock)
                {
                    CurrentTradePostLocations[tradePostTrackingId] = locationTrackingId;
                }
            }
        }

        /// <summary>
        /// Gets all the trade posts at the given location
        /// </summary>
        /// <param name="locationTrackingId">The location to look at</param>
        /// <returns>All the trade posts at the given location</returns>
        public List<TradePost> GetTradePostsAtLocation(Guid locationTrackingId)
        {
            lock (locationLock)
            {
                lock (tradeLock)
                {
                    var tradePostNames = CurrentTradePostLocations
                        .Where(kvp => kvp.Value.Equals(locationTrackingId))
                        .Select(kvp => GetTradePost(kvp.Key))
                        .ToList();
                    return tradePostNames;
                }
            }
        }

        public void AddTurnBehavior(string behaviorName, ITurnBehavior behavior)
        {
            TurnBehaviors[behaviorName] = behavior;
        }

        public ITurnBehavior GetTurnBehavior(string behaviorName)
        {
            if (behaviorName == null)
            {
                return null;
            }
            if (TurnBehaviors.ContainsKey(behaviorName))
            {
                return TurnBehaviors[behaviorName];
            }
            return null;
        }

        public void AddGameCommand(IGameCommand command)
        {
            GameCommands.Add(command);
        }

        public IGameCommand GetGameCommand(string commandName, List<string> accountPermissions)
        {
            if (string.IsNullOrWhiteSpace(commandName))
            {
                return null;
            }

            var command = GameCommands
                .Where(c => c.ActivatingWords.Any(w => w.Equals(commandName, StringComparison.OrdinalIgnoreCase)))
                .FirstOrDefault(c => c.PermissionNeeded == null || accountPermissions.Contains(c.PermissionNeeded, StringComparer.OrdinalIgnoreCase));

            return command;
        }

        public Account CreateAccount(string userName)
        {
            lock (accountLock)
            {
                var alreadyExistsAccount = GetAccount(userName);
                if (alreadyExistsAccount != null)
                {
                    // This account already exists.. just return it.
                    return alreadyExistsAccount;
                }

                var account = new Account()
                {
                    UserName = userName,
                    Permissions = new List<string>()
                };
                Accounts.Add(userName, account);
                return account;
            }
        }

        public Account GetAccount(string userName)
        {
            lock (accountLock)
            {
                if (Accounts.ContainsKey(userName))
                {
                    return Accounts[userName];
                }
                return null;
            }
        }
    }
}