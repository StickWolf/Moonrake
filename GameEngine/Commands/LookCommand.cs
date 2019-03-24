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
            Console.WriteLine(location?.LocalDescription.AddLineReturns(true));

            // Get all portals that have a rule that originates from the current location
            var originPortals = engine.GameData.Portals.Where(p => p.HasOriginLocation(playerLocationName));

            // Loop through all portals in that location 
            foreach (var portal in originPortals)
            {
                // get the portal destination
                var portalDest = portal.GetDestination(location.Name);

                // Evaluate each rule and choose what should be diplayed 
                if(portalDest.Description == null)
                {
                    // The player sees nothing
                } 
                else if(portalDest.Destination == null)
                {
                    // If we get here, the description exists, but the destination does not
                    Console.WriteLine(portalDest.Description.AddLineReturns(true));
                }
                else
                {
                    // If we got here, the description AND the destiation exist.
                    var remoteLocation = engine.GameData.Locations.First(loc => loc.Name.Equals(portalDest.Destination));
                    string discriptionAndDestination = portalDest.Description + ' ' + remoteLocation.RemoteDescription;
                    // TODO: write a new specialized Console class that can add these line returns automatically
                    Console.WriteLine(discriptionAndDestination.AddLineReturns(true));
                }
            }
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("look", StringComparison.OrdinalIgnoreCase);
        }
    }
}