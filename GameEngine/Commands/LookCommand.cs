using System;
using System.Linq;

namespace GameEngine.Commands
{
    class LookCommand : ICommand
    {
        public void Exceute(EngineInternal engine)
        {
            // Figure out the location name of where the player is at
            var playerLocationName = GameState.CurrentGameState.CharacterLocations["Player"];

            // Get a referience to that location from the GameData
            var location = engine.GameData.Locations.FirstOrDefault(loc => loc.Name.Equals(playerLocationName));

            // Display the local description of the location
            Console.WriteLine(location?.LocalDescription);

            // Get all portals that have a rule that originates from the current location
            var originPortals = engine.GameData.Portals.Where(p => p.HasOriginLocation(playerLocationName));

            var portalDesinations = originPortals
                .Select(p => p.GetDestination(location.Name))
                .OrderBy(d => d.Destination);

            // Loop through all destinations in that location 
            foreach (var portalDest in portalDesinations)
            {
                // Evaluate each rule and choose what should be diplayed 
                if(portalDest.Description == null)
                {
                    // The player sees nothing
                } 
                else if(portalDest.Destination == null)
                {
                    // If we get here, the description exists, but the destination does not
                    Console.WriteLine(portalDest.Description);
                }
                else
                {
                    // If we got here, the description AND the destination exist.
                    var remoteLocation = engine.GameData.Locations.First(loc => loc.Name.Equals(portalDest.Destination));
                    Console.WriteLine($"[{portalDest.Destination}] {portalDest.Description} {remoteLocation.RemoteDescription}");
                }
            }
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("look", StringComparison.OrdinalIgnoreCase);
        }
    }
}