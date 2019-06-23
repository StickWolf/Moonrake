using ServerEngine;
using ServerEngine.Characters;
using ServerEngine.GrainSiloAndClient;

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
                interactingCharacter.SendDescriptiveTextDtoMessage("You can't eat the apple, you are at full health.");
                return;
            }
            int healAmount = 20;
            if (interactingCharacter.HitPoints + healAmount > interactingCharacter.MaxHitPoints)
            {
                healAmount = interactingCharacter.MaxHitPoints - interactingCharacter.HitPoints;
            }

            interactingCharacter.HitPoints += healAmount;
            interactingCharacter.SendDescriptiveTextDtoMessage("You eat an apple, and you feel some of your health come back.");
            interactingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{interactingCharacter.Name} eats a delicious looking apple.", interactingCharacter);
            GrainClusterClient.Universe.TryAddCharacterItemCount(interactingCharacter.TrackingId, this.TrackingId, -1).Wait();
        }
    }
}
