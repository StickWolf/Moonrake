using ServerEngine;
using ServerEngine.GrainSiloAndClient;

namespace Moonrake
{
    public class MoonRakeGameVariables
    {
        public string IceCreamShopDoor { get; private set; }

        public void NewWorld(MoonrakeGameData gameData)
        {
            IceCreamShopDoor = GrainClusterClient.Universe.SetGameVarValue("IceCreamShopDoor", "open").Result;
        }
    }
}
