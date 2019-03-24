using GameEngine;
using GameEngine.Locations;
using System.Collections.Generic;

namespace GameData
{
    public class MoonrakeGameData : GameSourceDataBase
    {
        public MoonrakeGameData()
        {
            DefaultPlayerName = "Eric";

            GameIntroductionText = "Once, there were three ancient instruments:" +
                " The Harp, Piano, and the Drum." +
                " Inside of each instrument there was a magical gem:" +
                " A ruby, sapphire, and a diamond." +
                " When the gems are merged, it will the create an ancient weapon:" +
                " The Moonrake." +
                " Hello, Welcome to Moonrake, a text adventure game. ";

            Characters.Add(new Character("Player", 50));

            #region GameVars

            var gvIceCreamShopDoor = AddDefaultGameVar("IceCreamShopDoor", "closed");

            #endregion

            #region Locations

            var locTreeHouse = new Location("Tree House", "what looks like a small living space",
                "you see a bed, blanket curtains and fridge full of soda while wooden planks smashed together for walls and" +
                "the floor. You have a very sertain feeling the tree house is going to break. "
                );
            Locations.Add(locTreeHouse);
            StartingLocationName = locTreeHouse.Name; // This is the starting location

            var locField = new Location("Field", "a beautiful trimmed lime-green grass field with benches and flowers.",
                "flowers and lime-green grass surround you as you stroll and stand around in the field. " +
                "There is a small tree house here with a rope ladder and windows. " +
                "You see an ice cream shop with a big sign on the door. There are tables with umbrellas around the shop."
                );
            Locations.Add(locField);

            var locIceCreamShop = new Location("Ice Cream Shop", "an ice cream shop full of people at tables",
                "you see a cashier and candy, ice cream and tables with many people sitting in chairs around them. " +
                "The store has checkered walls and a floor decorated in all sorts of different candies."
                );
            Locations.Add(locIceCreamShop);

            #endregion

            #region Portals

            // Tree house <--> Field
            AddPortal(
                new PortalAlwaysOpenRule(locTreeHouse.Name, locField.Name, "Through the door shaped space you see"),

                new PortalAlwaysOpenRule(locField.Name, locTreeHouse.Name, "Through the tree house door shaped space you see")
                );

            // Field <--> Ice cream shop
            AddPortal(
                new PortalOpenGameVarRule(locField.Name, locIceCreamShop.Name,
                "Through the Ice Cream Shop's medium wooden door you see", gvIceCreamShopDoor, "open"),
                new PortalAlwaysClosedRule(locField.Name, null,
                "You see a closed medium sized wooden door under the big sign on the Ice Cream Shop"),

                new PortalOpenGameVarRule(locIceCreamShop.Name, locField.Name,
                "Through the medium wooden door you see", gvIceCreamShopDoor, "open"),
                new PortalAlwaysClosedRule(locIceCreamShop.Name, null,
                "You see a closed medium sized wooden door inside the Ice Cream Shop")
                );

            #endregion

        }
    }
}
