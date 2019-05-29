﻿using ServerEngine;
using ServerEngine.Locations;
using System;

namespace Moonrake
{
    public class MoonRakeLocations
    {
        public Guid TreeHouse { get; private set; }

        public Guid Field { get; private set; }

        public Guid IceCreamShop { get; private set; }

        public Guid TheBurgerDimplomatShop { get; private set; }

        public Guid BigPepperShop { get; private set; }

        public void NewWorld(MoonrakeGameData gameData)
        {
            TreeHouse = GameState.CurrentGameState.AddLocation(new Location("Tree House", "what looks like a small living space.",
                "You are in a tree house. You see a bed covered with a blanket, curtains on the windows and a fridge full of soda. " +
                "White wooden planks smashed together make up the walls and the floor. " +
                "You have a very certain feeling the tree house is going to break. "
                ));

            Field = GameState.CurrentGameState.AddLocation(new Location("Field", "a beautifuly trimmed field of lime-green grass and flowers.",
                "You are in an open field, flowers and lime-green grass surround you. A number of benches provide a resting place for visitors. " +
                "There is a small tree house here with a rope ladder and windows. " +
                "In the center of the field you see an ice cream shop with a big sign on the door. There are tables with umbrellas around the shop."
                ));

            IceCreamShop = GameState.CurrentGameState.AddLocation(new Location("Ice Cream Shop", "an ice cream shop full of people at tables.",
                "You see many containers of candy, an ice cream machine and many people sitting in chairs around tables. " +
                "A cashier resides in the central area of the shop. " +
                "The store has checkered walls and a floor decorated with many pantings of candies."
                ));

            TheBurgerDimplomatShop = GameState.CurrentGameState.AddLocation(new Location("The Burger Dimplomat Shop",
                "a large grey building with a red and white checker pattern line at the bottom.", 
                "black-colored tables that are evenly spaced out. In one corner, there is a soda vending machine."));

            BigPepperShop = GameState.CurrentGameState.AddLocation(new Location("Big Pepper", 
                "a small, tan-colored restuarant with rainbow-colored lighted letters that read out Big Pepper",
                "Brown colored tables are arranged in lines 2 by 7. you see a big menu with delicious sounding mexican foods"));

            // Tree House <--> Field
            GameState.CurrentGameState.AddPortal(
                new PortalAlwaysOpenRule(TreeHouse, Field, "Through the door shaped space you see"),

                new PortalAlwaysOpenRule(Field, TreeHouse, "Through the tree house door shaped space you see")
                );

            // Field <--> Ice cream shop
            GameState.CurrentGameState.AddPortal(
                new PortalOpenGameVarRule(Field, IceCreamShop,
                "Through the Ice Cream Shop's medium wooden door you see", gameData.MoonRakeGameVariables.IceCreamShopDoor, "open"),
                new PortalAlwaysClosedRule(Field, Guid.Empty,
                "You see a closed medium sized wooden door under the big sign on the Ice Cream Shop."),

                new PortalOpenGameVarRule(IceCreamShop, Field,
                "Through the medium wooden door you see", gameData.MoonRakeGameVariables.IceCreamShopDoor, "open"),
                new PortalAlwaysClosedRule(IceCreamShop, Guid.Empty,
                "You see a closed medium sized wooden door inside the Ice Cream Shop.")
                );
        }
    }
}