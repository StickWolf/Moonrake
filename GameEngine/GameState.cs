using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace GameEngine
{
    /// <summary>
    /// The GameState class is meant to represent the current state of the game.
    /// This class holds any data that should be saved and reloaded for saved games.
    /// </summary>
    public class GameState
    {
        // This data goes into save files
        public string PlayerName { get; set; }

        // CharacterLocations[{CharacterName}] = {LocationName}
        public Dictionary<string, string> CharacterLocations { get; set; } = new Dictionary<string, string>();

        // GameVars[{GameVarName}] = {GameVarValue}
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
    }
}
