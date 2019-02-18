using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Engine
    {
        private List<Character> AllCharacters { get; set; } = new List<Character>();

        private IGameData gameData;

        public Engine(IGameData gameData)
        {
            this.gameData = gameData;

            // Ask the gamedata class to create all the characters that will be needed for the game
            // and fill up our local list with the objects we get back.
            var characters = gameData.CreateAllGameCharacters();
            AllCharacters.AddRange(characters);
        }

        public bool Start()
        {
            // Show the game introduction
            string intro = gameData.GetGameIntroduction();
            Console.Clear();
            Console.WriteLine(intro);

            // The main game loop, 1 loop = 1 game turn
            while (true)
            {
                // TODO: decide everything here that should happen in the main game loop
                Console.ReadLine();

                // TODO: if the player is dead, or if the game is complete then break out of this loop.
                // TODO: this should eventually be an if check surrounding this break.
                break;
            }

            // Prompt the player to see if they want to play the game again.
            // TODO: When we get the ability to save the game implemented, rewrite this code so instead
            // TODO: it lets the player start over from the last time they saved instead of starting the whole game again.
            Console.WriteLine("Would you like to play again (yes/no)?");
            return Console.ReadLine().Equals("yes", StringComparison.OrdinalIgnoreCase);
        }
    }
}
