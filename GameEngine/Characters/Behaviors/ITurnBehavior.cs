namespace GameEngine.Characters.Behaviors
{
    public interface ITurnBehavior
    {
        bool HasPromps { get; }

        void Turn(Character turnTakingCharacter);
    }
}
