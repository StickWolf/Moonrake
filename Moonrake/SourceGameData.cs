using GameEngine;
using GameEngine.Locations;
using System.Collections.Generic;

namespace GameData
{
    public class SourceGameData : IGameSourceData
    {
        public string DefaultPlayerName => "Eric";

        public string GameIntroductionText => 
            "Once, there were three ancient instruments." +
            " The Harp, Piano, and the Drum." +
            " Inside of each instrument there was a magical gem." +
            " A ruby, sapphire, and a diamond." +
            " When the gems are merged, it will the create an ancient weapon;" +
            " The Moonrake." +
            " Hello, Welcome to Moonrake, a text adventure game. ";

        public List<Character> Characters => new List<Character>()
        {
            new Character("Player", 50)
        };

        public List<Location> Locations => CreateLocations();

        private List<Location> CreateLocations()
        {
            var locations = new List<Location>();

            var locAreaToTheWest = new Location("AreaToTheWest");
            locations.Add(locAreaToTheWest);

            var locAreaToTheEast = new Location("AreaToTheEast");
            locations.Add(locAreaToTheEast);

            var locStart = new Location("StartingArea");
            locStart.LocationConnections.Add(new LocationConnectionBasic(locAreaToTheWest.Name));
            locStart.LocationConnections.Add(new LocationConnectionGameVar(locAreaToTheEast.Name, "EastUnlock", "True"));
            locations.Add(locStart);

            return locations;
        }
    }
}
