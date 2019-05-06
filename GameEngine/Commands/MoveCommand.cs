using GameEngine.Characters;
using GameEngine.Locations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class MoveCommand : ICommand
    {
        public void Execute(List<string> extraWords, Guid movingCharacterTrackingId)
        {
            var movingCharacter = GameState.CurrentGameState.GetCharacter(movingCharacterTrackingId);
            var movingCharacterLocation = GameState.CurrentGameState.GetCharacterLocation(movingCharacterTrackingId);

            // Get a list of locations that can be moved to from here.
            var validLocations = GameState.CurrentGameState.GetConnectedLocations(movingCharacterLocation.TrackingId)
                .ToDictionary(kvp => kvp, kvp => kvp.LocationName); // convert to Dictionary[{location}] = {locationName}

            var wordLocationMap = CommandHelper.WordsToLocations(extraWords, validLocations.Keys.ToList());
            var foundLocations = wordLocationMap
                .Where(i => i.Value != null)
                .Select(i => i.Value)
                .ToList();

            Location location;
            if (foundLocations.Count > 0)
            {
                location = foundLocations[0];
            }
            else
            {
                location = Console.Choose("Where would you like to move to?", validLocations, includeCancel: true);
                if (location == null)
                {
                    movingCharacter.SendMessage("Canceled Move");
                    movingCharacterLocation.SendMessage($"{movingCharacter.Name} looks indecisive.", movingCharacter.TrackingId);
                    return;
                }
            }

            GameState.CurrentGameState.SetCharacterLocation(movingCharacter.TrackingId, location.TrackingId);

            // Make the player automatically look after they move to the new location
            movingCharacter.SendMessage();
            CommandHelper.TryRunPublicCommand("look", new List<string>(), movingCharacterTrackingId);
        }

        public bool IsActivatedBy(string word)
        {
            var validWords = new List<string>() { "go", "move", "travel" };
            return validWords.Contains(word.ToLower());
        }
    }
}
