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

        public Guid LilysMainRoom { get; private set; }

        public Guid LilysSecretRoom { get; private set; }

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

            JamesKitchen = GameState.CurrentGameState.AddLocation(new Location("Jame's House (Kitchen)", "a place to cook food.",
                "You stand in the kicthen of Jame's house, you greatly want to grab a snack from here."
                ));

            LilysMainRoom = GameState.CurrentGameState.AddLocation(new Location("Lily's Shed (Main Room)", "a strange, lone place.",
                "You stand in a tidy room, wanting to know who is cleaning this place."
                ));

            LilysSecretRoom = GameState.CurrentGameState.AddLocation(new Location("Lily's Shed (Secret Room)", "a room full of machines.",
                "You stand in a place that is full of machines, you want to know who made these and why."
                ));


            BRStreet = GameState.CurrentGameState.AddLocation(new Location("BR Street", "a very calm street.",
                "You stand on the street you and you friends live on, you can visit a friend's house from here."
                ));

            ESStreet = GameState.CurrentGameState.AddLocation(new Location("ES Street", "a area so crowded with cars, making it imposible for an accident to happen.",
                "You stand in a street that has lots of stores in it, you can by something here."
                ));

            BlackSmithShop = GameState.CurrentGameState.AddLocation(new Location("The Black-Smith", "a shop that makes metal for you",
                "You stand in the Black-Smith store, you wish you had ice-cream here, because it is very hot."
                ));

            // JamesLivingRoom <-> JamesRoom
            GameState.CurrentGameState.AddPortal(
               new PortalAlwaysOpenRule(JamesRoom, JamesLivingRoom, "Through a white door you see"),

               new PortalAlwaysOpenRule(JamesLivingRoom, JamesRoom, "Through a white door you see")
               );

            // JamesLivingRoom <-> JamesBackyard
            GameState.CurrentGameState.AddPortal(
                new PortalAlwaysOpenRule(JamesLivingRoom, JamesBackyard, "Through the screen door you see"),

                new PortalAlwaysOpenRule(JamesBackyard, JamesLivingRoom, "Through the screen door you see")
                );

            // JamesLivingRoon <-> JamesKitchen
            GameState.CurrentGameState.AddPortal(
                new PortalAlwaysOpenRule(JamesLivingRoom, JamesKitchen, "Through the open door you see"),

                new PortalAlwaysOpenRule(JamesKitchen, JamesRoom, "Through the open door you see")
                );
            // JamesLivingRoom <-> BRStreet
            GameState.CurrentGameState.AddPortal(
                new PortalAlwaysOpenRule(JamesLivingRoom, BRStreet, "Through the front door you see"),

                new PortalAlwaysOpenRule(BRStreet, JamesLivingRoom, "Through the door to your house, you see")
                );

            // BRStreet <-> LilysMainRoom
            GameState.CurrentGameState.AddPortal(
                new PortalAlwaysOpenRule(BRStreet, LilysMainRoom, "Through a broken door you see"),

                new PortalAlwaysOpenRule(LilysMainRoom, BRStreet, "Through a hole in the door you see")
                );

            //LilysMainRoom <-> LilysSecretRoom
            GameState.CurrentGameState.AddPortal(
              new PortalAlwaysOpenRule(LilysMainRoom, LilysSecretRoom, "Through a trapdoor you see"),

              new PortalAlwaysOpenRule(LilysSecretRoom, LilysMainRoom, "Through the trapdoor you see")
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
