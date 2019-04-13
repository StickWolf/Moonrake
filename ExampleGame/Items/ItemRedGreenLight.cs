using GameEngine;

namespace ExampleGame.Items
{
    public class ItemRedGreenLight : Item
    {
        public ItemRedGreenLight() : base("Colored light")
        {
            // TODO: This is not a unique items, however not that in the description all instances of the redgreen light
            // TODO: are controlled by the same GameVariable. This needs to be resolved so that each individual light
            // TODO: can be controlled by different game variables.
            IsUnique = false;
            IsBound = true;
        }

        public override string GetDescription(int count, GameSourceData gameData, GameState gameState)
        {
            // We know that the game data coming in is for our game, so "cast" it to the right type.
            // Without this we won't be able to access the properties we added just in this project
            var exampleGameData = gameData as ExampleGameSourceData;

            // TODO: As a temporary demonstration I'm flipping the colors here on each call, but really what
            // TODO: I'd like to do instead is to have a switch in the same room that changes the colors like this:
            // TODO: For now you can just type look several times in a row to see the effect.
            string lightColor = null;
            switch (gameState.GameVars[exampleGameData.GameVariables.RedGreenLightColor])
            {
                case "red":
                    lightColor = gameState.GameVars[exampleGameData.GameVariables.RedGreenLightColor] = "green";
                    break;
                case "green":
                    lightColor = gameState.GameVars[exampleGameData.GameVariables.RedGreenLightColor] = "red";
                    break;
            }

            return $"a light neatly fastened to the wall, covered in metal mesh that glows bright {lightColor}";
        }
    }
}
