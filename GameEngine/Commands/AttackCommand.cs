﻿using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class AttackCommand : ICommand
    {
        public void Exceute(GameSourceData gameData, List<string> extraWords)
        {
            var playerLoc = GameState.CurrentGameState.GetCharacterLocation(PlayerCharacter.TrackingName);

            if (!gameData.TryGetCharacter(PlayerCharacter.TrackingName, out Character playerCharacter))
            {
                // TODO: why can't we get the player char?? weird error
            }
            var otherCharactersInLoc = GameState.CurrentGameState.GetCharactersInLocation(playerLoc);
            if (otherCharactersInLoc.Count == 0)
            {
                Console.WriteLine("There are no charaters to attack here.");
                return;
            }

            var wordCharacterMap = CommandHelper.WordsToCharacters(extraWords, otherCharactersInLoc, gameData);
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
                otherCharactersInLoc.Add("Cancel");
                var playerToHit = Console.Choose("Who do you want to hit?", otherCharactersInLoc);
                if (playerToHit.Equals("Cancel"))
                {
                    Console.WriteLine("Stopped Attack.");
                    return;
                }
                defendingCharacter = gameData.GetCharacter(playerToHit);
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
