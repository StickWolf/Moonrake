using GameEngine;

namespace ExampleGame.Items
{
    public class ColoredLight : Item
    {
        private string GameVariableColor { get; set; }

        public ColoredLight(string gameVariableColor) : base($"ColoredLight[{gameVariableColor}]", "Colored light")
        {
            GameVariableColor = gameVariableColor;
            IsUnique = false;
            IsBound = true;
        }

        public override string GetDescription(int count, GameState gameState)
        {
            string lightColor = gameState.GetGameVarValue(GameVariableColor) ?? "blue";
            return $"a light neatly fastened to the wall, covered in metal mesh that glows bright {lightColor}";
        }
    }
}
