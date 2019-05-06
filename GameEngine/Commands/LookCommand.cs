using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class LookCommand : ICommand
    {
        public void Execute(List<string> extraWords, Guid lookingCharacterTrackingId)
        {
            // TODO: Instead pass this in from the character that is using the command
            var lookingCharacter = GameState.CurrentGameState.GetPlayerCharacter();
            var characterLocation = GameState.CurrentGameState.GetCharacterLocation(lookingCharacter.TrackingId);

            // Display the local description of the location
            Console.WriteLine(characterLocation?.LocalDescription);

            // Get all portals that have a rule that originates from the current location
            var originPortals = GameState.CurrentGameState.GetPortalsInLocation(characterLocation.TrackingId);

            var portalDesinations = originPortals
                .Select(p => p.GetDestination(characterLocation.TrackingId));
                //.OrderBy(d => d.Destination); // TODO: Bring sorting back

            Console.WriteLine();
            // Loop through all destinations in that location 
            foreach (var portalDest in portalDesinations)
            {
                // Evaluate each rule and choose what should be diplayed 
                if(portalDest.Description == null)
                {
                    // The player sees nothing
                } 
                else if(portalDest.DestinationTrackingId == Guid.Empty)
                {
                    // If we get here, the description exists, but the destination does not
                    Console.WriteLine(portalDest.Description);
                }
                else
                {
                    // If we got here, the description AND the destination exist.
                    var remoteLocation = GameState.CurrentGameState.GetLocation(portalDest.DestinationTrackingId);
                    Console.WriteLine($"[{remoteLocation.LocationName}] {portalDest.Description} {remoteLocation.RemoteDescription}");
                }
            }

            var locationItems = GameState.CurrentGameState.GetLocationItems(characterLocation.TrackingId);
            if (locationItems != null)
            {
                string itemDescriptions = string.Empty;
                var currentItemIndex = 0;
                foreach (var locationItem in locationItems)
                {
                    currentItemIndex++;
                    var item = locationItem.Key;
                    if (item != null)
                    {
                        // Skip items that are not visible
                        if (!item.IsVisible)
                        {
                            continue;
                        }

                        if (itemDescriptions != string.Empty)
                        {
                            itemDescriptions += (currentItemIndex == locationItems.Count) ? " and " : ", ";
                        }
                        itemDescriptions += item.GetDescription(locationItem.Value);
                    }
                }

                if (!string.IsNullOrWhiteSpace(itemDescriptions))
                {
                    Console.WriteLine();
                    Console.WriteLine($"You see {itemDescriptions}.");
                }
            }

            var otherCharactersInLocation = GameState.CurrentGameState.GetCharactersInLocation(characterLocation.TrackingId, includePlayer: false);
            if(otherCharactersInLocation.Count != 0)
            {
                Console.WriteLine();
                Console.WriteLine("The following other characters are here:");
                foreach(var character in otherCharactersInLocation)
                {
                    Console.WriteLine($"{character.Name}");
                }
            }
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("look", StringComparison.OrdinalIgnoreCase);
        }
    }
}