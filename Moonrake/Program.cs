using GameData;
using GameEngine;

namespace Moonrake
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameData = new Data();
            var gameEngine = new Engine(gameData);

            gameEngine.Start();
        }
    }
}
