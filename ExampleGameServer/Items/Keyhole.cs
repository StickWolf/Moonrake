using ServerEngine;
using ServerEngine.Characters;
using Newtonsoft.Json;
using System;
using ServerEngine.GrainSiloAndClient;

namespace ExampleGameServer.Items
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
            var doorIsOpen = GrainClusterClient.Universe.GetGameVarValue(GameVarDoorIsOpen).Result;
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
                interactingCharacter.SendDescriptiveTextDtoMessage("The keyhole looks like it needs a key.");
                interactingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{interactingCharacter.Name} is examining the keyhole.", interactingCharacter);
                return;
            }

            if (otherItem.TrackingId == RequiredKeyItemTrackingId)
            {
                interactingCharacter.SendDescriptiveTextDtoMessage("The door unlocks and swings open!");
                interactingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{interactingCharacter.Name} has unlocked the door!", interactingCharacter);
                GrainClusterClient.Universe.SetGameVarValue(GameVarDoorIsOpen, "true").Wait();
                IsVisible = false;
                return;
            }

            interactingCharacter.SendDescriptiveTextDtoMessage("This item doesn't seem to work with the keyhole.");
            interactingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{interactingCharacter.Name} is examining the keyhole.", interactingCharacter);
        }
    }
}
