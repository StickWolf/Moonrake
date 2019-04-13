using GameEngine;
using GameEngine.Locations;

namespace DreamsAndWhatTheyMean
{
    class TheTaleOfTheDragonKittyGameData : GameSourceData
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
            var charPlayer = AddCharacter(new Character("Player", 50, 40));
            var charPlayersDad = AddCharacter(new Character("Dad", 500, 250));
            var charPlayersMom = AddCharacter(new Character("Mom", 400, 150));
            var charBlackSmith = AddCharacter(new Character("The Black-Smith", 1000, 700));
            #endregion

            #region Locations
            var locPlayersLivingRoom = AddLocation(new Location("Your House (Living Room)", "a regular room that blends in with anything.",
               "You are in the living room of your house, you find nothing of use here, except to throw things. " +
               "As you go explore, you find out that most of the plants here are made of plastic. "
                ));

            var locPlayersRoom = AddLocation(new Location("Your House (Your Room)", "a messy room that most would run from.",
                "You are in your room, yet you find nothing of use here, except the paper on your table." +
                " You feel proud of your knowlege of folding ninja-stars when you see the paper."
                ));

            var locPlayersBackyard = AddLocation(new Location("Your House (The Backyard)", "a nice area to go to calm down when having a fight.",
                "You stand in your backyard, the rushing air and the sound of the birds relaxes you. " + 
                "This place brings back your knowlege of what you did with your family here."
                ));

            var locPlayersStreetHeLivesOn = AddLocation(new Location("BR Street", "a dangerous area to go to during the daytime, since the cars are everywhere.",
                "You stand in the middle of the street you walk on every day, while you are lucky that you haven't been hit by a car." +
                " The best thing for you do now is to move out of the road."
                ));

            var locStreetNextToBRStreet = AddLocation(new Location("ES Street", "a area so crowded with cars, making it imposible for an accident to happen.",
                "You stand in the middle of a street less familiar to you, there are stores all over the place." +
                " But you are also standing in the middle of the road, move fast, there are cars beeping at you."
                ));

            var locTheBlackSmith = AddLocation(new Location("The Black-Smith", "a shop that makes metal for you",
                "You stand in the Black-Smith store, you wish you had ice-cream here, because it is very hot."
                ));
            #endregion

            #region Portals
            AddPortal(
               new PortalAlwaysOpenRule(locPlayersLivingRoom, locPlayersRoom, "Through the door shaped like a star you see"),

               new PortalAlwaysOpenRule(locPlayersRoom, locPlayersLivingRoom, "Through the star shaped door you see")
               );

            AddPortal(
                new PortalAlwaysOpenRule(locPlayersLivingRoom, locPlayersBackyard, "Through the screen door you see"),

                new PortalAlwaysOpenRule(locPlayersBackyard, locPlayersLivingRoom, "Through the screen door you see")
                );

            AddPortal(
                new PortalAlwaysOpenRule(locPlayersLivingRoom, locPlayersStreetHeLivesOn, "Through the front door you see"),

                new PortalAlwaysOpenRule(locPlayersStreetHeLivesOn, locPlayersLivingRoom, "Through a door you can clearly see is a door to your house, you see")
                );

            AddPortal(
                new PortalAlwaysOpenRule(locPlayersStreetHeLivesOn, locStreetNextToBRStreet, "To the corner of the road you see"),

                new PortalAlwaysOpenRule(locStreetNextToBRStreet, locPlayersStreetHeLivesOn, "To the corner of the road you see")
                );

            AddPortal(
                new PortalAlwaysOpenRule(locStreetNextToBRStreet, locTheBlackSmith, "To a side of the road you see"),

                new PortalAlwaysOpenRule(locTheBlackSmith, locStreetNextToBRStreet, "Out the exit of the shop you see")
                );
            #endregion

            #region Items
            var moneyItem = AddItem(new Item("Dollars") { IsUnique = false });
            var paperItem = AddItem(new Item("Paper") { IsUnique = false });
            var bronzeSlabItem = AddItem(new Item("Bronze Slab") { IsUnique = false });
            var bronzeBarItem = AddItem(new Item("Bronze Bar") { IsUnique = false });
            #endregion

            #region Starter Items
            AddDefaultCharacterItem(charPlayer, moneyItem, 200);
            #endregion

            #region Room Items
            AddDefaultLocationItem(locPlayersRoom, moneyItem, 30);
            AddDefaultLocationItem(locPlayersRoom, paperItem, 100);
            AddDefaultLocationItem(locPlayersBackyard, bronzeSlabItem, 10);
            AddDefaultLocationItem(locPlayersBackyard, moneyItem, 1);
            AddDefaultLocationItem(locTheBlackSmith, bronzeBarItem, 3);
            #endregion

            #region Character Locations
            AddDefaultCharacterLocation(charPlayer, locPlayersRoom);
            AddDefaultCharacterLocation(charPlayersDad, locPlayersBackyard);
            AddDefaultCharacterLocation(charPlayersMom, locPlayersLivingRoom);
            AddDefaultCharacterLocation(charBlackSmith, locTheBlackSmith);
            #endregion

            #region Trade-Sets
            var tsBlackSmith = AddTradeSet("Metal",
                new ItemRecipe(bronzeBarItem,
                    new ItemRecipeIngredient(bronzeSlabItem, 3),
                    new ItemRecipeIngredient(moneyItem, 50)
                    ));
            #endregion

            #region Trade-Posts
            var tpBlackSmith = AddTradePost("The Black-Smith",
                tsBlackSmith);
            DefaultTradePostLocations[tpBlackSmith] = locTheBlackSmith; 
            #endregion
        }
    }
}
