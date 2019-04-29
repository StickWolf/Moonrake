using GameEngine;

namespace ExampleGame
{
    /// <summary>
    /// This class gives an example of how to implement all the features available to the game data
    /// </summary>
    public class ExampleGameSourceData : GameSourceData
    {
        public ExampleGameVariables GameVariables { get; private set; }
        public ExampleLocations EgLocations { get; private set; }
        public ExampleCharacters EgCharacters { get; private set; }
        public ExampleItems EgItems { get; private set; }

        /// <summary>
        /// Constructor that fills in the data
        /// </summary>
        public ExampleGameSourceData()
        {
            // Note that the properties should be defined in this order due to how they reference each other
            GameVariables = new ExampleGameVariables(this);
            EgLocations = new ExampleLocations(this);
            EgCharacters = new ExampleCharacters(this);
            EgItems = new ExampleItems(this);

            GameIntroductionText = "There once was an example game.";

            // TODO: From the cemetary theatre you'll need to set the numbers on a combination lock to 1234 through the use command.
            // TODO: When a new game starts the combination should be initially set to 8734.
            // TODO: getting the combination right should lead to the warped hall
        }
    }
}
