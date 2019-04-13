using ExampleGame.Items;
using GameEngine;

namespace ExampleGame
{
    public class ExampleItems
    {
        public string CrystalDiviner { get; private set; }
        public string DullBronzeKey { get; private set; }
        public string RedGreenLight { get; private set; }

        public ExampleItems(ExampleGameSourceData gameData)
        {
            CrystalDiviner = gameData.AddItem(new CrystalDiviner());
            DullBronzeKey = gameData.AddItem(new Item("Dull Bronze Key") { IsUnique = true });
            RedGreenLight = gameData.AddItem(new ItemRedGreenLight());
        }
    }
}
