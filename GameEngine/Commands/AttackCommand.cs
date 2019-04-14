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
            var playerLoc = GameState.CurrentGameState.GetCharacterLocation("Player");

            if (!engine.GameData.TryGetCharacter("Player", out Character playerCharacter))
            {
                // TODO: why can't we get the player char?? weird error
            }
            var otherCharactersInLoc = GameState.CurrentGameState.GetCharactersInLocation(playerLoc);
            otherCharactersInLoc.Add("Cancel");
            var playerToHit = Console.Choose("Who do you want to hit?", otherCharactersInLoc);
            if (playerToHit.Equals("Cancel"))
            {
                Console.WriteLine("Stopped Attack.");
                return;
            }
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
