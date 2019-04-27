using GameEngine.Characters;
using GameEngine.Locations;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class MoveCommand : ICommand
    {
        public void Exceute(EngineInternal engine, List<string> extraWords)
        {
            var playerLoc = GameState.CurrentGameState.GetCharacterLocation(PlayerCharacter.TrackingName);
            var originPortalsDestinations = engine.GameData.Portals
                .Where(p => p.HasOriginLocation(playerLoc))
                .Select(p => p.GetDestination(playerLoc))
                .Where(d => d.Destination != null)
                .Select(d =>  d.Destination)
                .ToList();

            var wordLocationMap = CommandHelper.WordsToLocations(extraWords, originPortalsDestinations.ToList(), engine);
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
                location = engine.GameData.GetLocation(chosenLocationName);
            }

            GameState.CurrentGameState.SetCharacterLocation(PlayerCharacter.TrackingName, location.Name);

            // Make the player automatically look after they move to the new location
            Console.WriteLine();
            var lookCommand = CommandHelper.GetCommand("look");
            lookCommand.Exceute(engine, new List<string>());
        }

        public bool IsActivatedBy(string word)
        {
            var validWords = new List<string>() { "go", "move", "travel" };
            return validWords.Contains(word.ToLower());
        }
    }
}
