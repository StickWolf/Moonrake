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

        // CharacterLocations[{CharacterName}] = {LocationName}
        [JsonProperty]
        public Dictionary<string, string> CharacterLocations { get; set; } = new Dictionary<string, string>();

        // GameVars[{GameVarName}] = {GameVarValue}
        [JsonProperty]
        public Dictionary<string, string> GameVars { get; set; } = new Dictionary<string, string>();

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

        public int GetCharacterItemCount(string playerName, string itemName)
        {
            if (CharactersItems.ContainsKey(playerName))
            {
                if (CharactersItems[playerName].ContainsKey(itemName))
                {
                   return CharactersItems[playerName][itemName];
                }
            }
            return 0;
        }

        public void AddCharacterItemCount(string characterName, string itemName, int count, GameSourceDataBase gameData)
        {
            //gameData.TryGets
        }
        // TODO: write the following functions
        // TODO:   void AddCharacterItemCount(string characterName, string itemName, int count)
        // TODO:   this will add or remove the specified amount of items (pass -10 to remove 10 items)
        // TODO:   This function should check if an item is unique also and make sure that if a unique
        // TODO:   item is being added that it is only 1 or -1 and that if any other character has that
        // TODO:   item it should be removed from the other characters inventory.

    }
}
