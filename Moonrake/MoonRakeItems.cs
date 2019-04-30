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
            WaterEye = GameState.CurrentGameState.AddItem(new Item("Water Eye") { IsUnique = true });
            Money = GameState.CurrentGameState.AddItem(new Item("Money"));
            ChocolateIceCream = GameState.CurrentGameState.AddItem(new Item("Chocolate Ice Cream"));
            StrawberryIceCream = GameState.CurrentGameState.AddItem(new Item("Strawberry Ice Cream"));
            VanillaIceCream = GameState.CurrentGameState.AddItem(new Item("Vanilla Ice Cream"));
        }
    }
}
