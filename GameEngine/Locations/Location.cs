using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Locations
{
    public class Location
    {
        /// <summary>
        /// This is the name of the location and must be unique within all locations.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// A description of what this location looks like when the user is at the location.
        /// </summary>
        public string LocalDescription { get; private set; }

        /// <summary>
        /// A description off what this location looks like when the user is
        /// looking ant the location from a remote location.
        /// </summary>
        public string RemoteDescription { get; private set; }

        /// <summary>
        /// A list of one-way connections that appear in this room and that
        /// potentially lead to another room
        /// </summary>
        public List<Portal> Portals { get; private set; } = new List<Portal>();

        /// <summary>
        /// Constructs a new Location
        /// </summary>
        /// <param name="remoteDescription">A description of what the location looks like from a remote location.</param>
        /// <param name="localDescription">A description of what the location looks like when the user is in it.</param>
        public Location(string locationName, string remoteDescription, string localDescription)
        {
            Name = locationName;
            LocalDescription = localDescription;
            RemoteDescription = remoteDescription;
        }
    }
}