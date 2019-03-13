using GameEngine.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    internal class EngineInternal
    {
        public bool RunGameLoop { get; set; } = true;
        public bool PlayerIsDead { get; set; } = false;
        public bool PlayerHasWon { get; set; } = false;

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
            Console.WriteLine($"Your name is {GameState.CurrentGameState.PlayerName} would you like to change it? Yes/No");

            string answer = Console.ReadLine();
            if (answer.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write($"What would you like your name to be?: ");
                GameState.CurrentGameState.PlayerName = Console.ReadLine();
                Console.WriteLine($"Very well, you are now {GameState.CurrentGameState.PlayerName}.");
            }
        }

        /// <summary>
        /// Runs the game until they win or die.
        /// </summary>
        public void Start()
        {
            if (GameState.CurrentGameState == null)
            {
                GameState.CurrentGameState = new GameState();
                GameState.CurrentGameState.PlayerName = gameData.DefaultPlayerName;

                string gameIntroductionText = gameData.GetGameIntroduction().AddLineReturns(true);
                Console.Clear();
                Console.WriteLine(gameIntroductionText);
                Console.WriteLine();

                // Give the player the option to change their name if desired before beginning the game.
                LetPlayerChangeTheirName();
            }

            // Main game loop
            GameLoop();
        }

        /// <summary>
        /// The main game loop
        /// </summary>
        private void GameLoop()
        {
            // Let the player keep playing until they are either dead, they won the game or they want to quit.
            // The main game loop, 1 loop = 1 game turn
            while (RunGameLoop)
            {
                ProcessUserInput();

                // TODO: Fix this to check the actual player instead of hardcoding true here.
                if (PlayerIsDead)
                {
                    Console.WriteLine();
                    Console.WriteLine("You have died. Please press a key.");
                    Console.ReadKey();
                    RunGameLoop = false;
                }
                else if (PlayerHasWon)
                {
                    // TODO: Print out the end of game story. This should be provided by the game data.
                    RunGameLoop = false;
                }
            }
        }

        private void ProcessUserInput()
        {
            string input;
            Console.Write(">");
            input = Console.ReadLine();
            Console.Clear();
            var partsOfInput = input.Split(' ');
            var firstWord = partsOfInput[0];

            var commandToRun = CommandHelper.GetCommand(firstWord);
            if (commandToRun == null)
            {
                Console.WriteLine($"I don't know what you mean by '{firstWord}'.");
                return;
            }

            // The command is a real command if we got this far
            commandToRun.Exceute(this);
        }
    }
}
