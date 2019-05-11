using GameEngine;
using GameEngine.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamsAndWhatTheyMean
{
    class DragonKittyLocations
    {
        public Guid JamesRoom { get; private set; }

        public Guid JamesLivingRoom { get; private set; }

        public Guid JamesBackyard { get; private set; }

        public Guid JamesKitchen { get; private set; }

        public Guid BRStreet { get; private set; }

        public Guid ESStreet { get; private set; }

        public Guid BlackSmithShop { get; private set; }

        public void NewGame(TheTaleOfTheDragonKittySourceData gameData)
        {
            JamesRoom = GameState.CurrentGameState.AddLocation(new Location("Jame's House (Jame's Room)", "a clean tidy room that would be fun to mess up.",
                "You find yourself in Jame's room, it is clean here, so it is fun to play in here."
                ));

            JamesLivingRoom = GameState.CurrentGameState.AddLocation(new Location("Jame's House (Living Room)", "a place to eat stuff.",
               "You are in the living room of Jame's house, it is boring here, you want food."
               ));

            JamesBackyard = GameState.CurrentGameState.AddLocation(new Location("Jame's House (Backyard)", "a area that is covered in bark and bones.",
                "You sit in the backyard of Jame's House, you see lots of bones, you are scared."
                ));

            BRStreet = GameState.CurrentGameState.AddLocation(new Location("BR Street", "a dangerous area to go to during the daytime, since the cars are everywhere.",
                "You stand in the middle of the street you walk on every day, while you are lucky that you haven't been hit by a car." +
                " The best thing for you do now is to move out of the road."
                ));

            ESStreet = GameState.CurrentGameState.AddLocation(new Location("ES Street", "a area so crowded with cars, making it imposible for an accident to happen.",
                "You stand in the middle of a street less familiar to you, there are stores all over the place." +
                " But you are also standing in the middle of the road, move fast, there are cars beeping at you."
                ));

            BlackSmithShop = GameState.CurrentGameState.AddLocation(new Location("The Black-Smith", "a shop that makes metal for you",
                "You stand in the Black-Smith store, you wish you had ice-cream here, because it is very hot."
                ));

            // PlayersLivingRoom <-> PlayersRoom
            GameState.CurrentGameState.AddPortal(
               new PortalAlwaysOpenRule(JamesRoom, JamesLivingRoom, "Through a white door you see"),

               new PortalAlwaysOpenRule(JamesLivingRoom, JamesRoom, "Through a white door you see")
               );

            // PlayersLivingRoom <-> PlayersBackyard
            GameState.CurrentGameState.AddPortal(
                new PortalAlwaysOpenRule(JamesLivingRoom, JamesBackyard, "Through the screen door you see"),

                new PortalAlwaysOpenRule(JamesBackyard, JamesLivingRoom, "Through the screen door you see")
                );

            // PlayersLivingRoom <-> BRStreet
            GameState.CurrentGameState.AddPortal(
                new PortalAlwaysOpenRule(JamesLivingRoom, BRStreet, "Through the front door you see"),

                new PortalAlwaysOpenRule(BRStreet, JamesLivingRoom, "Through the door to your house, you see")
                );

            // BRStreet <-> ESStreet
            GameState.CurrentGameState.AddPortal(
                new PortalAlwaysOpenRule(BRStreet, ESStreet, "To the corner of the road you see"),

                new PortalAlwaysOpenRule(ESStreet, BRStreet, "To the corner of the road you see")
                );

            GameState.CurrentGameState.AddPortal(
                new PortalAlwaysOpenRule(ESStreet, BlackSmithShop, "To a side of the road you see"),

                new PortalAlwaysOpenRule(BlackSmithShop, ESStreet, "Out the shop you see")
                );
        }
    }
}
