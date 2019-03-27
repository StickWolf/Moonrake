using GameEngine;
using GameEngine.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamsAndWhatTheyMean
{
    class TheTaleOfTheDragonKittyGameData : GameSourceDataBase
    {
        public TheTaleOfTheDragonKittyGameData()
        {
            DefaultPlayerName = "James";

            GameIntroductionText = "Once, there was a group of kids." +
                " These kids played around every day, they even had a youtube channel that they shared." +
                " Now, in this story, you will join these kids and go on a journey with them." +
                " Their names are: Zach, and Amaya." +
                " Welcome to The Tale of The DragonKitty.";

            #region Characters
            Characters.Add(new Character("Player", 20));
            #endregion
            
            #region Locations
            var locPlayersLivingRoom = new Location("Your House (Living Room)", "a regular room that blends in with anything.",
               "You are in the living room of your house, you find nothing of use here, except to throw things at your parents. " +
               "As you go explore, you find out that most of the plants here are made of plastic. "
                );
            Locations.Add(locPlayersLivingRoom);

            var locPlayersRoom = new Location("Your House (Your Room)", "a messy room that most would run from.",
                "You are in your room, yet you find nothing of use here, except the paper on your table." +
                " You feel proud of your knowlege of folding ninja-stars when you see the paper."
                );
            Locations.Add(locPlayersRoom);
            StartingLocationName = locPlayersRoom.Name;

            var locPlayersBackyard = new Location("Your House (The Backyard)", "a nice area to go to calm down when having a fight.",
                "You stand in your backyard, the rushing air and the sound of the birds relaxes you. " + 
                "This place brings back your knowlege of what you did with your family here."
                );
            Locations.Add(locPlayersBackyard);

            var locPlayersStreetHeLivesOn = new Location("BR Street", "a dangerous area to go to during the daytime, since the cars are everywhere.",
                "You stand in the middle of the street you walk on every day, while you are lucky that you haven't been hit by a car." +
                " The best thing for you do now is to move out of the road."
                );
            Locations.Add(locPlayersStreetHeLivesOn);

            var locStreetNextToBRStreet = new Location("ES Street", "a area so crowded with cars, making it imposible for an acident to happen.",
                "You stand in the middle of a street less familiar to you, there are stores all over the place." +
                " But you are also standing in the middle of the road, move fast, there are cars beeping at you."
                );
            Locations.Add(locStreetNextToBRStreet);
            #endregion

            #region Portals
            AddPortal(
               new PortalAlwaysOpenRule(locPlayersLivingRoom.Name, locPlayersRoom.Name, "Through the door shaped like a star you see"),

               new PortalAlwaysOpenRule(locPlayersRoom.Name, locPlayersLivingRoom.Name, "Through the star shaped door you see")
               );

            AddPortal(
                new PortalAlwaysOpenRule(locPlayersLivingRoom.Name, locPlayersBackyard.Name, "Through the screen door you see"),

                new PortalAlwaysOpenRule(locPlayersBackyard.Name, locPlayersLivingRoom.Name, "Through the screen door you see")
                );

            AddPortal(
                new PortalAlwaysOpenRule(locPlayersLivingRoom.Name, locPlayersStreetHeLivesOn.Name, "Through the front door you see"),

                new PortalAlwaysOpenRule(locPlayersStreetHeLivesOn.Name, locPlayersLivingRoom.Name, "Through a door you can clearly see is a door to your house, you see")
                );

            AddPortal(
                new PortalAlwaysOpenRule(locPlayersStreetHeLivesOn.Name, locStreetNextToBRStreet.Name, "To the corner of the road you see"),

                new PortalAlwaysOpenRule(locStreetNextToBRStreet.Name, locPlayersStreetHeLivesOn.Name, "To the corner of the road you see")
                );
            #endregion
        }
    }
}
