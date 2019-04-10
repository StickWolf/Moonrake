using GameEngine;

namespace DreamsAndWhatTheyMean
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineFactory.Start(CreateDragonKittyGameData);
        }

        static GameSourceData CreateDragonKittyGameData()
        {
            return new TheTaleOfTheDragonKittyGameData();
        }
    }
}
