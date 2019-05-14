using ServerEngine.Characters;
using System.Collections.Generic;
using System.Linq;

namespace ServerEngine.Commands.Public
{
    public class AttackCommand : ICommand
    {
        public List<string> ActivatingWords => new List<string>() { "attack", "hit", "fight", "injure" };

        public void Execute(List<string> extraWords, Character attackingCharacter)
        {
            var attackingCharacterLocation = GameState.CurrentGameState.GetCharacterLocation(attackingCharacter.TrackingId);
            var otherCharactersInLoc = GameState.CurrentGameState.GetCharactersInLocation(attackingCharacterLocation.TrackingId)
                .Where (c => c.TrackingId != attackingCharacter.TrackingId) // don't include the character doing the attacking
                .Select(c => new KeyValuePair<Character, string>(c, c.Name))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            if (otherCharactersInLoc.Count == 0)
            {
                attackingCharacter.SendMessage("There are no characters to attack here.");
                attackingCharacter.GetLocation().SendMessage($"{attackingCharacter.Name} is looking around for someone to attack!", attackingCharacter);
                return;
            }

            var wordCharacterMap = PublicCommandHelper.WordsToCharacters(extraWords, otherCharactersInLoc.Keys.ToList());
            var foundCharacters = wordCharacterMap
                .Where(i => i.Value != null)
                .Select(i => i.Value)
                .ToList();
            Character defendingCharacter;
            if (foundCharacters.Count > 0)
            {
                defendingCharacter = foundCharacters[0];
            }
            // Don't prompt NPCs who are running actions
            else if (!attackingCharacter.HasPromptingBehaviors())
            {
                return;
            }
            else
            {
                defendingCharacter = attackingCharacter.Choose("Who do you want to hit?", otherCharactersInLoc, includeCancel: true );
                if (defendingCharacter == null)
                {
                    attackingCharacter.SendMessage("Stopped Attack.");
                    attackingCharacter.GetLocation().SendMessage($"{attackingCharacter.Name} is acting dangerously!", attackingCharacter);
                    return;
                }
            }

            defendingCharacter.Attack(attackingCharacter);
        }
    }
}
