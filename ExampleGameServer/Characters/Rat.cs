using ExampleGameServer.Characters.Behaviors;
using ServerEngine.Characters;
using ServerEngine.Characters.Behaviors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ExampleGameServer.Characters
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
            TurnCooldown = TimeSpan.FromMinutes(1);
        }

        public override void Attack(Character attackingCharacter)
        {
            WhiskerCount--;
            attackingCharacter.SendDescriptiveTextDtoMessage($"{Name}'s whisker count is now {WhiskerCount}");
        }
    }
}
