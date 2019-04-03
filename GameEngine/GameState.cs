using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public string PlayerName { get; set; }

        // This dictionary is for storing WHO has an item, WHAT the item is,
        // and how MUCH the player has.
        [JsonProperty]
        private Dictionary<string, Dictionary<string, int>> CharactersItems { get; set; } = new Dictionary<string, Dictionary<string, int>>();

        //This is for keeping track of how much items a room has.
        [JsonProperty]
        public Dictionary<string, Dictionary<string, int>> LocationItems { get; set; } = new Dictionary<string, Dictionary<string, int>>();

        // CharacterLocations[{CharacterName}] = {LocationName}
        [JsonProperty]
        public Dictionary<string, string> CharacterLocations { get; set; } = new Dictionary<string, string>();

        // GameVars[{GameVarName}] = {GameVarValue}
        [JsonProperty]
        public Dictionary<string, string> GameVars { get; set; } = new Dictionary<string, string>();

        // CurrentTradePostLocations[{TradePostName}] = {LocationName}
        [JsonProperty]
        public Dictionary<string, string> CurrentTradePostLocations { get; set; } = new Dictionary<string, string>();

        // This data does NOT go into save files
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

            // serialize the dictionary to a string
            string serializedDictionary = JsonConvert.SerializeObject(savedGamesDictionary);

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

                // deserialize the string into a dictionary<string, gamestate>
                savedGamesDictionary = JsonConvert.DeserializeObject<Dictionary<string, GameState>>(fileContents);
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

        public void RemoveItemEveryWhere(string itemName)
        {
            // Remove the item from all characters
            foreach (var characterName in CharactersItems.Keys)
            {
                if (CharactersItems[characterName].ContainsKey(itemName))
                {
                    CharactersItems[characterName].Remove(itemName);
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
                if(LocationItems[locationName].ContainsKey(itemName))
                {
                    LocationItems[locationName].Remove(itemName);
                }
            }
            var zeroLocationKeys = LocationItems.Where(kvp => !kvp.Value.Any()).Select(kvp => kvp.Key).ToList();
            foreach (var key in zeroLocationKeys)
            {
                LocationItems.Remove(key);
            }
        }

        public bool TryAddCharacterItemCount(string characterName, string itemName, int count, GameSourceDataBase gameData)
        {
            // If the item we are trying to get does not even exist, we will stop the process of this method
            if (gameData.TryGetItem(itemName, out Item item) == false)
            {
                return false;
            }

            if (count == 0)
            {
                return true;
            }

            void setCharacterItemCount(string sCharacterName, string sItemName, int sCount)
            {
                if (!CharactersItems.ContainsKey(sCharacterName))
                {
                    CharactersItems.Add(sCharacterName, new Dictionary<string, int>());
                }
                if (!CharactersItems[sCharacterName].ContainsKey(sItemName))
                {
                    CharactersItems[sCharacterName].Add(sItemName, sCount);
                }
            }

            void removeCharacterItem(string sCharacterName, string sItemName)
            {
                if (!CharactersItems.ContainsKey(sCharacterName))
                {
                    return;
                }
                if (CharactersItems[sCharacterName].ContainsKey(sItemName))
                {
                    CharactersItems[sCharacterName].Remove(sItemName);
                    if (!CharactersItems[sCharacterName].Any())
                    {
                        CharactersItems.Remove(sCharacterName);
                    }
                }
            }

            // The goal is to only keep records in the dictionary for counts greater than 0
            // Even if we do temporarily add keys here, they should be cleaned up before returning if needed
            var characterItemCount = 0;
            if (CharactersItems.ContainsKey(characterName) && CharactersItems[characterName].ContainsKey(itemName))
            {
                characterItemCount = CharactersItems[characterName][itemName];
            }

            // Non-unique items can be added and removed from characters without worrying about the count
            if (!item.IsUnique)
            {
                characterItemCount += count;
                if (characterItemCount > 0)
                {
                    setCharacterItemCount(characterName, itemName, characterItemCount);
                    return true;
                }
                else if (characterItemCount == 0)
                {
                    removeCharacterItem(characterName, itemName);
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
                    removeCharacterItem(characterName, itemName);
                    return true;
                }
                return false;
            }

            // The only other option is that this is a unique item being added to the character
            // First remove it from everywhere else, then add it here.
            RemoveItemEveryWhere(itemName);
            setCharacterItemCount(characterName, itemName, 1);
            return true;
        }

        public bool TryAddLocationItemCount(string locationName, string itemName, int count, GameSourceDataBase gameData)
        {
            if (gameData.TryGetItem(itemName, out Item item) == false)
            {
                return false;
            }

            if (count == 0)
            {
                return true;
            }

            void setLocationItemCount(string sLocationName, string sItemName, int sCount)
            {
                if (!LocationItems.ContainsKey(sLocationName))
                {
                    LocationItems.Add(sLocationName, new Dictionary<string, int>());
                }
                if (!LocationItems[sLocationName].ContainsKey(sItemName))
                {
                    LocationItems[sLocationName].Add(sItemName, sCount);
                }
            }

            void removeLocationItem(string sLocationName, string sItemName)
            {
                if (!LocationItems.ContainsKey(sLocationName))
                {
                    return;
                }
                if (LocationItems[sLocationName].ContainsKey(sItemName))
                {
                    LocationItems[sLocationName].Remove(sItemName);
                    if (!LocationItems[sLocationName].Any())
                    {
                        LocationItems.Remove(sLocationName);
                    }
                }
            }

            // The goal is to only keep records in the dictionary for counts greater than 0
            // Even if we do temporarily add keys here, they should be cleaned up before returning if needed
            var locationItemCount = 0;
            if (LocationItems.ContainsKey(locationName) && LocationItems[locationName].ContainsKey(itemName))
            {
                locationItemCount = LocationItems[locationName][itemName];
            }

            // Non-unique items can be added and removed from locations without worrying about the count
            if (!item.IsUnique)
            {
                locationItemCount += count;
                if (locationItemCount > 0)
                {
                    setLocationItemCount(locationName, itemName, locationItemCount);
                    return true;
                }
                else if (locationItemCount == 0)
                {
                    removeLocationItem(locationName, itemName);
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
                    removeLocationItem(locationName, itemName);
                    return true;
                }
                return false;
            }

            // The only other option is that this is a unique item being added to a location
            // First remove it from everywhere else, then add it here.
            RemoveItemEveryWhere(itemName);
            setLocationItemCount(locationName, itemName, 1);
            return true;
        }

        public Dictionary<string, int> GetCharacterItems(string characterName)
        {
            if (CharactersItems.ContainsKey(characterName))
            {
                return CharactersItems[characterName];
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
    }
}
