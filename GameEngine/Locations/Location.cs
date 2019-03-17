using System.Collections.Generic;

namespace GameEngine.Locations
{
    public class Location
    {
        public string Name { get; private set; }

        public List<Portal> Portals { get; private set; } = new List<Portal>();

        public Location(string locationName)
        {
            Name = locationName;
        }

        /// <summary>
        /// Helper method that can be used to add a portal to a location more elegantly
        /// </summary>
        /// <param name="destinationRules">An array of portal rules to apply in order to the portal</param>
        public void AddPortal(params PortalDestinationRule[] destinationRules)
        {
            var portal = new Portal();
            portal.DestinationRules = new List<PortalDestinationRule>();
            portal.DestinationRules.AddRange(destinationRules);
        }
    }
}
