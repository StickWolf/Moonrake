using GameEngine;

namespace Moonrake
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineFactory.Start(CreateMoonrakeGameData);
        }

        static GameSourceDataBase CreateMoonrakeGameData()
        {
            return new GameData.MoonrakeGameData();
        }
    }
}
