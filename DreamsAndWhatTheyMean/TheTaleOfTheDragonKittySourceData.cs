using GameEngine;

namespace DreamsAndWhatTheyMean
{
    class TheTaleOfTheDragonKittySourceData : GameSourceData
    {
        public DragonKittyCharacters DkCharacters { get; private set; }
        public DragonKittyLocations Locations { get; private set; }
        public DragonKittyGameVarables GameVarables { get; private set; }
        public DragonKittyItems Items { get; private set; }

        public TheTaleOfTheDragonKittySourceData()
        {
            DkCharacters = new DragonKittyCharacters(this);
            Locations = new DragonKittyLocations(this);
            GameVarables = new DragonKittyGameVarables(this);
            Items = new DragonKittyItems(this);

            GameIntroductionText = "Once, there was a group of kids." +
                " These kids played around every day, they even had a youtube channel that they shared." +
                " Now, in this story, you will join these kids and go on a journey with them." +
                " Their names are: Zach, and Amaya." +
                " Welcome to The Tale of The DragonKitty.";

            GameEndingText = "You have saved the last of the dragonkittys and you have won the game.";

            #region Starter Items
            AddDefaultCharacterItem(DkCharacters.Player, Items.Money, 200);
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
            AddDefaultLocationItem(Locations.PlayersBackyard, Items.DadsWallet, 1);
            #endregion

            #region Character Locations
            AddDefaultCharacterLocation(DkCharacters.Player, Locations.PlayersRoom);
            AddDefaultCharacterLocation(DkCharacters.DadCharacter, Locations.PlayersBackyard);
            AddDefaultCharacterLocation(DkCharacters.MomCharacter, Locations.PlayersLivingRoom);
            AddDefaultCharacterLocation(DkCharacters.BlackSmithCharacter, Locations.BlackSmithShop);
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
