using ServerEngine;

namespace DragonKittyServer
{
    class DragonKittySourceData
    {
        public DragonKittyCharacters DkCharacters { get; private set; } = new DragonKittyCharacters();
        public DragonKittyLocations DkLocations { get; private set; } = new DragonKittyLocations();
        public DragonKittyGameVarables GameVarables { get; private set; } = new DragonKittyGameVarables();
        public DragonKittyItems DkItems { get; private set; } = new DragonKittyItems();

        public void NewWorld()
        {
            DkLocations.NewWorld(this);
            DkCharacters.NewWorld(this);
            GameVarables.NewWorld(this);
            DkItems.NewWorld(this);

            GameState.CurrentGameState.GameIntroductionText = "Once, there was a group of kids." +
                " These kids played around every day, they even had a youtube channel that they shared." +
                " Now, in this story, you will join these kids and go on a journey with them." +
                " Their names are: Zach, and Amaya." +
                " Welcome to The Tale of The DragonKitty.";

            #region Room Items
            GameState.CurrentGameState.TryAddLocationItemCount(DkLocations.PlayersRoom, DkItems.Money, 30);
            GameState.CurrentGameState.TryAddLocationItemCount(DkLocations.PlayersRoom, DkItems.Paper, 21);
            GameState.CurrentGameState.TryAddLocationItemCount(DkLocations.PlayersBackyard, DkItems.BronzeChunk, 9);
            GameState.CurrentGameState.TryAddLocationItemCount(DkLocations.PlayersBackyard, DkItems.Money, 2);
            GameState.CurrentGameState.TryAddLocationItemCount(DkLocations.BlackSmithShop, DkItems.BronzeBar, 1);
            GameState.CurrentGameState.TryAddLocationItemCount(DkLocations.PlayersLivingRoom, DkItems.PlasticChunk, 23);
            GameState.CurrentGameState.TryAddLocationItemCount(DkLocations.PlayersRoom, DkItems.PlayersRoomLight, 1);
            GameState.CurrentGameState.TryAddLocationItemCount(DkLocations.PlayersLivingRoom, DkItems.PlayersLivingRoomLight, 1);
            GameState.CurrentGameState.TryAddLocationItemCount(DkLocations.ESStreet, DkItems.BronzeTalisman, 1);
            GameState.CurrentGameState.TryAddLocationItemCount(DkLocations.PlayersLivingRoom, DkItems.Apple, 10);
            GameState.CurrentGameState.TryAddLocationItemCount(DkLocations.PlayersBackyard, DkItems.DadsWallet, 1);
            #endregion

            #region Trade-Sets
            var tsBlackSmith = GameState.CurrentGameState.AddTradeSet("Metal",
                new ItemRecipe(DkItems.BronzeBar,
                    new ItemRecipeIngredient(DkItems.BronzeChunk, 3),
                    new ItemRecipeIngredient(DkItems.Money, 50)
                    ));
            #endregion

            #region Trade-Posts
            var tpBlackSmith = GameState.CurrentGameState.AddTradePost(DkLocations.BlackSmithShop, "The Black-Smith",
                tsBlackSmith);
            #endregion

            GameState.CurrentGameState.Custom = this;
        }

        public static DragonKittySourceData Current()
        {
            return GameState.CurrentGameState.Custom as DragonKittySourceData;
        }
    }
}
