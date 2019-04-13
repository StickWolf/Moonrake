namespace Moonrake
{
    public class MoonRakeGameVariables
    {
        public string IceCreamShopDoor { get; private set; }

        public MoonRakeGameVariables(MoonrakeGameData gameData)
        {
            IceCreamShopDoor = gameData.AddDefaultGameVar("IceCreamShopDoor", "closed");
        }
    }
}
