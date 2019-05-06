using GameEngine.Characters;
using GameEngine.Commands;
using System;
using System.Collections.Generic;

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

        public Action NewGameFiller { get; private set; }

        public EngineInternal(Action newGameFiller)
        {
            NewGameFiller = newGameFiller;
        }

        /// <summary>
        /// Runs the game until they win, die or exit.
        /// </summary>
        public void StartEngine()
        {
            // Ask the player to pick to load a saved game if there are any
            CommandHelper.TryRunInternalCommand("load", new List<string>(), this);

            // Main game loop goes 1 loop for 1 game turn.
            while (RunGameLoop)
            {
                // Get all characters in the game that are still alive
                var allLocateableCharacters = GameState.CurrentGameState.GetAllCharacters();
                foreach (var gameCharacter in allLocateableCharacters) // TODO: Sort turn order by character speed, fastest should go first.
                {
                    // Only characters that are alive get a turn
                    if (gameCharacter.IsDead())
                    {
                        continue;
                    }

                    // If this is the player character, then call a special version of "Turn"
                    if (gameCharacter is PlayerCharacter)
                    {
                        (gameCharacter as PlayerCharacter).InternalTurn(this);
                    }
                    else
                    {
                        gameCharacter.Turn();
                    }
                }

                var playerCharacter = GameState.CurrentGameState.GetPlayerCharacter();
                if (playerCharacter.HitPoints <= 0)
                {
                    PlayerIsDead = true;
                }
                if (PlayerIsDead)
                {
                    Console.WriteLine();
                    Console.WriteLine("You have died. Please press a key.");
                    Console.ReadKey();
                    RunGameLoop = false;
                }
                // TODO: Find a way to figure out when the player has won.
                else if (PlayerHasWon)
                {
                    Console.WriteLine(GameState.CurrentGameState.GameEndingText);
                    Console.WriteLine("             |--The End--|             ");
                    Console.ReadLine();
                    RunGameLoop = false;
                }
            }
        }

        /// <summary>
        /// Sets up everything to start a new game and shows the game introduction text
        /// </summary>
        public void StartNewGame()
        {
            GameState.CreateNewGameState();
            NewGameFiller();

            // Show the intro
            Console.Clear();
            Console.WriteLine(GameState.CurrentGameState.GameIntroductionText);
            Console.WriteLine();

            var playerCharacter = GameState.CurrentGameState.GetPlayerCharacter();
            CommandHelper.TryRunPublicCommand("look", new List<string>(), playerCharacter.TrackingId);
        }
    }
}
