using ServerEngine;

namespace DragonKittyServer
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineFactory.Start(
                () => new DragonKittySourceData().NewWorld(),
                () =>
                {
                    var newPlayerCreator = new NewPlayerSourceData();
                    return newPlayerCreator.NewPlayer();
                }
                );
        }
    }
}
