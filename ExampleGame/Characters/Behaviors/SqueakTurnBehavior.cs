using GameEngine.Characters;
using GameEngine.Characters.Behaviors;

namespace ExampleGame.Characters.Behaviors
{
    public class SqueakTurnBehavior : ITurnBehavior
    {
        public void Turn(Character turnTakingCharacter)
        {
            turnTakingCharacter.GetLocation().SendMessage($"{turnTakingCharacter.Name} squeaks.", turnTakingCharacter);
        }
    }
}
