using GameEngine.Commands;
using System;

namespace GameEngine
{
    public static class EngineFactory
    {
        /// <summary>
        /// Starts the factory.
        /// The factory will create games and start them over and over until stopped.
        /// </summary>
        /// <param name="gameDataFactory">A method that returns a GameData</param>
        public static void Start(Func<IGameData> gameDataFactory)
        {
            bool keepPlaying = true;
            while (keepPlaying)
            {
                // Create a new game data instance
                var gameData = gameDataFactory();

                // Ask the player to pick to load a saved game if there are any
                var loadCommand = CommandHelper.GetCommand("load");
                loadCommand.Exceute(null);

                // Create a new engine 
                var engine = new EngineInternal(gameData);

                // Start the game
                engine.Start();
            }
        }
    }
}
