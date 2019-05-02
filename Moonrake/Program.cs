using GameEngine;

namespace Moonrake
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineFactory.Start(() => new MoonrakeGameData());
        }
    }
}
