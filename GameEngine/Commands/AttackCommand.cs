using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class AttackCommand : ICommand
    {
        public void Exceute(GameSourceData gameData, List<string> extraWords)
        {
            var playerLoc = GameState.CurrentGameState.GetPlayerCharacterLocation();
            var playerCharacter = GameState.CurrentGameState.GetPlayerCharacter();

            var otherCharactersInLoc = GameState.CurrentGameState.GetCharactersInLocation(playerLoc.TrackingId, includePlayer: false)
                .Select(c => new KeyValuePair<Character, string>(c, c.Name))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            if (otherCharactersInLoc.Count == 0)
            {
                Console.WriteLine("There are no charaters to attack here.");
                return;
            }

            var wordCharacterMap = CommandHelper.WordsToCharacters(extraWords, otherCharactersInLoc.Keys.ToList(), gameData);
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
                var cancelChar = new Character("Cancel", 0); // TODO: instead of this hacky adding-ness, instead update the choose method to support a cancel option natively
                otherCharactersInLoc[cancelChar] = "Cancel";
                defendingCharacter = Console.Choose("Who do you want to hit?", otherCharactersInLoc);
                if (defendingCharacter == cancelChar)
                {
                    Console.WriteLine("Stopped Attack.");
                    return;
                }
            }

            defendingCharacter.Attack(playerCharacter, gameData);
        }

        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "attack", "hit", "fight", "injure" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
