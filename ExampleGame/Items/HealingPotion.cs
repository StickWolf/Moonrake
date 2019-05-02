using GameEngine;
using Newtonsoft.Json;
using System;

namespace ExampleGame.Items
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
            IsInteractable = true;
        }

        public override string GetDescription(int count)
        {
            if (count == 1)
            {
                return $"a {DisplayName} with {Charges} use(s) left.";
            }

            return $"{count} {DisplayName}s with {Charges} use(s) left.";
        }

        public override void Interact(Item otherItem)
        {
            // TODO: rewrite commands that are intended to be used by NPCs to work for them too
            // TODO: the way this is written, even if a NPC used it, it would heal the player
            var healee = GameState.CurrentGameState.GetPlayerCharacter();

            // TODO: be able to write code like this, so we can check to make sure this item in in the characters inventory
            // TODO: but maybe the below removeResult is good enough
            ///if (healee.IsHoldingItem(this)) ...

            if (Charges == 0)
            {
                GameEngine.Console.WriteLine($"The potion bottle is empty!");
                return;
            }

            bool removeResult = GameState.CurrentGameState.TryAddCharacterItemCount(healee.TrackingId, this.TrackingId, -1);
            if (!removeResult)
            {
                GameEngine.Console.WriteLine($"You don't appear to be holding a potion.");
                return;

            }
            GameState.CurrentGameState.ConvertItemToClone(this.TrackingId);
            GameState.CurrentGameState.TryAddCharacterItemCount(healee.TrackingId, this.TrackingId, 1);

            healee.Heal(HealPerCharge);
            Charges--;
            GameEngine.Console.WriteLine($"You feel refreshed. The potion has {Charges} uses left.");

            GameState.CurrentGameState.DedupeItems();
        }
    }
}
