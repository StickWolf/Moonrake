using GameEngine;

namespace ExampleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineFactory.Start(CreateExampleGameData);
        }

        static GameSourceDataBase CreateExampleGameData()
        {
            return new ExampleSourceGameData();
        }
    }
}
