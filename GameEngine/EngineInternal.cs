using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    internal class EngineInternal
    {
        private GameState CurrentGameState { get; set; }

        private List<Character> AllCharacters { get; set; } = new List<Character>();

        private IGameData gameData;

        public EngineInternal(IGameData gameData)
        {
            this.gameData = gameData;

            // Ask the gamedata class to create all the characters that will be needed for the game
            // and fill up our local list with the objects we get back.
            var characters = gameData.CreateAllGameCharacters();
            AllCharacters.AddRange(characters);
        }

        /// <summary>
        /// Gives the player an option to change their name
        /// </summary>
        public void LetPlayerChangeTheirName()
        {
            Console.WriteLine($"Your name is {CurrentGameState.PlayerName} would you like to change it? Yes/No");

            string answer = Console.ReadLine();
            if (answer.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write($"What would you like your name to be?: ");
                CurrentGameState.PlayerName = Console.ReadLine();
                Console.WriteLine($"Very well, you are now {CurrentGameState.PlayerName}.");
            }
        }

        /// <summary>
        /// Runs the game until they win or die.
        /// </summary>
        public void Start(GameState startingGameState)
        {
            if (startingGameState != null)
            {
                CurrentGameState = startingGameState;
            }
            else
            {
                CurrentGameState = new GameState();
                CurrentGameState.PlayerName = gameData.DefaultPlayerName;

                string gameIntroductionText = gameData.GetGameIntroduction().AddLineReturns(true);
                Console.Clear();
                Console.WriteLine(gameIntroductionText);
                Console.WriteLine();

                // Give the player the option to change their name if desired before beginning the game.
                LetPlayerChangeTheirName();
            }

            // Main game loop
            Start();
        }

        /// <summary>
        /// The main game loop
        /// </summary>
        private void Start()
        {
            // Let the player keep playing until they are either dead, they won the game or they want to quit.
            // The main game loop, 1 loop = 1 game turn
            while (true)
            {
                // TODO: Decide everything here that should happen in the main game loop

                Console.ReadLine();

                // TODO: If the user types in exit, ask they if they want to save first, then set keepPlaying to false and break;

                // TODO: If the user types in load, figure out what slot they want to load from e.g. "load 3" will load from slot 3.
                // TODO: set loadFromSlot to the correct number and break. This will assure we clear the engine of the current game
                // TODO: and are starting from the save correctly.

                // TODO: Fix this to check the actual player instead of hardcoding true here.
                bool playerIsDead = true;
                if (playerIsDead)
                {
                    Console.WriteLine();
                    Console.WriteLine("You have died. Please press a key.");
                    Console.ReadKey();
                    break;
                }

                bool playerHasWon = true;
                if (playerHasWon)
                {
                    // TODO: Print out the end of game story. This should be provided by the game data.
                    break;
                }
            }
        }
    }
}
