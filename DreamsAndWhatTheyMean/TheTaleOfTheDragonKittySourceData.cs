using GameEngine;

namespace DreamsAndWhatTheyMean
{
    class TheTaleOfTheDragonKittySourceData : GameSourceData
    {
        public DragonKittyCharacters DkCharacters { get; private set; }
        public DragonKittyLocations DkLocations { get; private set; }
        public DragonKittyGameVarables GameVarables { get; private set; }
        public DragonKittyItems Items { get; private set; }

        public TheTaleOfTheDragonKittySourceData()
        {
            DkCharacters = new DragonKittyCharacters(this);
            DkLocations = new DragonKittyLocations(this);
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
            AddDefaultLocationItem(DkLocations.PlayersRoom, Items.Money, 30);
            AddDefaultLocationItem(DkLocations.PlayersRoom, Items.Paper, 21);
            AddDefaultLocationItem(DkLocations.PlayersBackyard, Items.BronzeChunk, 9);
            AddDefaultLocationItem(DkLocations.PlayersBackyard, Items.Money, 2);
            AddDefaultLocationItem(DkLocations.BlackSmithShop, Items.BronzeBar, 1);
            AddDefaultLocationItem(DkLocations.PlayersLivingRoom, Items.PlasticChunk, 23);
            AddDefaultLocationItem(DkLocations.PlayersRoom, Items.PlayersRoomLight, 1);
            AddDefaultLocationItem(DkLocations.PlayersLivingRoom, Items.PlayersLivingRoomLight, 1);
            AddDefaultLocationItem(DkLocations.ESStreet, Items.BronzeTalisman, 1);
            AddDefaultLocationItem(DkLocations.PlayersLivingRoom, Items.Apple, 10);
            AddDefaultLocationItem(DkLocations.PlayersBackyard, Items.DadsWallet, 1);
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
            DefaultTradePostLocations[tpBlackSmith] = DkLocations.BlackSmithShop; 
            #endregion
        }
    }
}
