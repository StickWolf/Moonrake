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

        public Guid CheeseBurger { get; private set; }

        public Guid Burger { get; private set; }

        public Guid Fries { get; private set; }

        public Guid Soda { get; private set; }

        public Guid Burrito { get; private set; }

        public Guid Taco { get; private set; }

        public Guid Rice { get; private set; }

        public void NewGame(MoonrakeGameData gameData)
        {
            WaterEye = GameState.CurrentGameState.AddItem(new Item("Water Eye") { IsUnique = true });
            Money = GameState.CurrentGameState.AddItem(new Item("Money"));
            ChocolateIceCream = GameState.CurrentGameState.AddItem(new Item("Chocolate Ice Cream"));
            StrawberryIceCream = GameState.CurrentGameState.AddItem(new Item("Strawberry Ice Cream"));
            VanillaIceCream = GameState.CurrentGameState.AddItem(new Item("Vanilla Ice Cream"));
            CheeseBurger = GameState.CurrentGameState.AddItem(new Item("Cheese Burger"));
            Burger = GameState.CurrentGameState.AddItem(new Item("Burger"));
            Fries = GameState.CurrentGameState.AddItem(new Item("Fries"));
            Soda = GameState.CurrentGameState.AddItem(new Item("Soda"));
            Burrito = GameState.CurrentGameState.AddItem(new Item("Burrito"));
            Taco = GameState.CurrentGameState.AddItem(new Item("Taco"));
            Rice = GameState.CurrentGameState.AddItem(new Item("Rice"));
        }
    }
}
