using ServerEngine;

namespace ExampleGameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineFactory.Start(
                () => new ExampleGameSourceData().NewWorld(),
                () => 
                {
                    var newPlayerCreator = new NewPlayerSourceData();
                    return newPlayerCreator.NewPlayer();
                }
                );
        }
    }
}
