using GameEngine;

namespace ExampleGame
{
    /// <summary>
    /// This class gives an example of how to implement all the features available to the game data
    /// </summary>
    public class ExampleGameSourceData : GameSourceData
    {
        public ExampleGameVariables GameVariables { get; private set; }
        public ExampleCharacters Characters { get; private set; }
        public ExampleLocations Locations { get; private set; }
        public ExampleItems Items { get; private set; }

        /// <summary>
        /// Constructor that fills in the data
        /// </summary>
        public ExampleGameSourceData()
        {
            GameVariables = new ExampleGameVariables(this);
            Characters = new ExampleCharacters(this);
            Locations = new ExampleLocations(this);
            Items = new ExampleItems(this);

            DefaultPlayerName = "Sally";
            GameIntroductionText = "There once was an example game.";

            // Default locations where characters are
            AddDefaultCharacterLocation(Characters.Player, Locations.Start);

            // Default locations where items are
            AddDefaultLocationItem(Locations.Start, Items.DullBronzeKey, 1);

            #region Events

            // TODO: Implement the "use" command before doing this
            // TODO: From the banquet hall the user will see a locked door. (the one leading into the warped hall)
            // TODO: The player can use the key (itemDullBronzeKey) on the door with the use command. Other keys should not work to open the door.
            // TODO: This should trigger an event that sets gvBanquetSecretHallOpen to true if it is false and false if it is true.
            // TODO: The event should also describe the door opening or closing.

            // TODO: From the cemetary theatre you'll need to set the numbers on a combination lock to 1234 through a command.
            // TODO: The command that lets the user change the combination is yet to be defined.
            // TODO: When a new game starts the combination should be initially set to 8734.
            // TODO: getting the combination right should lead to the warped hall

            #endregion
        }
    }
}
