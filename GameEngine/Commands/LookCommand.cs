using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class LookCommand : ICommand
    {
        public void Exceute(GameSourceData gameData, List<string> extraWords)
        {
            // Figure out the location name of where the player is at
            var playerLocationName = GameState.CurrentGameState.GetCharacterLocation(PlayerCharacter.TrackingName);

            // Get a referience to that location from the GameData
            gameData.TryGetLocation(playerLocationName, out var location);

            // Display the local description of the location
            Console.WriteLine(location?.LocalDescription);

            // Get all portals that have a rule that originates from the current location
            var originPortals = gameData.Portals.Where(p => p.HasOriginLocation(playerLocationName));

            var portalDesinations = originPortals
                .Select(p => p.GetDestination(location.Name))
                .OrderBy(d => d.Destination);

            Console.WriteLine();
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
                    gameData.TryGetLocation(portalDest.Destination, out var remoteLocation);
                    Console.WriteLine($"[{portalDest.Destination}] {portalDest.Description} {remoteLocation.RemoteDescription}");
                }
            }

            var locationItems = GameState.CurrentGameState.GetLocationItems(playerLocationName);
            if (locationItems != null)
            {
                string itemDescriptions = string.Empty;
                var currentItemIndex = 0;
                foreach (var locationItem in locationItems)
                {
                    currentItemIndex++;
                    if (gameData.TryGetItem(locationItem.Key, out Item item))
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
                        itemDescriptions += item.GetDescription(locationItem.Value, GameState.CurrentGameState);
                    }
                }

                if (!string.IsNullOrWhiteSpace(itemDescriptions))
                {
                    Console.WriteLine();
                    Console.WriteLine($"You see {itemDescriptions}.");
                }
            }

            var otherCharactersInLocation = GameState.CurrentGameState.GetCharactersInLocation(playerLocationName);
            if(otherCharactersInLocation.Count != 0)
            {
                Console.WriteLine();
                Console.WriteLine("The following people are here:");
                foreach(var characterName in otherCharactersInLocation)
                {
                    Console.WriteLine($"{characterName}");
                }
            }
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("look", StringComparison.OrdinalIgnoreCase);
        }
    }
}