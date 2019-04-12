using GameEngine;

namespace Moonrake
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineFactory.Start(CreateMoonrakeGameData);
        }

        static GameSourceData CreateMoonrakeGameData()
        {
            return new MoonrakeGameData();
        }
    }
}
