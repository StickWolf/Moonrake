using GameEngine;

namespace DreamsAndWhatTheyMean
{
    class TheTaleOfTheDragonKittySourceData : GameSourceData
    {
        public DragonKittyCharacters Characters { get; private set; }
        public DragonKittyLocations Locations { get; private set; }
        public DragonKittyGameVarables GameVarables { get; private set; }
        public DragonKittyItems Items { get; private set; }

        public TheTaleOfTheDragonKittySourceData()
        {
            Characters = new DragonKittyCharacters(this);
            Locations = new DragonKittyLocations(this);
            GameVarables = new DragonKittyGameVarables(this);
            Items = new DragonKittyItems(this);

            DefaultPlayerName = "James";

            GameIntroductionText = "Once, there was a group of kids." +
                " These kids played around every day, they even had a youtube channel that they shared." +
                " Now, in this story, you will join these kids and go on a journey with them." +
                " Their names are: Zach, and Amaya." +
                " Welcome to The Tale of The DragonKitty.";

            #region Starter Items
            AddDefaultCharacterItem(Characters.Player, Items.Money, 200);
            #endregion

            #region Room Items
            AddDefaultLocationItem(Locations.PlayersRoom, Items.Money, 30);
            AddDefaultLocationItem(Locations.PlayersRoom, Items.Paper, 21);
            AddDefaultLocationItem(Locations.PlayersBackyard, Items.BronzeChunk, 9);
            AddDefaultLocationItem(Locations.PlayersBackyard, Items.Money, 2);
            AddDefaultLocationItem(Locations.BlackSmithShop, Items.BronzeBar, 1);
            AddDefaultLocationItem(Locations.PlayersLivingRoom, Items.PlasticChunk, 23);
            AddDefaultLocationItem(Locations.PlayersRoom, Items.PlayersRoomLight, 1);
            AddDefaultLocationItem(Locations.PlayersLivingRoom, Items.PlayersLivingRoomLight, 1);
            AddDefaultLocationItem(Locations.ESStreet, Items.BronzeTalisman, 1);
            AddDefaultLocationItem(Locations.PlayersLivingRoom, Items.Apple, 10);
            #endregion

            #region Character Locations
            AddDefaultCharacterLocation(Characters.Player, Locations.PlayersRoom);
            AddDefaultCharacterLocation(Characters.DadCharacter, Locations.PlayersBackyard);
            AddDefaultCharacterLocation(Characters.MomCharacter, Locations.PlayersLivingRoom);
            AddDefaultCharacterLocation(Characters.BlackSmithCharacter, Locations.BlackSmithShop);
            #endregion

            #region Trade-Sets
            var tsBlackSmith = AddTradeSet("Metal",
                new ItemRecipe(Items.BronzeBar,
                    new ItemRecipeIngredient(Items.BronzeChunk, 3),
                    new ItemRecipeIngredient(Items.Money, 50)
                    ));
            #endregion

            #region Trade-Posts
            var tpBlackSmith = AddTradePost("The Black-Smith",
                tsBlackSmith);
            DefaultTradePostLocations[tpBlackSmith] = Locations.BlackSmithShop; 
            #endregion
        }
    }
}
