using GameEngine.Characters;
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

        // CharacterItems[{CharacterTrackingId}][{ItemTrackingName}] = {ItemCount}
        [JsonProperty]
        private Dictionary<Guid, Dictionary<string, int>> CharactersItems { get; set; } = new Dictionary<Guid, Dictionary<string, int>>();

        // LocationItems[{LocationName}][{ItemTrackingName}] = {ItemCount}
        [JsonProperty]
        private Dictionary<string, Dictionary<string, int>> LocationItems { get; set; } = new Dictionary<string, Dictionary<string, int>>();

        // CharacterLocations[{CharacterTrackingId}] = {LocationName}
        [JsonProperty]
        private Dictionary<Guid, string> CharacterLocations { get; set; } = new Dictionary<Guid, string>();

        // GameVars[{GameVarName}] = {GameVarValue}
        [JsonProperty]
        private Dictionary<string, string> GameVars { get; set; } = new Dictionary<string, string>();

        // CurrentTradePostLocations[{TradePostName}] = {LocationName}
        [JsonProperty]
        private Dictionary<string, string> CurrentTradePostLocations { get; set; } = new Dictionary<string, string>();

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

            // If there is an existing saves file
            if (File.Exists(SaveFileName))
            {
                // read the save file to a string
                string fileContents = File.ReadAllText(SaveFileName);

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

        public bool TryAddLocationItemCount(string locationName, string itemTrackingName, int count, GameSourceData gameData)
        {
            if (gameData.TryGetItem(itemTrackingName, out Item item) == false)
            {
                return false;
            }
            return TryAddLocationItemCount(locationName, itemTrackingName, count, item);
        }

        public bool TryAddLocationItemCount(string locationName, string itemTrackingName, int count, Item item)
        {
            if (count == 0)
            {
                return true;
            }

            void setLocationItemCount(string sLocationName, string sItemTrackingName, int sCount)
            {
                if (!LocationItems.ContainsKey(sLocationName))
                {
                    LocationItems.Add(sLocationName, new Dictionary<string, int>());
                }
                if (!LocationItems[sLocationName].ContainsKey(sItemTrackingName))
                {
                    LocationItems[sLocationName].Add(sItemTrackingName, sCount);
                }
                else
                {
                    LocationItems[sLocationName][sItemTrackingName] = sCount;
                }
            }

            void removeLocationItem(string sLocationName, string sItemTrackingName)
            {
                if (!LocationItems.ContainsKey(sLocationName))
                {
                    return;
                }
                if (LocationItems[sLocationName].ContainsKey(sItemTrackingName))
                {
                    LocationItems[sLocationName].Remove(sItemTrackingName);
                    if (!LocationItems[sLocationName].Any())
                    {
                        LocationItems.Remove(sLocationName);
                    }
                }
            }

            // The goal is to only keep records in the dictionary for counts greater than 0
            // Even if we do temporarily add keys here, they should be cleaned up before returning if needed
            var locationItemCount = 0;
            if (LocationItems.ContainsKey(locationName) && LocationItems[locationName].ContainsKey(itemTrackingName))
            {
                locationItemCount = LocationItems[locationName][itemTrackingName];
            }

            // Non-unique items can be added and removed from locations without worrying about the count
            if (!item.IsUnique)
            {
                locationItemCount += count;
                if (locationItemCount > 0)
                {
                    setLocationItemCount(locationName, itemTrackingName, locationItemCount);
                    return true;
                }
                else if (locationItemCount == 0)
                {
                    removeLocationItem(locationName, itemTrackingName);
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
                    removeLocationItem(locationName, itemTrackingName);
                    return true;
                }
                return false;
            }

            // The only other option is that this is a unique item being added to a location
            // First remove it from everywhere else, then add it here.
            RemoveItemEveryWhere(itemTrackingName);
            setLocationItemCount(locationName, itemTrackingName, 1);
            return true;
        }

        public void AddCharacter(Character character)
        {
            Debug.Assert(!Characters.ContainsKey(character.TrackingId), $"A character with the same tracking id '{character.TrackingId}' has already been added.");

            // If this is the player character, then keep track of the tracking id
            if (character is PlayerCharacter)
            {
                Debug.Assert(PlayerTrackingId == Guid.Empty, $"A player character has already been set in the gamestate.");
                PlayerTrackingId = character.TrackingId;
            }
            Characters.Add(character.TrackingId, character);
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

        public Dictionary<string, int> GetLocationItems(string locationName)
        {
            if (LocationItems.ContainsKey(locationName))
            {
                return LocationItems[locationName];
            }
            else
            {
                return null;
            }
        }

        public List<Character> GetCharactersInAllLocations()
        {
            var characters = CharacterLocations
                .Select(kvp => GetCharacter(kvp.Key))
                .ToList();
            return characters;
        }

        public List<Character> GetCharactersInLocation(string locationName, bool includePlayer) // TODO: update to instead accept a location tracking guid
        {
            var characters = CharacterLocations
                .Where(kvp => kvp.Value.Equals(locationName, StringComparison.OrdinalIgnoreCase)) // Where location is the one passed in
                .Select(kvp => GetCharacter(kvp.Key));
            
            // remove player if needed.
            if (!includePlayer)
            {
                characters = characters
                    .Where(c => c.TrackingId != PlayerTrackingId);
            }

            return characters.ToList();
        }

        public string GetPlayerCharacterLocation() // TODO: update to instead return an actual Location
        {
            return GetCharacterLocation(PlayerTrackingId);
        }

        /// <summary>
        /// Gets the location of the specified character
        /// </summary>
        /// <param name="characterTrackingId">The character name</param>
        /// <returns>Character location or null</returns>
        public string GetCharacterLocation(Guid characterTrackingId) // TODO: update to instead return an actual Location
        {
            if (CharacterLocations.ContainsKey(characterTrackingId))
            {
                return CharacterLocations[characterTrackingId];
            }
            return null;
        }

        /// <summary>
        /// Sets the location of the specified character
        /// </summary>
        /// <param name="characterTrackingId">The Name of the character</param>
        /// <param name="locationName">The location to place the character at</param>
        public void SetCharacterLocation(Guid characterTrackingId, string locationName) // TODO: update to accept a location tracking guid instead
        {
            CharacterLocations[characterTrackingId] = locationName;
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
        /// <param name="locationName">The location the trade post is at</param>
        public void SetTradePostLocation(string tradePostName, string locationName)
        {
            CurrentTradePostLocations[tradePostName] = locationName;
        }

        /// <summary>
        /// Gets all the trade posts at the given location
        /// </summary>
        /// <param name="locationName">The location to look at</param>
        /// <returns>All the trade posts at the given location</returns>
        public List<string> GetTradePostsAtLocation(string locationName)
        {
            var tradePostNames = CurrentTradePostLocations
                .Where(kvp => kvp.Value.Equals(locationName))
                .Select(kvp => kvp.Key)
                .ToList();
            return tradePostNames;
        }
    }
}
