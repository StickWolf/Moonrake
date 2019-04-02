using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

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

        private int GetCharacterItemCount(string characterName, string itemName)
        {
            if (CharactersItems.ContainsKey(characterName))
            {
                if (CharactersItems[characterName].ContainsKey(itemName))
                {
                    return CharactersItems[characterName][itemName];
                }
            }
            return 0;
        }

        public void RemoveItemEveryWhere(string itemName)
        {
            foreach (var characterName in CharactersItems.Keys)
            {
                if (CharactersItems[characterName].ContainsKey(itemName))
                {
                    CharactersItems[characterName].Remove(itemName);
                }
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

            if (!CharactersItems.ContainsKey(characterName))
            {
                CharactersItems.Add(characterName, new Dictionary<string, int>());
            }

            if (!CharactersItems[characterName].ContainsKey(itemName))
            {
                CharactersItems[characterName].Add(itemName, 0);
            }

            var characterItemCount = GetCharacterItemCount(characterName, itemName);

            if (!item.IsUnique)
            {
                characterItemCount += count;
                if (characterItemCount > 0)
                {
                    CharactersItems[characterName][itemName] = characterItemCount;
                    return true;
                }
                else if (characterItemCount == 0)
                {
                    CharactersItems[characterName].Remove(itemName);
                    return true;
                }
                return false;
            }

            if (count < 0)
            {
                if (characterItemCount > 0)
                {
                    CharactersItems[characterName].Remove(itemName);
                    return true;
                }
                return false;
            }

            RemoveItemEveryWhere(itemName);
            CharactersItems[characterName][itemName] = 1;
            return true;
        }

        public bool TryAddRoomItemCount(string roomName, string itemName, int count, GameSourceDataBase gameData)
        {
            if (gameData.TryGetItem(itemName, out Item item) == false)
            {
                return false;
            }

            if (count == 0)
            {
                return true;
            }

            if (!LocationItems.ContainsKey(roomName))
            {
                LocationItems.Add(roomName, new Dictionary<string, int>());
            }

            if (!LocationItems[roomName].ContainsKey(itemName))
            {
                LocationItems[roomName].Add(itemName, 0);
            }

            var roomItemCount = GetLocationItemCount(roomName, itemName);

            if (!item.IsUnique)
            {
                roomItemCount += count;
                if (roomItemCount > 0)
                {
                    LocationItems[roomName][itemName] = roomItemCount;
                    return true;
                }
                else if (roomItemCount == 0)
                {
                    LocationItems[roomName].Remove(itemName);
                    return true;
                }
                return false;
            }

            if (count < 0)
            {
                if (roomItemCount > 0)
                {
                    LocationItems[roomName].Remove(itemName);
                    return true;
                }
                return false;
            }

            RemoveItemEveryWhere(itemName);
            LocationItems[roomName][itemName] = 1;
            return true;
        }

        public int GetLocationItemCount(string locationName, string itemName)
        {
            if (LocationItems.ContainsKey(locationName))
            {
                if (LocationItems[locationName].ContainsKey(itemName))
                {
                    return LocationItems[locationName][itemName];
                }
            }
            return 0;
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
