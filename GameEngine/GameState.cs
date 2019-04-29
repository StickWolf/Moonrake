using GameEngine.Characters;
using GameEngine.Locations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GameEngine
{
    /// <summary>
    /// The GameState class is meant to represent the current state of the game.
    /// This class holds any data that should be saved and reloaded for saved games.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class GameState
    {
        // This data goes into save files
        [JsonProperty]
        private Guid PlayerTrackingId { get; set; }

        // Characters[{CharacterTrackingId}] = {Character}
        [JsonProperty]
        private Dictionary<Guid, Character> Characters { get; set; } = new Dictionary<Guid, Character>();

        // Locations[{LocationTrackingId}] = {Location}
        [JsonProperty]
        private Dictionary<Guid, Location> Locations { get; set; } = new Dictionary<Guid, Location>();

        // CharacterItems[{CharacterTrackingId}][{ItemTrackingName}] = {ItemCount}
        [JsonProperty]
        private Dictionary<Guid, Dictionary<string, int>> CharactersItems { get; set; } = new Dictionary<Guid, Dictionary<string, int>>();

        // LocationItems[{LocationTrackingId}][{ItemTrackingName}] = {ItemCount}
        [JsonProperty]
        private Dictionary<Guid, Dictionary<string, int>> LocationItems { get; set; } = new Dictionary<Guid, Dictionary<string, int>>();

        // CharacterLocations[{CharacterTrackingId}] = {LocationTrackingId}
        [JsonProperty]
        private Dictionary<Guid, Guid> CharacterLocations { get; set; } = new Dictionary<Guid, Guid>();

        // GameVars[{GameVarName}] = {GameVarValue}
        [JsonProperty]
        private Dictionary<string, string> GameVars { get; set; } = new Dictionary<string, string>();

        // CurrentTradePostLocations[{TradePostName}] = {LocationTrackingId}
        [JsonProperty]
        private Dictionary<string, Guid> CurrentTradePostLocations { get; set; } = new Dictionary<string, Guid>();

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

        public static void SaveGameState(string slotName)
        {
            var savedGamesDictionary = GetGameStates();

            // add (or update) the gamestate that was passed in into the dictionary we now have using slotName as the key
            savedGamesDictionary[slotName] = CurrentGameState;

            var serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };

            // serialize the dictionary to a string
            string serializedDictionary = JsonConvert.SerializeObject(savedGamesDictionary, Formatting.Indented, serializerSettings);

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

                var serializerSettings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                // deserialize the string into a dictionary<string, gamestate>
                savedGamesDictionary = JsonConvert.DeserializeObject<Dictionary<string, GameState>>(fileContents, serializerSettings);
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

        public void RemoveItemEveryWhere(string itemTrackingName)
        {
            // Remove the item from all characters
            foreach (var characterTrackingId in CharactersItems.Keys)
            {
                if (CharactersItems[characterTrackingId].ContainsKey(itemTrackingName))
                {
                    CharactersItems[characterTrackingId].Remove(itemTrackingName);
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
                if(LocationItems[locationName].ContainsKey(itemTrackingName))
                {
                    LocationItems[locationName].Remove(itemTrackingName);
                }
            }
            var zeroLocationKeys = LocationItems.Where(kvp => !kvp.Value.Any()).Select(kvp => kvp.Key).ToList();
            foreach (var key in zeroLocationKeys)
            {
                LocationItems.Remove(key);
            }
        }

        public bool TryAddCharacterItemCount(Guid characterTrackingId, string itemTrackingName, int count, GameSourceData gameData)
        {
            // If the item we are trying to get does not even exist, we will stop the process of this method
            if (gameData.TryGetItem(itemTrackingName, out Item item) == false)
            {
                return false;
            }
            return TryAddCharacterItemCount(characterTrackingId, itemTrackingName, count, item);
        }

        public bool TryAddCharacterItemCount(Guid characterTrackingId, string itemTrackingName, int count, Item item)
        {
            if (count == 0)
            {
                return true;
            }

            void setCharacterItemCount(Guid sCharacterTrackingId, string sItemTrackingName, int sCount)
            {
                if (!CharactersItems.ContainsKey(sCharacterTrackingId))
                {
                    CharactersItems.Add(sCharacterTrackingId, new Dictionary<string, int>());
                }
                if (!CharactersItems[sCharacterTrackingId].ContainsKey(sItemTrackingName))
                {
                    CharactersItems[sCharacterTrackingId].Add(sItemTrackingName, sCount);
                }
                else
                {
                    CharactersItems[sCharacterTrackingId][sItemTrackingName] = sCount;
                }
            }

            void removeCharacterItem(Guid sCharacterTrackingId, string sItemTrackingName)
            {
                if (!CharactersItems.ContainsKey(sCharacterTrackingId))
                {
                    return;
                }
                if (CharactersItems[sCharacterTrackingId].ContainsKey(sItemTrackingName))
                {
                    CharactersItems[sCharacterTrackingId].Remove(sItemTrackingName);
                    if (!CharactersItems[sCharacterTrackingId].Any())
                    {
                        CharactersItems.Remove(sCharacterTrackingId);
                    }
                }
            }

            // The goal is to only keep records in the dictionary for counts greater than 0
            // Even if we do temporarily add keys here, they should be cleaned up before returning if needed
            var characterItemCount = 0;
            if (CharactersItems.ContainsKey(characterTrackingId) && CharactersItems[characterTrackingId].ContainsKey(itemTrackingName))
            {
                characterItemCount = CharactersItems[characterTrackingId][itemTrackingName];
            }

            // Non-unique items can be added and removed from characters without worrying about the count
            if (!item.IsUnique)
            {
                characterItemCount += count;
                if (characterItemCount > 0)
                {
                    setCharacterItemCount(characterTrackingId, itemTrackingName, characterItemCount);
                    return true;
                }
                else if (characterItemCount == 0)
                {
                    removeCharacterItem(characterTrackingId, itemTrackingName);
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
                    removeCharacterItem(characterTrackingId, itemTrackingName);
                    return true;
                }
                return false;
            }

            // The only other option is that this is a unique item being added to the character
            // First remove it from everywhere else, then add it here.
            RemoveItemEveryWhere(itemTrackingName);
            setCharacterItemCount(characterTrackingId, itemTrackingName, 1);
            return true;
        }

        public bool TryAddLocationItemCount(Guid locationTrackingId, string itemTrackingName, int count, GameSourceData gameData)
        {
            if (gameData.TryGetItem(itemTrackingName, out Item item) == false)
            {
                return false;
            }
            return TryAddLocationItemCount(locationTrackingId, itemTrackingName, count, item);
        }

        public bool TryAddLocationItemCount(Guid locationTrackingId, string itemTrackingName, int count, Item item)
        {
            if (count == 0)
            {
                return true;
            }

            void setLocationItemCount(Guid sLocationTrackingId, string sItemTrackingName, int sCount)
            {
                if (!LocationItems.ContainsKey(sLocationTrackingId))
                {
                    LocationItems.Add(sLocationTrackingId, new Dictionary<string, int>());
                }
                if (!LocationItems[sLocationTrackingId].ContainsKey(sItemTrackingName))
                {
                    LocationItems[sLocationTrackingId].Add(sItemTrackingName, sCount);
                }
                else
                {
                    LocationItems[sLocationTrackingId][sItemTrackingName] = sCount;
                }
            }

            void removeLocationItem(Guid sLocationTrackingId, string sItemTrackingName)
            {
                if (!LocationItems.ContainsKey(sLocationTrackingId))
                {
                    return;
                }
                if (LocationItems[sLocationTrackingId].ContainsKey(sItemTrackingName))
                {
                    LocationItems[sLocationTrackingId].Remove(sItemTrackingName);
                    if (!LocationItems[sLocationTrackingId].Any())
                    {
                        LocationItems.Remove(sLocationTrackingId);
                    }
                }
            }

            // The goal is to only keep records in the dictionary for counts greater than 0
            // Even if we do temporarily add keys here, they should be cleaned up before returning if needed
            var locationItemCount = 0;
            if (LocationItems.ContainsKey(locationTrackingId) && LocationItems[locationTrackingId].ContainsKey(itemTrackingName))
            {
                locationItemCount = LocationItems[locationTrackingId][itemTrackingName];
            }

            // Non-unique items can be added and removed from locations without worrying about the count
            if (!item.IsUnique)
            {
                locationItemCount += count;
                if (locationItemCount > 0)
                {
                    setLocationItemCount(locationTrackingId, itemTrackingName, locationItemCount);
                    return true;
                }
                else if (locationItemCount == 0)
                {
                    removeLocationItem(locationTrackingId, itemTrackingName);
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
                    removeLocationItem(locationTrackingId, itemTrackingName);
                    return true;
                }
                return false;
            }

            // The only other option is that this is a unique item being added to a location
            // First remove it from everywhere else, then add it here.
            RemoveItemEveryWhere(itemTrackingName);
            setLocationItemCount(locationTrackingId, itemTrackingName, 1);
            return true;
        }

        public Guid AddLocation(Location location)
        {
            Debug.Assert(!Locations.ContainsKey(location.TrackingId), $"A location with the same tracking id '{location.TrackingId}' has already been added.");
            Locations.Add(location.TrackingId, location);
            return location.TrackingId;
        }

        public Location GetLocation(Guid locationTrackingId)
        {
            return Locations.ContainsKey(locationTrackingId) ? Locations[locationTrackingId] : null;
        }

        public Guid AddCharacter(Character character, Guid locationTrackingId)
        {
            Debug.Assert(!Characters.ContainsKey(character.TrackingId), $"A character with the same tracking id '{character.TrackingId}' has already been added.");

            // If this is the player character, then keep track of the tracking id
            if (character is PlayerCharacter)
            {
                Debug.Assert(PlayerTrackingId == Guid.Empty, $"A player character has already been set in the gamestate.");
                PlayerTrackingId = character.TrackingId;
            }
            Characters.Add(character.TrackingId, character);
            CharacterLocations[character.TrackingId] = locationTrackingId;
            return character.TrackingId;
        }

        public Character GetPlayerCharacter()
        {
            return GetCharacter(PlayerTrackingId);
        }

        public Character GetCharacter(Guid characterTrackingId)
        {
            if (Characters.ContainsKey(characterTrackingId))
            {
                return Characters[characterTrackingId];
            }
            return null;
        }

        public List<Character> GetCharactersWithName(string characterName)
        {
            var characters = Characters.Values.Where(c => c.Name.Equals(characterName, StringComparison.OrdinalIgnoreCase)).ToList();
            return characters;
        }

        public Dictionary<string, int> GetCharacterItems(Guid characterTrackingId)
        {
            if (CharactersItems.ContainsKey(characterTrackingId))
            {
                return CharactersItems[characterTrackingId];
            }
            else
            {
                return null;
            }
        }

        public Dictionary<string, int> GetLocationItems(Guid locationTrackingId)
        {
            if (LocationItems.ContainsKey(locationTrackingId))
            {
                return LocationItems[locationTrackingId];
            }
            else
            {
                return null;
            }
        }

        public List<Character> GetAllCharacters()
        {
            var characters = Characters.Values.ToList();
            return characters;
        }

        public List<Character> GetCharactersInLocation(Guid locationTrackingId, bool includePlayer)
        {
            var characters = CharacterLocations
                .Where(kvp => kvp.Value == locationTrackingId) // Where location is the one passed in
                .Select(kvp => GetCharacter(kvp.Key));

            // remove player if needed.
            if (!includePlayer)
            {
                characters = characters
                    .Where(c => c.TrackingId != PlayerTrackingId);
            }

            return characters.ToList();
        }

        public Location GetPlayerCharacterLocation()
        {
            return GetCharacterLocation(PlayerTrackingId);
        }

        /// <summary>
        /// Gets the location of the specified character
        /// </summary>
        /// <param name="characterTrackingId">The character name</param>
        /// <returns>Character location or null</returns>
        public Location GetCharacterLocation(Guid characterTrackingId) 
        {
            if (CharacterLocations.ContainsKey(characterTrackingId))
            {
                var locationTrackingId = CharacterLocations[characterTrackingId];
                return Locations[locationTrackingId];
            }
            return null;
        }

        /// <summary>
        /// Sets the location of the specified character
        /// </summary>
        /// <param name="characterTrackingId">The Name of the character</param>
        /// <param name="locationTrackingId">The location to place the character at</param>
        public void SetCharacterLocation(Guid characterTrackingId, Guid locationTrackingId)
        {
            CharacterLocations[characterTrackingId] = locationTrackingId;
        }

        /// <summary>
        /// Gets a game variable by its full name
        /// </summary>
        /// <param name="gameVariableName">The name of the game variable to get</param>
        /// <returns>The value or null if it's not set.</returns>
        public string GetGameVarValue(string gameVariableName)
        {
            if (GameVars.ContainsKey(gameVariableName))
            {
                return GameVars[gameVariableName];
            }
            return null;
        }

        /// <summary>
        /// Sets the game variable to the specified value
        /// </summary>
        /// <param name="gameVariableName">The game variable to set</param>
        /// <param name="value">The value to set the game variable to</param>
        public void SetGameVarValue(string gameVariableName, string value)
        {
            GameVars[gameVariableName] = value;
        }

        /// <summary>
        /// Adds/sets a tradepost to be at the specified location
        /// </summary>
        /// <param name="tradePostName">The name of the trade post</param>
        /// <param name="locationTrackingId">The location the trade post is at</param>
        public void SetTradePostLocation(string tradePostName, Guid locationTrackingId)
        {
            CurrentTradePostLocations[tradePostName] = locationTrackingId;
        }

        /// <summary>
        /// Gets all the trade posts at the given location
        /// </summary>
        /// <param name="locationTrackingId">The location to look at</param>
        /// <returns>All the trade posts at the given location</returns>
        public List<string> GetTradePostsAtLocation(Guid locationTrackingId)
        {
            var tradePostNames = CurrentTradePostLocations
                .Where(kvp => kvp.Value.Equals(locationTrackingId))
                .Select(kvp => kvp.Key)
                .ToList();
            return tradePostNames;
        }
    }
}
