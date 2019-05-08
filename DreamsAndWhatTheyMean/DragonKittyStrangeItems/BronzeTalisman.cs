using GameEngine;
using GameEngine.Characters;

namespace DreamsAndWhatTheyMean.DragonKittyStrangeItems
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
            interactingCharacter.SendMessage("You put the talisman on, it vanishes and you feel stronger.");
            interactingCharacter.GetLocation().SendMessage($"{interactingCharacter.Name} puts on a talisman and looks stronger.", interactingCharacter);
            GameState.CurrentGameState.TryAddCharacterItemCount(interactingCharacter.TrackingId, this.TrackingId, -1);
        }
    }
}
