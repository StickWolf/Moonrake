using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            WaterEye = gameData.AddItem(new Item("Water Eye") { IsUnique = true });
            Money = gameData.AddItem(new Item("Money") { IsUnique = false });
            ChocolateIceCream = gameData.AddItem(new Item("Chocolate Ice Cream") { IsUnique = false });
            StrawberryIceCream = gameData.AddItem(new Item("Strawberry Ice Cream") { IsUnique = false });
            VanillaIceCream = gameData.AddItem(new Item("Vanilla Ice Cream") { IsUnique = false });
        }
    }
}
