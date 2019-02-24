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
    }
}
