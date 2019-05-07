using GameEngine;
using GameEngine.Characters;
using Newtonsoft.Json;
using System;

namespace ExampleGame.Items
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Keyhole : Item
    {
        [JsonProperty]
        private string GameVarDoorIsOpen { get; set; }

        [JsonProperty]
        private Guid RequiredKeyItemTrackingId { get; set; }

        public Keyhole(string gameVarDoorIsOpen, Guid requiredKeyItemTrackingId) : base("Keyhole")
        {
            RequiredKeyItemTrackingId = requiredKeyItemTrackingId;
            GameVarDoorIsOpen = gameVarDoorIsOpen;
            IsUnique = true;
            IsBound = true;
            IsUseableFrom = ItemUseableFrom.Location;
            IsInteractionPrimary = true;
        }

        public override string GetDescription(int count)
        {
            var doorIsOpen = GameState.CurrentGameState.GetGameVarValue(GameVarDoorIsOpen);
            if (!doorIsOpen.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                return "a keyhole in the locked door";
            }

            return null;
        }

        public override void Interact(Item otherItem, Character interactingCharacter)
        {
            if (otherItem == null)
            {
                interactingCharacter.SendMessage("The keyhole looks like it needs a key.");
                interactingCharacter.GetLocation().SendMessage($"{interactingCharacter.Name} is examining the keyhole.", interactingCharacter);
                return;
            }

            if (otherItem.TrackingId == RequiredKeyItemTrackingId)
            {
                interactingCharacter.SendMessage("The door unlocks and swings open!");
                interactingCharacter.GetLocation().SendMessage($"{interactingCharacter.Name} has unlocked the door!", interactingCharacter);
                GameState.CurrentGameState.SetGameVarValue(GameVarDoorIsOpen, "true");
                IsVisible = false;
                return;
            }

            interactingCharacter.SendMessage("This item doesn't seem to work with the keyhole.");
            interactingCharacter.GetLocation().SendMessage($"{interactingCharacter.Name} is examining the keyhole.", interactingCharacter);
        }
    }
}
