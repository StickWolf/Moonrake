using GameEngine;
using GameEngine.Characters;

namespace ExampleGame.Characters
{
    public class Rat : Character
    {
        public int WhiskerCount { get; set; }

        public Rat(string name, int hp, int whiskerCount) : base(name, hp)
        {
            WhiskerCount = whiskerCount;
        }

        public override void Attack(Character attackingCharacter, GameSourceData gameData)
        {
            WhiskerCount--;
            Console.WriteLine($"My whisker count is: {WhiskerCount}");
        }

        public override void Turn(GameSourceData gameData)
        {
            //Console.WriteLine($"{Name} is taking their turn as a rat.");
        }
    }
}
