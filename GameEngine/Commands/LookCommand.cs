using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class LookCommand : ICommand
    {
        public void Execute(List<string> extraWords, Character lookingCharacter)
        {
            var lookingCharacterLocation = GameState.CurrentGameState.GetCharacterLocation(lookingCharacter.TrackingId);

            // Display the local description of the location
            lookingCharacter.SendMessage(lookingCharacterLocation?.LocalDescription);
            lookingCharacterLocation.SendMessage($"{lookingCharacter.Name} glances around.", lookingCharacter);

            // Get all portals that have a rule that originates from the current location
            var originPortals = GameState.CurrentGameState.GetPortalsInLocation(lookingCharacterLocation.TrackingId);

            var portalDesinations = originPortals
                .Select(p => p.GetDestination(lookingCharacterLocation.TrackingId));
            //.OrderBy(d => d.Destination); // TODO: Bring sorting back

            lookingCharacter.SendMessage();
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
                    lookingCharacter.SendMessage(portalDest.Description);
                }
                else
                {
                    // If we got here, the description AND the destination exist.
                    var remoteLocation = GameState.CurrentGameState.GetLocation(portalDest.DestinationTrackingId);
                    lookingCharacter.SendMessage($"[{remoteLocation.LocationName}] {portalDest.Description} {remoteLocation.RemoteDescription}");
                }
            }

            var locationItems = GameState.CurrentGameState.GetLocationItems(lookingCharacterLocation.TrackingId);
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
                    lookingCharacter.SendMessage();
                    lookingCharacter.SendMessage($"You see {itemDescriptions}.");
                }
            }

            var otherCharactersInLocation = GameState.CurrentGameState.GetCharactersInLocation(lookingCharacterLocation.TrackingId, includePlayer: true);
            otherCharactersInLocation.Remove(lookingCharacter);
            if(otherCharactersInLocation.Count != 0)
            {
                lookingCharacter.SendMessage();
                lookingCharacter.SendMessage("The following other characters are here:");
                foreach(var character in otherCharactersInLocation)
                {
                    lookingCharacter.SendMessage(character.Name);
                }
            }
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("look", StringComparison.OrdinalIgnoreCase);
        }
    }
}