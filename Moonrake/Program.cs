using GameData;
using GameData.Example;
using GameEngine;
using System;

namespace Moonrake
{
    class Program
    {
        static void Main(string[] args)
        {
            // Keep running new games of MoonRake as long as the user wants to keep playing
            EngineFactory.Start(CreateMoonrakeGameData); // TODO: uncomment this to run the real game
            //EngineFactory.Start(CreateExampleGameData);
        }

        static IGameSourceData CreateMoonrakeGameData()
        {
            return new GameData.SourceGameData();
        }

        // TODO: instead create a command that would stop the current game and start the example game so that it can be fully triggered from within the game engine itself
        // TODO: and not have to rely on code in moonrake like it is now.
        static IGameSourceData CreateExampleGameData()
        {
            return new ExampleSourceGameData();
        }
    }
}
