using ExampleGame.Characters.Behaviors;
using GameEngine.Characters;
using GameEngine.Characters.Behaviors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ExampleGame.Characters
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Rat : Character
    {
        [JsonProperty]
        private int WhiskerCount { get; set; }

        private Random rnd = new Random();

        public Rat(string name, int hp, int whiskerCount) : base(name, hp)
        {
            WhiskerCount = whiskerCount;
            TurnBehaviors = new List<string>()
            {
                CustomTurnBehavior.Squeak,
                BuiltInTurnBehaviors.Random
            };
        }

        public override void Attack(Character attackingCharacter)
        {
            WhiskerCount--;
            attackingCharacter.SendMessage($"{Name}'s whisker count is now {WhiskerCount}");
        }
    }
}
