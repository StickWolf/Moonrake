using ServerEngine;
using ServerEngine.Characters;
using Newtonsoft.Json;
using System;

namespace ExampleGameServer.Items
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class HealingPotion : Item
    {
        [JsonProperty]
        private int Charges { get; set; }

        [JsonProperty]
        private int HealPerCharge { get; set; } = 15;

        public HealingPotion(int charges) : base("Healing Potion")
        {
            Charges = charges;
            IsUnique = false;
            IsUseableFrom = ItemUseableFrom.Inventory;
            IsInteractionPrimary = true;
        }

        public override string GetDescription(int count)
        {
            if (count == 1)
            {
                return $"a {DisplayName} with {Charges} use(s) left.";
            }

            return $"{count} {DisplayName}s with {Charges} use(s) left.";
        }

        public override void Interact(Item otherItem, Character interactingCharacter)
        {
            if (Charges == 0)
            {
                interactingCharacter.SendMessage($"The potion bottle is empty!");
                return;
            }

            bool removeResult = GameState.CurrentGameState.TryAddCharacterItemCount(interactingCharacter.TrackingId, this.TrackingId, -1);
            if (!removeResult)
            {
                interactingCharacter.SendMessage($"You don't appear to be holding a potion.");
                return;

            }
            GameState.CurrentGameState.ConvertItemToClone(this.TrackingId);
            GameState.CurrentGameState.TryAddCharacterItemCount(interactingCharacter.TrackingId, this.TrackingId, 1);

            interactingCharacter.Heal(HealPerCharge);
            Charges--;
            interactingCharacter.SendMessage($"You feel refreshed. The potion has {Charges} uses left.");
            interactingCharacter.GetLocation().SendMessage($"{interactingCharacter.Name} takes a swig of a healing potion and looks refreshed.", interactingCharacter);

            GameState.CurrentGameState.DedupeItems();
        }
    }
}
