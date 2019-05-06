using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class AttackCommand : ICommand
    {
        public void Exceute(List<string> extraWords)
        {
            // TODO: instead pass in the character that is using this command
            var attackingCharacter = GameState.CurrentGameState.GetPlayerCharacter();
            var playerLoc = GameState.CurrentGameState.GetPlayerCharacterLocation();

            var otherCharactersInLoc = GameState.CurrentGameState.GetCharactersInLocation(playerLoc.TrackingId, includePlayer: false)
                .Select(c => new KeyValuePair<Character, string>(c, c.Name))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            if (otherCharactersInLoc.Count == 0)
            {
                Console.WriteLine("There are no charaters to attack here.");
                return;
            }

            var wordCharacterMap = CommandHelper.WordsToCharacters(extraWords, otherCharactersInLoc.Keys.ToList());
            var foundCharacters = wordCharacterMap
                .Where(i => i.Value != null)
                .Select(i => i.Value)
                .ToList();
            Character defendingCharacter;
            if (foundCharacters.Count > 0)
            {
                defendingCharacter = foundCharacters[0];
            }
            else
            {
                defendingCharacter = Console.Choose("Who do you want to hit?", otherCharactersInLoc, includeCancel: true);
                if (defendingCharacter == null)
                {
                    Console.WriteLine("Stopped Attack.");
                    return;
                }
            }

            defendingCharacter.Attack(attackingCharacter);
        }

        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "attack", "hit", "fight", "injure" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
