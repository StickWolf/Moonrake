﻿using GameEngine;
using GameEngine.Characters;
using GameEngine.Characters.Behaviors;
using System;

namespace DreamsAndWhatTheyMean.DragonKittyStrangeCharacters.StrangeBehaviors
{
    public class TurnBehaviorRandomHeal : ITurnBehavior
    {
        public bool HasPromps => false;

        private static Random rnd = new Random();

        public void Turn(Character turnTakingCharacter)
        {
            var characterList = GameState.CurrentGameState.GetCharactersInLocation(turnTakingCharacter.GetLocation().TrackingId);
            int characterAmount = characterList.Count;
            int characterToHealIndex = rnd.Next(0, characterAmount);
            var characterToHeal = characterList[characterToHealIndex];
            turnTakingCharacter.Heal(characterToHeal);
        }
    }
}