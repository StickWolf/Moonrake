using GameEngine;

namespace Moonrake
{
    public class MoonRakeItems
    {
        public string WaterEye { get; private set; }

        public string Money { get; private set; }

        public string ChocolateIceCream { get; private set; }

        public string StrawberryIceCream { get; private set; }

        public string VanillaIceCream { get; private set; }

        public MoonRakeItems(MoonrakeGameData gameData)
        {
            WaterEye = gameData.AddItem(new Item("WaterEye","Water Eye") { IsUnique = true });
            Money = gameData.AddItem(new Item("Money","Money"));
            ChocolateIceCream = gameData.AddItem(new Item("ChocolateIceCream","Chocolate Ice Cream"));
            StrawberryIceCream = gameData.AddItem(new Item("StrawberryIceCream","Strawberry Ice Cream"));
            VanillaIceCream = gameData.AddItem(new Item("VanillaIceCream","Vanilla Ice Cream"));
        }
    }
}
