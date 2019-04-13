using GameEngine;

namespace ExampleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineFactory.Start(CreateExampleGameData);
        }

        static GameSourceData CreateExampleGameData()
        {
            return new ExampleGameSourceData();
        }
    }
}
