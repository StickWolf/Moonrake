using GameEngine;
using System;

namespace ExampleGame.Items
{
    public class Lever : Item
    {
        private string GameVariableToggle { get; set; }

        public Action<GameState,string> CustomInteract { get; set; }

        public Lever(string gameVariableToggle) : base($"Lever[{gameVariableToggle}]", "Lever")
        {
            GameVariableToggle = gameVariableToggle;
            IsUnique = false;
            IsBound = true;
            IsInteractable = true;
        }

        public override string GetDescription(int count, GameState gameState)
        {
            return $"a lever";
        }

        public override void Interact(GameState gameState, string otherItemTrackingName)
        {
            string leverPosition = gameState.GetGameVarValue(GameVariableToggle);
            if (leverPosition == null)
            {
                GameEngine.Console.WriteLine($"The lever appears to be broken and cannot be moved.");
            }
            else
            {
                if (CustomInteract != null)
                {
                    CustomInteract(gameState, leverPosition);
                }
                else
                {
                    GameEngine.Console.WriteLine($"You move the lever.");
                    gameState.SetGameVarValue(GameVariableToggle, leverPosition.Equals("on") ? "off" : "on");
                }
            }
        }
    }
}
