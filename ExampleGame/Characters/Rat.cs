using GameEngine;
using GameEngine.Characters;
using Newtonsoft.Json;

namespace ExampleGame.Characters
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Rat : Character
    {
        [JsonProperty]
        private int WhiskerCount { get; set; }

        public Rat(string name, int hp, int whiskerCount) : base(name, hp)
        {
            WhiskerCount = whiskerCount;
        }

        public override void Attack(Character attackingCharacter)
        {
            WhiskerCount--;
            Console.CharacterLocationWriteLine($"{Name}'s whisker count is now {WhiskerCount}", this.TrackingId);
        }

        public override void Turn()
        {
            Console.CharacterLocationWriteLine($"{Name} squeaks.", this.TrackingId);
        }
    }
}
