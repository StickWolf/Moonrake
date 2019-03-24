using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class MoveCommand : ICommand
    {
        public void Exceute(EngineInternal engine)
        {
            var playerLoc = GameState.CurrentGameState.CharacterLocations["Player"];
            var originPortalsDestinations = engine.GameData.Portals
                .Where(p => p.HasOriginLocation(playerLoc))
                .Select(p => p.GetDestination(playerLoc))
                .Where(d => d.Destination != null)
                .Select(d =>  d.Destination)
                .ToList();

            var placeToMoveTo = Console.Choose("Where would you like to move to?", originPortalsDestinations);
            GameState.CurrentGameState.CharacterLocations["Player"] = placeToMoveTo;
        }

        public bool IsActivatedBy(string word)
        {
            var validWords = new List<string>() { "go", "move", "travel" };
            return validWords.Contains(word.ToLower());
        }
    }
}
