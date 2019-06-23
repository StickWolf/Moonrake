using ServerEngine;
using ServerEngine.Characters;
using ServerEngine.Characters.Behaviors;
using ServerEngine.GrainSiloAndClient;
using System;

namespace DragonKittyServer.DragonKittyStrangeCharacters.StrangeBehaviors
{
    public class TurnBehaviorRandomHeal : ITurnBehavior
    {
        private static Random rnd = new Random();

        public void Turn(Character turnTakingCharacter)
        {
            var characterList = GrainClusterClient.Universe.GetCharactersInLocation(turnTakingCharacter.GetLocation().TrackingId).Result;
            int characterAmount = characterList.Count;
            int characterToHealIndex = rnd.Next(0, characterAmount);
            var characterToHeal = characterList[characterToHealIndex];
            turnTakingCharacter.Heal(characterToHeal);
        }
    }
}
