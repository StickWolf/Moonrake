using GameEngine.Characters;
using GameEngine.Locations;
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
            var originPortalsDestinations = gameData.Portals
                .Where(p => p.HasOriginLocation(movingCharacterLoc))
                .Select(p => p.GetDestination(movingCharacterLoc))
                .Where(d => d.Destination != null)
                .Select(d =>  d.Destination)
                .ToList();

            var wordLocationMap = CommandHelper.WordsToLocations(extraWords, originPortalsDestinations.ToList(), gameData);
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
                originPortalsDestinations.Add("Cancel");
                var chosenLocationName = Console.Choose("Where would you like to move to?", originPortalsDestinations);
                if (chosenLocationName.Equals("Cancel"))
                {
                    Console.WriteLine("Canceled Move");
                    return;
                }
                location = gameData.GetLocation(chosenLocationName);
            }

            GameState.CurrentGameState.SetCharacterLocation(movingCharacter.TrackingId, location.Name);

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
