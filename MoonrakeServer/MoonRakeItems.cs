using ServerEngine;
using ServerEngine.GrainSiloAndClient;
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

        public void NewWorld(MoonrakeGameData gameData)
        {
            WaterEye = GrainClusterClient.Universe.AddItem(new Item("Water Eye") { IsUnique = true }).Result;
            Money = GrainClusterClient.Universe.AddItem(new Item("Money")).Result;
            ChocolateIceCream = GrainClusterClient.Universe.AddItem(new Item("Chocolate Ice Cream")).Result;
            StrawberryIceCream = GrainClusterClient.Universe.AddItem(new Item("Strawberry Ice Cream")).Result;
            VanillaIceCream = GrainClusterClient.Universe.AddItem(new Item("Vanilla Ice Cream")).Result;
            CheeseBurger = GrainClusterClient.Universe.AddItem(new Item("Cheese Burger")).Result;
            Burger = GrainClusterClient.Universe.AddItem(new Item("Burger")).Result;
            Fries = GrainClusterClient.Universe.AddItem(new Item("Fries")).Result;
            Soda = GrainClusterClient.Universe.AddItem(new Item("Soda")).Result;
            Burrito = GrainClusterClient.Universe.AddItem(new Item("Burrito")).Result;
            Taco = GrainClusterClient.Universe.AddItem(new Item("Taco")).Result;
            Rice = GrainClusterClient.Universe.AddItem(new Item("Rice")).Result;
        }
    }
}
