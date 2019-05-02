using GameEngine;
using GameEngine.Characters;
using GameEngine.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamsAndWhatTheyMean.DragonKittyStrangeCharacters
{
    class HealingDrone : Character
    {
        public Location MainLocation { get; set; }
        private static Random rnd = new Random();

        public HealingDrone(Guid mainLocationOfDrone) : base("Drone", 20)
        {
            CounterAttackChance = 10;
            MaxAttack = 10;
            MaxHeal = 500;
        }

        public override void Turn()
        {
            var characterList = GameState.CurrentGameState.GetCharactersInLocation(MainLocation.TrackingId, true);
            int characterAmount = characterList.Count;
            int characterToHealIndex = rnd.Next(0, characterAmount);
            var characterToHeal = characterList[characterToHealIndex];
            Heal(characterToHeal);
        }


    }
}
