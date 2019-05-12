using System;

namespace GameEngine
{
    public static class EngineFactory
    {
        /// <summary>
        /// Starts the factory.
        /// The factory will create games and start them over and over until stopped.
        /// </summary>
        /// <param name="newGameFiller">A method that fills out GameState for a new game</param>
        public static void Start(Action newGameFiller)
        {
            EngineInternal.NewGameFiller = newGameFiller;
            do
            {
                // Start the game
                EngineInternal.StartEngine();
            } while (EngineInternal.RunFactory);
        }
    }
}
