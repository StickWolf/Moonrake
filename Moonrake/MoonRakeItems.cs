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

        public string CheeseBurger { get; private set; }

        public string Burger { get; private set; }

        public string Fries { get; private set; }

        public string Soda { get; private set; }

        public string Burrito { get; private set; }

        public string Taco { get; private set; }

        public string Rice { get; private set; }

        public MoonRakeItems(MoonrakeGameData gameData)
        {
            WaterEye = gameData.AddItem(new Item("WaterEye","Water Eye") { IsUnique = true });
            Money = gameData.AddItem(new Item("Money","Money"));
            ChocolateIceCream = gameData.AddItem(new Item("ChocolateIceCream","Chocolate Ice Cream"));
            StrawberryIceCream = gameData.AddItem(new Item("StrawberryIceCream","Strawberry Ice Cream"));
            VanillaIceCream = gameData.AddItem(new Item("VanillaIceCream","Vanilla Ice Cream"));
            CheeseBurger = gameData.AddItem(new Item("CheeseBurger", "Cheese Burger"));
            Burger = gameData.AddItem(new Item("Burger", "Burger"));
            Fries = gameData.AddItem(new Item("Fries", "Fries"));
            Soda = gameData.AddItem(new Item("Soda", "Soda"));
            Burrito = gameData.AddItem(new Item("Burrito", "Burrito"));
            Taco = gameData.AddItem(new Item("Taco", "Taco"));
            Rice = gameData.AddItem(new Item("Rice", "Rice"));
        }
    }
}
