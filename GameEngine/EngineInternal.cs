using GameEngine.Characters;
using GameEngine.Characters.Behaviors;
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
            // We can't get the real PlayerCharacter because the game isn't loaded yet.
            // But we can fake it out for this call
            var playerCharacter = new PlayerCharacter("loader", 1);
            InternalCommandHelper.TryRunInternalCommand("load", new List<string>(), this, playerCharacter);

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

                if (playerCharacter.IsDead())
                {
                    playerCharacter.SendMessage();
                    playerCharacter.SendMessage("You have died. Please press a key.");
                    Console.ReadKey();
                    RunGameLoop = false;
                }
                // TODO: Find a way to figure out when the player has won.
                else if (PlayerHasWon)
                {
                    playerCharacter.SendMessage(GameState.CurrentGameState.GameEndingText);
                    playerCharacter.SendMessage("             |--The End--|             ");
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
            // Create a new game
            GameState.CreateNewGameState();

            // Add built-in things
            GameState.CurrentGameState.AddTurnBehavior(BuiltInTurnBehaviors.Random, new TurnBehaviorRandom());
            // TODO: should commands also be added to GameState like this so games can provide custom commands?

            // Have the game fill in its game data
            NewGameFiller();

            // Show the intro and take a look around
            var playerCharacter = GameState.CurrentGameState.GetPlayerCharacter();
            PublicCommandHelper.TryRunPublicCommand("clear", new List<string>(), playerCharacter);
            playerCharacter.SendMessage(GameState.CurrentGameState.GameIntroductionText);
            playerCharacter.SendMessage();
            PublicCommandHelper.TryRunPublicCommand("look", new List<string>(), playerCharacter);
        }
    }
}
