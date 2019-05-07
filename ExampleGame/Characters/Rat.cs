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
            attackingCharacter.SendMessage($"{Name}'s whisker count is now {WhiskerCount}");
        }

        public override void Turn()
        {
            this.GetLocation().SendMessage($"{Name} squeaks.", this.TrackingId);
        }
    }
}
