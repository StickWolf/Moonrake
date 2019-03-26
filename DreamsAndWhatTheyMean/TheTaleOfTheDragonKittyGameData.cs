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
                " Wellcome to The Tale of The DragonKitty.";

            #region Characters
            Characters.Add(new Character("Player", 50));
            #endregion

            #region Locations
            var locPlayersLivingRoom = new Location("Your House (Living Room) ", "an regular room that blends in with anything.",
               "You are in the living room of your house, you find nothing of use here, except to throw things at your parents. " +
               "As you go explore, you find out that most of the plants here are made of plastic. "
                );
            Locations.Add(locPlayersLivingRoom);
            StartingLocationName = locPlayersLivingRoom.Name;

            var locPlayersRoom = new Location("Your House (Your Room)", "a messy room that most would run from.",
                "You are in your room, yet you find nothing of use here, except the paper on your table." +
                " You feel proud of your knowlege of folding ninja-stars when you see the paper."
                );
            Locations.Add(locPlayersRoom);

            var locPlayersBackyard = new Location("Your House (The Backyard)", "a nice area to go to calm down when having a fight",
                "You stand in your backyard, the rushing air and the sound of the birds relaxes you. " + 
                "This place brings back your knowlege of what you did with your family here."
                );
            Locations.Add(locPlayersBackyard);
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
            #endregion
        }
    }
}
