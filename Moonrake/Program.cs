using GameData;
using GameEngine;
using System;

namespace Moonrake
{
    class Program
    {
        static void Main(string[] args)
        {
            // Keep running new games of MoonRake as long as the user wants to keep playing
            EngineFactory.Start(CreateMoonrakeGameData);
        }

        static IGameSourceData CreateMoonrakeGameData()
        {
            return new GameData.SourceGameData();
        }
    }
}
