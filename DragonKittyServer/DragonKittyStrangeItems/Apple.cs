using ServerEngine;
using ServerEngine.Characters;

namespace DragonKittyServer.DragonKittyStrangeItems
{
    class Apple : Item
    {
        public Apple() : base("apple")
        {
            IsBound = false;
            IsUseableFrom = ItemUseableFrom.Inventory;
            IsUnique = false;
            IsVisible = true;
            IsInteractionPrimary = true;
        }

        public override void Interact(Item otherItem, Character interactingCharacter)
        {
            if (interactingCharacter.HitPoints == interactingCharacter.MaxHitPoints)
            {
                interactingCharacter.SendMessage("You can't eat the apple, you are at full health.");
                return;
            }
            int healAmount = 20;
            if (interactingCharacter.HitPoints + healAmount > interactingCharacter.MaxHitPoints)
            {
                healAmount = interactingCharacter.MaxHitPoints - interactingCharacter.HitPoints;
            }

            interactingCharacter.HitPoints += healAmount;
            interactingCharacter.SendMessage("You eat an apple, and you feel some of your health come back.");
            interactingCharacter.GetLocation().SendMessage($"{interactingCharacter.Name} eats a delicious looking apple.", interactingCharacter);
            GameState.CurrentGameState.TryAddCharacterItemCount(interactingCharacter.TrackingId, this.TrackingId, -1);
        }
    }
}
