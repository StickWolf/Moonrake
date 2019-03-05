using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GameEngine
{
    /// <summary>
    /// The GameState class is meant to represent the current state of the game.
    /// This class holds any data that should be saved and reloaded for saved games.
    /// </summary>
    public class GameState
    {
        public string PlayerName { get; set; }
        private static string SaveFileName { get; set; } = "GameSaves.json";

        public static List<string> GetValidSaveSlotNames()
        {
            // This function should list all save slots that currently have save data in them
            throw new NotImplementedException();
        }

        public static GameState LoadGameState(string slotName)
        {
            // read the save file into a string
            string fileContents = File.ReadAllText(SaveFileName);
            // deserialize the string into a dictionary
            var savedGames = JsonConvert.DeserializeObject<Dictionary<string, GameState>>(fileContents);
            // get the specified value out of the dictionary using slotName as the key
            var game = savedGames[slotName];
            // return the value which should be the game state
            return game;
        }

        public static void SaveGameState(string slotName, GameState gameState)
        {
            // This function should save the gameState that is passed in to the slotName slot

            throw new NotImplementedException();
        }
    }
}
