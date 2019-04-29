using GameEngine;

namespace DreamsAndWhatTheyMean
{
    class TheTaleOfTheDragonKittySourceData : GameSourceData
    {
        public DragonKittyCharacters DkCharacters { get; private set; }
        public DragonKittyLocations DkLocations { get; private set; }
        public DragonKittyGameVarables GameVarables { get; private set; }
        public DragonKittyItems DkItems { get; private set; }

        public TheTaleOfTheDragonKittySourceData()
        {
            DkCharacters = new DragonKittyCharacters(this);
            DkLocations = new DragonKittyLocations(this);
            GameVarables = new DragonKittyGameVarables(this);
            DkItems = new DragonKittyItems(this);

            GameIntroductionText = "Once, there was a group of kids." +
                " These kids played around every day, they even had a youtube channel that they shared." +
                " Now, in this story, you will join these kids and go on a journey with them." +
                " Their names are: Zach, and Amaya." +
                " Welcome to The Tale of The DragonKitty.";

            GameEndingText = "You have saved the last of the dragonkittys and you have won the game.";

            #region Starter Items
            AddDefaultCharacterItem(DkCharacters.Player, DkItems.Money, 200);
            #endregion

            #region Room Items
            AddDefaultLocationItem(DkLocations.PlayersRoom, DkItems.Money, 30);
            AddDefaultLocationItem(DkLocations.PlayersRoom, DkItems.Paper, 21);
            AddDefaultLocationItem(DkLocations.PlayersBackyard, DkItems.BronzeChunk, 9);
            AddDefaultLocationItem(DkLocations.PlayersBackyard, DkItems.Money, 2);
            AddDefaultLocationItem(DkLocations.BlackSmithShop, DkItems.BronzeBar, 1);
            AddDefaultLocationItem(DkLocations.PlayersLivingRoom, DkItems.PlasticChunk, 23);
            AddDefaultLocationItem(DkLocations.PlayersRoom, DkItems.PlayersRoomLight, 1);
            AddDefaultLocationItem(DkLocations.PlayersLivingRoom, DkItems.PlayersLivingRoomLight, 1);
            AddDefaultLocationItem(DkLocations.ESStreet, DkItems.BronzeTalisman, 1);
            AddDefaultLocationItem(DkLocations.PlayersLivingRoom, DkItems.Apple, 10);
            AddDefaultLocationItem(DkLocations.PlayersBackyard, DkItems.DadsWallet, 1);
            #endregion

            #region Trade-Sets
            var tsBlackSmith = AddTradeSet("Metal",
                new ItemRecipe(DkItems.BronzeBar,
                    new ItemRecipeIngredient(DkItems.BronzeChunk, 3),
                    new ItemRecipeIngredient(DkItems.Money, 50)
                    ));
            #endregion

            #region Trade-Posts
            var tpBlackSmith = AddTradePost(DkLocations.BlackSmithShop, "The Black-Smith",
                tsBlackSmith);
            #endregion
        }
    }
}
