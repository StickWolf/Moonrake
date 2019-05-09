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
        public Guid PlayersRoom { get; private set; }

        public Guid PlayersLivingRoom { get; private set; }

        public Guid PlayersBackyard { get; private set; }

        public Guid BRStreet { get; private set; }

        public Guid ESStreet { get; private set; }

        public Guid BlackSmithShop { get; private set; }

        public Guid AbandonedAlley { get; private set; }

        public void NewGame(TheTaleOfTheDragonKittySourceData gameData)
        {
            PlayersRoom = GameState.CurrentGameState.AddLocation(new Location("Your House (Your Room)", "a messy room that most would run from.",
                "You are in your room, yet you find nothing of use here, except the paper on your table." +
                " You feel proud of your knowlege of folding ninja-stars when you see the paper."
                ));

            PlayersLivingRoom = GameState.CurrentGameState.AddLocation(new Location("Your House (Living Room)", "a regular room that blends in with anything.",
               "You are in the living room of your house, you find nothing of use here, except to throw things. " +
               "As you go explore, you find out that most of the plants here are made of plastic. "
               ));

            PlayersBackyard = GameState.CurrentGameState.AddLocation(new Location("Your House (The Backyard)", "a nice area to go to calm down when having a fight.",
                "You stand in your backyard, the rushing air and the sound of the birds relaxes you. " +
                "This place brings back your knowlege of what you did with your family here."
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

            AbandonedAlley = GameState.CurrentGameState.AddLocation(new Location("Abandoned Alley", "a very creepy place.",
                "You stand in the alley, you find many broken machine parts here and a bit of human flesh."
                ));

            // PlayersLivingRoom <-> PlayersRoom
            GameState.CurrentGameState.AddPortal(
               new PortalAlwaysOpenRule(PlayersRoom, PlayersLivingRoom, "Through a white door you see"),

               new PortalAlwaysOpenRule(PlayersLivingRoom, PlayersRoom, "Through a white door you see")
               );

            // PlayersLivingRoom <-> PlayersBackyard
            GameState.CurrentGameState.AddPortal(
                new PortalAlwaysOpenRule(PlayersLivingRoom, PlayersBackyard, "Through the screen door you see"),

                new PortalAlwaysOpenRule(PlayersBackyard, PlayersLivingRoom, "Through the screen door you see")
                );

            // PlayersLivingRoom <-> BRStreet
            GameState.CurrentGameState.AddPortal(
                new PortalAlwaysOpenRule(PlayersLivingRoom, BRStreet, "Through the front door you see"),

                new PortalAlwaysOpenRule(BRStreet, PlayersLivingRoom, "Through the door to your house, you see")
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
