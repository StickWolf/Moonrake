using GameEngine.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Commands
{
    class LookCommand : ICommand
    {
        public void Exceute(EngineInternal engine)
        {
            // Figure out the location name of where the player is at
            var characterLocationName = GameState.CurrentGameState.CharacterLocations["Player"];
            // Get a referience to that location from the GameData
            var location = engine.GameData.Locations.FirstOrDefault(loc => loc.Name.Equals(characterLocationName));

            // Display the local dicription of the location
            Console.WriteLine(location?.LocalDescription.AddLineReturns(true));

            // Loop through all portals in that location 
            foreach(var portal in location.Portals)
            {
                // get the portal destination
                var portalDest = portal.GetDestination();


                // - elavuate eatch rule and choose what should be diplayed 
                if(portalDest.Description == null)
                {
                    // The player sees nothing
                } 
                else if(portalDest.Destination == null)
                {
                    // If we get here, the Description exists, but the destination does not
                    Console.WriteLine(portalDest.Description.AddLineReturns(true));
                }
                else
                {
                    // If we got here, the Description AND the destiation exist.
                    var remoteLocation = engine.GameData.Locations.First(loc => loc.Name.Equals(portalDest.Destination));
                    string discriptionAndDestination = portalDest.Description + ' ' + remoteLocation.RemoteDescription;
                    // TODO: write a new specialized Console class that can add these line returns automatically
                    Console.WriteLine(discriptionAndDestination.AddLineReturns(true));
                }
                //if portal is open then get remote dicripion
            }
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("look", StringComparison.OrdinalIgnoreCase);
        }
    }
}