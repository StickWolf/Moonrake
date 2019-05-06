using GameEngine;
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

        public override void Interact(Item otherItem, Guid interactingCharacterTrackingId)
        {
            if (otherItem == null)
            {
                GameEngine.Console.WriteLine("The keyhole looks like it needs a key.");
                return;
            }

            if (otherItem.TrackingId == RequiredKeyItemTrackingId)
            {
                GameEngine.Console.WriteLine("The door unlocks and swings open!");
                GameState.CurrentGameState.SetGameVarValue(GameVarDoorIsOpen, "true");
                IsVisible = false;
                return;
            }

            GameEngine.Console.WriteLine("This item doesn't seem to work with the keyhole.");
        }
    }
}
