using ServerEngine;

namespace Moonrake
{
    public class MoonRakeGameVariables
    {
        public string IceCreamShopDoor { get; private set; }

        public void NewWorld(MoonrakeGameData gameData)
        {
            IceCreamShopDoor = GameState.CurrentGameState.SetGameVarValue("IceCreamShopDoor", "open");
        }
    }
}
