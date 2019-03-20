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

            // TODO: create the locations

            return locations;
        }
    }
}
