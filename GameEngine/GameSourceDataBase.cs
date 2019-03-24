using GameEngine.Locations;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine
{
    /// <summary>
    /// This class provides methods that help create a GameSourceData
    /// </summary>
    public abstract class GameSourceDataBase
    {
        public string DefaultPlayerName { get; protected set; }

        public string GameIntroductionText { get; protected set; }

        public List<Character> Characters { get; private set; } = new List<Character>();

        public List<Location> Locations { get; private set; } = new List<Location>();

        public string StartingLocationName { get; protected set; }

        public List<Portal> Portals { get; private set; } = new List<Portal>();

        /// <summary>
        /// Helper method that can be used to add a portal more elegantly
        /// </summary>
        /// <param name="destinationRules">An array of portal rules to apply in order to the portal</param>
        public void AddPortal(params PortalRule[] destinationRules)
        {
            Portals.Add(new Portal(destinationRules.ToList()));
        }
    }
}
