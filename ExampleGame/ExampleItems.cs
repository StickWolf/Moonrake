using ExampleGame.Items;
using GameEngine;

namespace ExampleGame
{
    public class ExampleItems
    {
        public string CrystalDiviner { get; private set; }
        public string DullBronzeKey { get; private set; }
        public string ColoredLightA { get; private set; }
        public string ColoredLightB { get; private set; }
        public string ColoredLightSwitchA { get; private set; }
        public string ColoredLightSwitchB { get; private set; }

        public ExampleItems(ExampleGameSourceData gameData)
        {
            CrystalDiviner = gameData.AddItem(new CrystalDiviner());
            DullBronzeKey = gameData.AddItem(new Item("DullBronzeKey", "Dull Bronze Key") { IsUnique = true });
            ColoredLightA = gameData.AddItem(new ColoredLight(gameData.GameVariables.ColoredLightAColor));
            ColoredLightB = gameData.AddItem(new ColoredLight(gameData.GameVariables.ColoredLightBColor));
            ColoredLightSwitchA = gameData.AddItem(new ColoredLightSwitch(gameData.GameVariables.ColoredLightAColor));
            ColoredLightSwitchB = gameData.AddItem(new ColoredLightSwitch(gameData.GameVariables.ColoredLightBColor));
        }
    }
}
