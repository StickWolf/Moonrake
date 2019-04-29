using GameEngine.Characters;
using GameEngine.Locations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class MoveCommand : ICommand
    {
        public void Exceute(GameSourceData gameData, List<string> extraWords)
        {
            var movingCharacter = GameState.CurrentGameState.GetPlayerCharacter();
            var movingCharacterLoc = GameState.CurrentGameState.GetCharacterLocation(movingCharacter.TrackingId);

            // Get a list of locations that can be moved to from here.
            var validLocations = gameData.Portals
                .Where(p => p.HasOriginLocation(movingCharacterLoc.TrackingId)) // Portals in this location
                .Select(p => p.GetDestination(movingCharacterLoc.TrackingId)) // Get destination info for each of the portals
                .Where(d => d.DestinationTrackingId != Guid.Empty) // Exclude destinations that lead nowhere (locked doors, etc)
                .Select(d => GameState.CurrentGameState.GetLocation(d.DestinationTrackingId)) // Get the actual destination location
                .ToDictionary(kvp => kvp, kvp => kvp.Name); // convert to Dictionary[{location}] = {locationName}

            var wordLocationMap = CommandHelper.WordsToLocations(extraWords, validLocations.Keys.ToList(), gameData);
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
                var cancelLocation = new Location("Cancel", "Cancel", "Cancel"); // TODO: fix this weirdness
                validLocations[cancelLocation] = "Cancel";
                location = Console.Choose("Where would you like to move to?", validLocations);
                if (location == cancelLocation)
                {
                    Console.WriteLine("Canceled Move");
                    return;
                }
            }

            GameState.CurrentGameState.SetCharacterLocation(movingCharacter.TrackingId, location.TrackingId);

            // Make the player automatically look after they move to the new location
            Console.WriteLine();
            CommandHelper.TryRunPublicCommand("look", new List<string>(), gameData);
        }

        public bool IsActivatedBy(string word)
        {
            var validWords = new List<string>() { "go", "move", "travel" };
            return validWords.Contains(word.ToLower());
        }
    }
}
