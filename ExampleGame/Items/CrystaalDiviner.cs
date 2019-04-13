using GameEngine;

namespace ExampleGame.Items
{
    public class CrystaalDiviner : Item
    {
        public CrystaalDiviner() : base("Crystal Diviner")
        {
            IsUnique = true;
            IsBound = true;
        }

        public override string GetDescription(int count, GameSourceData gameData, GameState gameState)
        {
            // We know that the game data coming in is for our game, so "cast" it to the right type.
            // Without this we won't be able to access the properties we added just in this project
            var exampleGameData = gameData as ExampleGameSourceData;

            return "A device made of crystal that has an unknown purpose.";
        }
    }
}
