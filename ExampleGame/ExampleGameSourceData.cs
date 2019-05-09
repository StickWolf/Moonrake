using GameEngine;

namespace ExampleGame
{
    /// <summary>
    /// This class gives an example of how to implement all the features available to the game data
    /// </summary>
    public class ExampleGameSourceData
    {
        public ExampleGameVariables GameVariables { get; private set; } = new ExampleGameVariables();
        public ExampleLocations EgLocations { get; private set; } = new ExampleLocations();
        public ExampleCharacters EgCharacters { get; private set; } = new ExampleCharacters();
        public ExampleItems EgItems { get; private set; } = new ExampleItems();

        public void NewGame()
        {
            // Note that the properties should be defined in this order due to how they reference each other
            GameVariables.NewGame(this);
            EgLocations.NewGame(this);
            EgCharacters.NewGame(this);
            EgItems.NewGame(this);

            GameState.CurrentGameState.GameIntroductionText = "There once was an example game.";
            GameState.CurrentGameState.Custom = this;

            // TODO: From the cemetary theatre you'll need to set the numbers on a combination lock to 1234 through the use command.
            // TODO: When a new game starts the combination should be initially set to 8734.
            // TODO: getting the combination right should lead to the warped hall
        }

        public static ExampleGameSourceData Current()
        {
           return GameState.CurrentGameState.Custom as ExampleGameSourceData;
        }
    }
}
