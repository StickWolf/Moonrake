using ServerEngine.Characters;
using ServerEngine.GrainSiloAndClient;
using System.Collections.Generic;
using System.Linq;

namespace ServerEngine.Commands.GameCommands
{
    public class AttackCommand : IGameCommand
    {
        public List<string> ActivatingWords => new List<string>() { "attack", "hit", "fight", "injure" };

        public string PermissionNeeded => null;

        public void Execute(List<string> extraWords, Character attackingCharacter)
        {
            var attackingCharacterLocation = GrainClusterClient.Universe.GetCharacterLocation(attackingCharacter.TrackingId).Result;
            var otherCharactersInLoc = GrainClusterClient.Universe.GetCharactersInLocation(attackingCharacterLocation.TrackingId).Result
                .Where(c => c.TrackingId != attackingCharacter.TrackingId) // don't include the character doing the attacking
                .Select(c => new KeyValuePair<Character, string>(c, c.Name))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            if (otherCharactersInLoc.Count == 0)
            {
                attackingCharacter.SendDescriptiveTextDtoMessage("There are no characters to attack here.");
                attackingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{attackingCharacter.Name} is looking around for someone to attack!", attackingCharacter);
                return;
            }

            var wordCharacterMap = WordTranslator.WordsToCharacters(extraWords, otherCharactersInLoc.Keys.ToList());
            var foundCharacters = wordCharacterMap
                .Where(i => i.Value != null)
                .Select(i => i.Value)
                .ToList();
            Character defendingCharacter;
            if (foundCharacters.Count == 0)
            {
                attackingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{attackingCharacter.Name} is acting dangerously!", attackingCharacter);
                return;
            }
            defendingCharacter = foundCharacters[0];
            defendingCharacter.Attack(attackingCharacter);
        }
    }
}
