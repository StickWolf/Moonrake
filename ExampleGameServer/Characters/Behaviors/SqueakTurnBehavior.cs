using ServerEngine.Characters;
using ServerEngine.Characters.Behaviors;

namespace ExampleGameServer.Characters.Behaviors
{
    public class SqueakTurnBehavior : ITurnBehavior
    {
        public bool HasPromps => false;

        public void Turn(Character turnTakingCharacter)
        {
            turnTakingCharacter.GetLocation().SendMessage($"{turnTakingCharacter.Name} squeaks.", turnTakingCharacter);
        }
    }
}
