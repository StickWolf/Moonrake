using GameEngine;
using System;

namespace ExampleGame.Items
{
    public class Keyhole : Item
    {
        private string GameVarDoorIsOpen { get; set; }
        private string RequiredKeyItemName { get; set; }

        public Keyhole(string gameVarDoorIsOpen, string requiredKeyItemName) : base($"Keyhole[{gameVarDoorIsOpen}]", "Keyhole")
        {
            RequiredKeyItemName = requiredKeyItemName;
            GameVarDoorIsOpen = gameVarDoorIsOpen;
            IsUnique = true;
            IsBound = true;
            IsInteractable = true;
        }

        public override string GetDescription(int count, GameState gameState)
        {
            var doorIsOpen = gameState.GetGameVarValue(GameVarDoorIsOpen);
            if (!doorIsOpen.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                return "a keyhole in the locked door";
            }

            return null;
        }

        public override void Interact(GameState gameState, string otherItemTrackingName)
        {
            if (otherItemTrackingName == null)
            {
                GameEngine.Console.WriteLine("The keyhole looks like it needs a key.");
                return;
            }

            if (otherItemTrackingName.Equals(RequiredKeyItemName))
            {
                GameEngine.Console.WriteLine("The door unlocks and swings open!");
                gameState.SetGameVarValue(GameVarDoorIsOpen, "true");
                IsVisible = false;
                return;
            }

            GameEngine.Console.WriteLine("This item doesn't seem to work with the keyhole.");
        }
    }
}
