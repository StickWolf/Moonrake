using GameEngine;
using GameEngine.Locations;
using System.Collections.Generic;

namespace ExampleGame
{
    /// <summary>
    /// This class gives an example of how to implement all the features available to the game data
    /// </summary>
    public class ExampleSourceGameData : GameSourceDataBase
    {
        /// <summary>
        /// Constructor that fills in the data
        /// </summary>
        public ExampleSourceGameData()
        {
            DefaultPlayerName = "Sally";

            GameIntroductionText = "There once was an example game.";

            #region Characters

            var charPlayer = AddCharacter(new Character("Player", 50));

            #endregion

            #region GameVars

            var gvBanquetElevatorFloor = AddDefaultGameVar("BanquetElevatorFloor", "1");
            var gvBanquetSecretHallOpen = AddDefaultGameVar("BanquetSecretHallOpen", "false");

            #endregion

            #region Locations

            var locStart = AddLocation(new Location("Starting Area", "an open area with a small marble colleseum",
                "You stand in the midsts of a miniture marble colleseum. " +
                "The pillars appear to have been carved by hand and you sense that this area is very old."
                ));
            AddDefaultCharacterLocation(charPlayer, locStart);

            var locBanquetHall = AddLocation(new Location("Banquet Hall", "a large hall filled with several tables",
                "Around you are several tables full of delicious looking food. " +
                "The hall is bustling with many visitors of different nationality"
                ));

            var locCemetaryTheatre = AddLocation(new Location("Cemetary Theatre", "a frightful looking cemetary",
                "Although the dirt in this area is real, the multitudes of tombstones are " +
                "obvious fakes. Upon further inspection you discover that they all have the " +
                "same name on them and have been made from a mold."
                ));

            var locBanquetElevator = AddLocation(new Location("Banquet Elevator", "an ornately designed elevator",
                "From within the elevator you view what could have only taken ages to craft. " +
                "The elevator walls are hand crafted from a giant, once whole, block of marble. " +
                "Although the design is from a historical era, the electronic components appear to " +
                "be more advanced than what you are familiar with."
                ));

            var locSecretWarpedHall = AddLocation(new Location("Warped Hallway", "a hallway that isn't quite straight",
                "As you peer down the hallway the walls curve in and out in a haphazardly manner."));

            #endregion

            #region Portals

            // This describes that the starting area is always connected to the banquet hall
            // and that the banquet hall is always connected to the starting area
            // Start <--> Banquet Hall
            AddPortal(
                new PortalAlwaysOpenRule(locStart, locBanquetHall, "To the west you see"),
                new PortalAlwaysOpenRule(locBanquetHall, locStart, "To the east you see")
                );

            // Another 2 way portal
            // Start <--> Cemetary
            AddPortal(
                new PortalAlwaysOpenRule(locStart, locCemetaryTheatre, "To the east you see"),
                new PortalAlwaysOpenRule(locCemetaryTheatre, locStart, "To the west you see")
                );

            // Banquet Hall <--> Elevator
            AddPortal(
                // These 2 rules will only be considered if the player is in the banquet hall.
                // If the game variable "BanquestElevatorFloor" is set to 1 then the first rule will match, otherwise it'll match the 2nd
                new PortalOpenGameVarRule(locBanquetHall, locBanquetElevator, "Through an open elevator door you see", gvBanquetElevatorFloor, "1"),
                new PortalAlwaysClosedRule(locBanquetHall, null, "You see a closed elevator door"),

                new PortalOpenGameVarRule(locBanquetElevator, locBanquetHall, "From within the elevator you peer through the door to see", gvBanquetElevatorFloor, "1"),
                new PortalAlwaysClosedRule(locBanquetElevator, null, "The elevator door is closed")
                );

            // Banquet Hall <--> Warped Hall
            AddPortal(
                new PortalOpenGameVarRule(locBanquetHall, locSecretWarpedHall, "Through the now open passageway you see", gvBanquetSecretHallOpen, "true"),
                new PortalAlwaysClosedRule(locBanquetHall, locSecretWarpedHall, "You see a large closed stone door with a keyhole. The door is locked."),

                new PortalOpenGameVarRule(locSecretWarpedHall, locBanquetHall, "Through the now open passageway you see", gvBanquetSecretHallOpen, "true"),
                new PortalAlwaysClosedRule(locSecretWarpedHall, locBanquetHall, "You see the end of the hallway. There is currently no way through in this direction.")
                );

            #endregion

            #region Items

            var itemDullBronzeKey = AddItem(new Item("Dull Bronze Key") { IsUnique = true });

            #endregion

            #region Location Items

            AddDefaultLocationItem(locStart, itemDullBronzeKey, 1);

            #endregion

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
