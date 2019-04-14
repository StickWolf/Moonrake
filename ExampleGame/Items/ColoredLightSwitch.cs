using GameEngine;

namespace ExampleGame.Items
{
    public class ColoredLightSwitch : Item
    {
        private string GameVariableColor { get; set; }

        public ColoredLightSwitch(string gameVariableColor) : base($"ColoredLightSwitch[{gameVariableColor}]", "Colored light switch")
        {
            GameVariableColor = gameVariableColor;
            IsUnique = false;
            IsBound = true;
            IsInteractable = true;
        }

        public override string GetDescription(int count, GameState gameState)
        {
            return $"a light switch";
        }

        public override void Interact(GameState gameState, string otherItemTrackingName)
        {
            string lightColor = gameState.GetGameVarValue(GameVariableColor);
            if (lightColor == null)
            {
                Console.WriteLine($"You flip the light switch, but nothing appears to happen.");
            }
            else
            {
                var newColor = "blue";
                switch (lightColor)
                {
                    case "red":
                        newColor = "green";
                        break;
                    case "green":
                        newColor = "red";
                        break;
                    case "dark purple":
                        newColor = "teal";
                        break;
                    case "teal":
                        newColor = "dark purple";
                        break;
                }
                gameState.SetGameVarValue(GameVariableColor, newColor);
                Console.WriteLine($"You flip the light switch and the {lightColor} light now begins to glow {newColor}.");
            }
        }
    }
}
