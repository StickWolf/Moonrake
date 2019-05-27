using ServerEngine.Characters;
using ServerEngine.Locations;
using System.Collections.Generic;
using System.Linq;

namespace ServerEngine.Commands.GameCommands
{
    public class MoveCommand : IGameCommand
    {
        public List<string> ActivatingWords => new List<string>() { "go", "move", "travel" };

        public string PermissionNeeded => null;

        public void Execute(List<string> extraWords, Character movingCharacter)
        {
            var movingCharacterStartLocation = GameState.CurrentGameState.GetCharacterLocation(movingCharacter.TrackingId);

            // Get a list of locations that can be moved to from here.
            var validLocations = GameState.CurrentGameState.GetConnectedLocations(movingCharacterStartLocation.TrackingId)
                .ToDictionary(kvp => kvp, kvp => kvp.LocationName); // convert to Dictionary[{location}] = {locationName}

            var wordLocationMap = WordTranslator.WordsToLocations(extraWords, validLocations.Keys.ToList());
            var foundLocations = wordLocationMap
                .Where(i => i.Value != null)
                .Select(i => i.Value)
                .ToList();

            if (foundLocations.Count > 0)
            {
                Location location = foundLocations[0];
                movingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{movingCharacter.Name} moves to the {location.LocationName}.", movingCharacter);
                GameState.CurrentGameState.SetCharacterLocation(movingCharacter.TrackingId, location.TrackingId);
                movingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{movingCharacter.Name} has arrived in the area.", movingCharacter);

                // Make the player automatically look after they move to the new location
                CommandRunner.TryRunCommandFromCharacter("look", new List<string>(), movingCharacter);
            }
            else
            {
                movingCharacter.SendDescriptiveTextDtoMessage($"Unknown location.");
            }
        }
    }
}
