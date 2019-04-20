using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class AttackCommand : ICommand
    {
        public void Exceute(EngineInternal engine, List<string> extraWords)
        {
            var playerLoc = GameState.CurrentGameState.GetCharacterLocation("Player");

            if (!engine.GameData.TryGetCharacter("Player", out Character playerCharacter))
            {
                // TODO: why can't we get the player char?? weird error
            }
            var otherCharactersInLoc = GameState.CurrentGameState.GetCharactersInLocation(playerLoc);
            if (otherCharactersInLoc.Count == 0)
            {
                Console.WriteLine("There is no charaters to attack");
                return;
            }
            if(otherCharactersInLoc.Count != 0)
            {
                otherCharactersInLoc.Add("Cancel");
            }
            var playerToHit = Console.Choose("Who do you want to hit?", otherCharactersInLoc);
            if (playerToHit.Equals("Cancel"))
            {
                Console.WriteLine("Stopped Attack.");
                return;
            }
            engine.GameData.TryGetCharacter(playerToHit, out Character defendingCharacter);
            defendingCharacter.Attack(playerCharacter);
        }

        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "attack", "hit", "fight", "injure" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
