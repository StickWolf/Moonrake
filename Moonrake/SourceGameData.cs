using GameEngine;
using GameEngine.Locations;
using System.Collections.Generic;

namespace GameData
{
    public class SourceGameData : IGameSourceData
    {
        public string DefaultPlayerName => "Eric";

        public string GameIntroductionText => 
            "Once, there were three ancient instruments:" +
            " The Harp, Piano, and the Drum." +
            " Inside of each instrument there was a magical gem:" +
            " A ruby, sapphire, and a diamond." +
            " When the gems are merged, it will the create an ancient weapon:" +
            " The Moonrake." +
            " Hello, Welcome to Moonrake, a text adventure game. ";

        public List<Character> Characters => new List<Character>()
        {
            new Character("Player", 50)
        };

        public List<Location> Locations => CreateLocations();

        public string StartingLocationName => "Tree House";

        private List<Location> CreateLocations()
        {
            var locations = new List<Location>();

            var locTreeHouse = new Location("Tree House", "a small tree house with a rope ladder and windows", 
                "you see a bed, blanket curtains and fridge full of soda while wooden planks smashed together for walls and"+
                "the floor. You have a very sertain feeling the tree house is going to break. "
                );
            locations.Add(locTreeHouse);

            var locField = new Location("Field", 
                "a beautiful grass field with trimmed lime-green grass with benches and flowers.",
                "flowers and lime-green grass surround you as you stroll and stand around in the field. "
                );
            locations.Add(locField);

            var locIceCreamShop = new Location("Ice Cream Shop",
                "an ice cream shop with a big sign on the door with tables under umbrellas around it.",
                "you see a cashier and candy, ice cream and tables with many people sitting in chairs around them. " +
                "The store has checkered walls and" +
                "a floor decorated in all sorts of different candies."
                );
            locations.Add(locIceCreamShop);

            locField.AddPortal(
                new PortalDestinationOpenGameVarRule(locIceCreamShop.Name,
                "Through the meduim wooden door you see","IceCreamShopDoor","open"),

                new PortalDestinationAlwaysClosedRule(locIceCreamShop.Name,
                "You see a medium sized wooden door under the big sign on the Ice Cream Shop")
                );

            locIceCreamShop.AddPortal(
                new PortalDestinationOpenGameVarRule(locField.Name,
                "Through the medium wooden door you see",
                "IceCreamShopDoorToTheField",
                "open"),

            new PortalDestinationAlwaysClosedRule(locField.Name, "you see a closed wooden door.")
                );

            locTreeHouse.AddPortal(
                new PortalDestinationAlwaysOpenRule(locField.Name,
                "Through the door shaped space you see")
                );

            locField.AddPortal(
                new PortalDestinationAlwaysOpenRule(locTreeHouse.Name,"Through the door shaped space you see")
                );


            return locations;
        }
    }
}
