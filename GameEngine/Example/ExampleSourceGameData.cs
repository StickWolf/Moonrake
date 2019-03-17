using GameEngine;
using GameEngine.Locations;
using System.Collections.Generic;

namespace GameData
{
    /// <summary>
    /// This class gives an example of how to implement all the features available to the game data
    /// </summary>
    public class ExampleSourceGameData : IGameSourceData
    {
        public string DefaultPlayerName => "Sally";

        public string GameIntroductionText => 
            "There once was an example game.";

        public List<Character> Characters => new List<Character>()
        {
            new Character("Player", 50)
        };

        public List<Location> Locations => CreateLocations();

        private List<Location> CreateLocations()
        {
            var locations = new List<Location>();

            var locAreaToTheWest = new Location("AreaToTheWest");
            locations.Add(locAreaToTheWest);

            var locAreaToTheEast = new Location("AreaToTheEast");
            locations.Add(locAreaToTheEast);

            var locStart = new Location("StartingArea");
            locStart.LocationConnections.Add(new LocationConnectionBasic(locAreaToTheWest.Name));
            locStart.LocationConnections.Add(new LocationConnectionBasic(locAreaToTheEast.Name));
            locations.Add(locStart);

            // TODO: Create a new location named LockedArea1 that west/east are both connected to, but you need to figure out locks to get into them

            // TODO: From the west the user will see a locked door.
            // TODO: When the game starts the state of this door should be set in the saved game data as locked.
            // TODO: The user should have picked up a key from the starting area. They can now use this key to open the door.
            // TODO: The command that lets the user pick up things and the command that lets the user use things are yet to be defined.
            // TODO: After the user uses the key on the door it should set that this door is now open in the saved game data.
            // TODO: When the user looks around in the west room
            // TODO:    If the door is locked it will get 1 text
            // TODO:        localtext (which is obtained from the west room) = "you see a locked door"
            // TODO:        The game will display "You see a locked door"
            // TODO:    If the door is opened the game should get 2 parts of text
            // TODO:        remotetext (which is obtained from LockedRoom1) = "a room with many locks inside"
            // TODO:        localtext (which is obtained from the west room) = "through a now open door you see"
            // TODO:        The game will put those together and display "Through a now open door you see a room with many locks inside"

            // TODO: From the east you'll need to set the numbers on a combination lock to 1234 through a command.
            // TODO: The command that lets the user change the combination is yet to be defined.
            // TODO: When a new game starts the combination should be initially set to 8734.
            // TODO: The current state of the combination should be part of the saved game data.
            // TODO: When the player is in the east room and looks around follows the same logic as defined for the west room above.

            return locations;
        }
    }
}
