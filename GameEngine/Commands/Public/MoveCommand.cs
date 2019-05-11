using GameEngine.Characters;
using GameEngine.Locations;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands.Public
{
    public class MoveCommand : ICommand
    {
        public List<string> ActivatingWords => new List<string>() { "go", "move", "travel" };

        public void Execute(List<string> extraWords, Character movingCharacter)
        {
            var movingCharacterStartLocation = GameState.CurrentGameState.GetCharacterLocation(movingCharacter.TrackingId);

            // Get a list of locations that can be moved to from here.
            var validLocations = GameState.CurrentGameState.GetConnectedLocations(movingCharacterStartLocation.TrackingId)
                .ToDictionary(kvp => kvp, kvp => kvp.LocationName); // convert to Dictionary[{location}] = {locationName}

            var wordLocationMap = PublicCommandHelper.WordsToLocations(extraWords, validLocations.Keys.ToList());
            var foundLocations = wordLocationMap
                .Where(i => i.Value != null)
                .Select(i => i.Value)
                .ToList();

            Location location;
            if (foundLocations.Count > 0)
            {
                location = foundLocations[0];
            }
            // Don't prompt NPCs who are running actions
            else if (!movingCharacter.IsPlayerCharacter())
            {
                return;
            }
            else
            {
                location = movingCharacter.Choose("Where would you like to move to?", validLocations, includeCancel: true);
                if (location == null)
                {
                    movingCharacter.SendMessage("Canceled Move");
                    movingCharacter.GetLocation().SendMessage($"{movingCharacter.Name} looks indecisive.", movingCharacter);
                    return;
                }
            }

            movingCharacter.GetLocation().SendMessage($"{movingCharacter.Name} moves to the area [{location.LocationName}].", movingCharacter);
            GameState.CurrentGameState.SetCharacterLocation(movingCharacter.TrackingId, location.TrackingId);
            movingCharacter.GetLocation().SendMessage($"{movingCharacter.Name} has arrived in the area.", movingCharacter);

            // Make the player automatically look after they move to the new location
            movingCharacter.SendMessage();
            PublicCommandHelper.TryRunPublicCommand("look", new List<string>(), movingCharacter);
        }
    }
}
