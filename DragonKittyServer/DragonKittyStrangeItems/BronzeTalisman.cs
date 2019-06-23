using ServerEngine;
using ServerEngine.Characters;
using ServerEngine.GrainSiloAndClient;

namespace DragonKittyServer.DragonKittyStrangeItems
{
    class BronzeTalisman : Item
    {
        public BronzeTalisman() : base("bronze talisman")
        {
            IsUnique = false;
            IsVisible = true;
            IsUseableFrom = ItemUseableFrom.Inventory;
            IsInteractionPrimary = true;
            IsBound = false;
        }

        public override void Interact(Item otherItem, Character interactingCharacter)
        {
            interactingCharacter.MaxAttack += 20;
            interactingCharacter.MaxHitPoints += 30;
            interactingCharacter.SendDescriptiveTextDtoMessage("You put the talisman on, it vanishes and you feel stronger.");
            interactingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{interactingCharacter.Name} puts on a talisman and looks stronger.", interactingCharacter);
            GrainClusterClient.Universe.TryAddCharacterItemCount(interactingCharacter.TrackingId, this.TrackingId, -1).Wait();
        }
    }
}
