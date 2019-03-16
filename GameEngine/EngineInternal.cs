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
        /// <summary>
        /// Indicates if the main game loop should keep running game turns
        /// </summary>
        public bool RunGameLoop { get; set; } = true;

        /// <summary>
        /// If set to false, triggers the engine factory to stop generating new engines and exit.
        /// </summary>
        public bool RunFactory { get; set; } = true;

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
        

        /// <summary>
        /// Runs the game until they win, die or exit.
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
