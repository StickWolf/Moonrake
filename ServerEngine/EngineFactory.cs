using ServerEngine.Characters;
using System;

namespace ServerEngine
{
    public static class EngineFactory
    {
        /// <summary>
        /// Starts the factory.
        /// The factory will create games and start them over and over until stopped.
        /// </summary>
        /// <param name="newWorldCreator">A method that fills out GameState for a new world</param>
        /// <param name="newPlayerCreator">A method that creates a new player character</param>
        public static void Start(Action newWorldCreator, Func<Character> newPlayerCreator)
        {
            EngineInternal.NewWorldCreator = newWorldCreator;
            EngineInternal.NewPlayerCreator = newPlayerCreator;

            do
            {
                // Start the game
                EngineInternal.StartEngine();
            } while (EngineInternal.RunFactory);
        }
    }
}
