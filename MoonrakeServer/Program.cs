using ServerEngine;

namespace Moonrake
{
    class Program
    {
        static void Main(string[] args)
        {
            EngineFactory.Start(
                () => new MoonrakeGameData().NewWorld(),
                () => 
                {
                    var newPlayerCreator = new NewPlayerSourceData();
                    return newPlayerCreator.NewPlayer();
                }
                );
        }
    }
}
