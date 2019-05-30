using ServerEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerEngine.Commands.GameCommands
{
    public class LookCommand : IGameCommand
    {
        public List<string> ActivatingWords => new List<string>() { "look" };

        public string PermissionNeeded => null;

        public void Execute(List<string> extraWords, Character lookingCharacter)
        {
            StringBuilder descriptiveText = new StringBuilder();

            // Display the local description of the location
            descriptiveText.AppendLine(lookingCharacter.GetLocation()?.LocalDescription);
            lookingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{lookingCharacter.Name} glances around.", lookingCharacter);

            // Get all portals that have a rule that originates from the current location
            var originPortals = GameState.CurrentGameState.GetPortalsInLocation(lookingCharacter.GetLocation().TrackingId);

            var portalDesinations = originPortals
                .Select(p => p.GetDestination(lookingCharacter.GetLocation().TrackingId));
            //.OrderBy(d => d.Destination); // TODO: Bring sorting back

            // Loop through all destinations in that location 
            foreach (var portalDest in portalDesinations)
            {
                // Evaluate each rule and choose what should be diplayed 
                if (portalDest.Description == null)
                {
                    // The player sees nothing
                }
                else if (portalDest.DestinationTrackingId == Guid.Empty)
                {
                    // If we get here, the description exists, but the destination does not
                    descriptiveText.AppendLine(portalDest.Description);
                }
                else
                {
                    // If we got here, the description AND the destination exist.
                    var remoteLocation = GameState.CurrentGameState.GetLocation(portalDest.DestinationTrackingId);
                    descriptiveText.AppendLine($"[{remoteLocation.LocationName}] {portalDest.Description} {remoteLocation.RemoteDescription}");
                }
            }

            var locationItems = GameState.CurrentGameState.GetLocationItems(lookingCharacter.GetLocation().TrackingId);
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
                    descriptiveText.AppendLine();
                    descriptiveText.AppendLine($"You see {itemDescriptions}.");
                }
            }

            var otherCharactersInLocation = GameState.CurrentGameState.GetCharactersInLocation(lookingCharacter.GetLocation().TrackingId)
                .Where(c => c.TrackingId != lookingCharacter.TrackingId).ToList();
            if (otherCharactersInLocation.Count != 0)
            {
                string otherCharactersMessage = "";
                for (int i = 0; i < otherCharactersInLocation.Count; i++)
                {
                    var character = otherCharactersInLocation[i];
                    if (i > 0)
                    {
                        otherCharactersMessage += (i == otherCharactersInLocation.Count - 1) ? " and " : ", ";
                    }

                    otherCharactersMessage += character.IsDead() ? $"{character.Name} (dead)" : character.Name;
                }
                otherCharactersMessage += otherCharactersInLocation.Count > 1 ? " are here." : " is here.";
                descriptiveText.AppendLine();
                descriptiveText.AppendLine(otherCharactersMessage);
            }

            lookingCharacter.SendDescriptiveTextDtoMessage(descriptiveText.ToString());
        }
    }
}