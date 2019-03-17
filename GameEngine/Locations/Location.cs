using System.Collections.Generic;

namespace GameEngine.Locations
{
    public class Location
    {
        public string Name { get; private set; }

        public List<LocationConnection> LocationConnections { get; private set; } = new List<LocationConnection>();

        public Location(string locationName)
        {
            Name = locationName;
        }
    }
}
