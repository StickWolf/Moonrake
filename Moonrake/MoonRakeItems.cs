using GameEngine;
using System;

namespace Moonrake
{
    public class MoonRakeItems
    {
        public Guid WaterEye { get; private set; }

        public Guid Money { get; private set; }

        public Guid ChocolateIceCream { get; private set; }

        public Guid StrawberryIceCream { get; private set; }

        public Guid VanillaIceCream { get; private set; }

        public MoonRakeItems(MoonrakeGameData gameData)
        {
            WaterEye = gameData.AddItem(new Item("Water Eye") { IsUnique = true });
            Money = gameData.AddItem(new Item("Money"));
            ChocolateIceCream = gameData.AddItem(new Item("Chocolate Ice Cream"));
            StrawberryIceCream = gameData.AddItem(new Item("Strawberry Ice Cream"));
            VanillaIceCream = gameData.AddItem(new Item("Vanilla Ice Cream"));
        }
    }
}
