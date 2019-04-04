using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Commands
{
    class AttackCommand : ICommand
    {
        void ICommand.Exceute(EngineInternal engine)
        {
            var playerLoc = GameState.CurrentGameState.CharacterLocations["Player"];

            if (!engine.GameData.TryGetCharacter("Player", out Character playerCharacter))
            {
                // TODO: why can't we get the player char?? weird error
            }
            var otherCharactersInLoc = GameState.CurrentGameState.GetCharactersInLocation(playerLoc);
            var playerToHit = Console.Choose("Who do you want to hit?", otherCharactersInLoc);
            engine.GameData.TryGetCharacter(playerToHit, out Character defendingCharacter);
            defendingCharacter.Attack(playerCharacter);
        }

        bool ICommand.IsActivatedBy(string word)
        {
            var activators = new List<string>() { "attack", "hit", "fight", "injure" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }

    }
}
