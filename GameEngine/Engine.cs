using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Engine
    {
        private List<Character> AllCharacters { get; set; }

        private IGameData gameData;

        public Engine(IGameData gameData)
        {
            this.gameData = gameData;

            // Ask the gamedata class to create all the characters that will be needed for the game
            // and fill up our local list with the objects we get back.
            var characters = gameData.CreateAllGameCharacters();
            AllCharacters.AddRange(characters);
        }

        public void Start()
        {
            while (true)
            {
                // TODO: decide everything here that should happen in the main game loop
            }
        }
    }
}
