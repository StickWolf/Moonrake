using ExampleGame.Items;
using GameEngine;

namespace ExampleGame
{
    public class ExampleItems
    {
        public string DullBronzeKey { get; private set; }

        public string RedGreenLight { get; private set; }

        public ExampleItems(ExampleGameSourceData gameData)
        {
            DullBronzeKey = gameData.AddItem(new Item("Dull Bronze Key") { IsUnique = true });

            RedGreenLight = gameData.AddItem(new ItemRedGreenLight());
        }
    }
}
