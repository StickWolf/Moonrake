using GameEngine;

namespace DreamsAndWhatTheyMean
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineFactory.Start(
                () => new TheTaleOfTheDragonKittySourceData().NewWorld(),
                () =>
                {
                    var newPlayerCreator = new NewPlayerSourceData();
                    return newPlayerCreator.NewPlayer();
                }
                );
        }
    }
}
