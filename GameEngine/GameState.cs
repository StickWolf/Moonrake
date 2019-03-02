using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    /// <summary>
    /// The GameState class is meant to represent the current state of the game.
    /// This class holds any data that should be saved and reloaded for saved games.
    /// </summary>
    public class GameState
    {
        public string PlayerName { get; set; }

        public static List<string> GetValidSaveSlotNames()
        {
            // This function should list all save slots that currently have save data in them

            throw new NotImplementedException();
        }

        public static GameState LoadGameState(string slotName)
        {
            // This function should load the named slotName into a GameState and return it.
            throw new NotImplementedException();
        }

        public static void SaveGameState(string slotName, GameState gameState)
        {
            // This function should save the gameState that is passed in to the slotName slot

            throw new NotImplementedException();
        }
    }
}
