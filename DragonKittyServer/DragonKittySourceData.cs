using ServerEngine;
using ServerEngine.GrainSiloAndClient;

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

            GrainClusterClient.Universe.SetGameIntroductionText("Once, there was a group of kids." +
                " These kids played around every day, they even had a youtube channel that they shared." +
                " Now, in this story, you will join these kids and go on a journey with them." +
                " Their names are: Zach, and Amaya." +
                " Welcome to The Tale of The DragonKitty.").Wait();

            #region Room Items
            GrainClusterClient.Universe.TryAddLocationItemCount(DkLocations.PlayersRoom, DkItems.Money, 30).Wait();
            GrainClusterClient.Universe.TryAddLocationItemCount(DkLocations.PlayersRoom, DkItems.Paper, 21).Wait();
            GrainClusterClient.Universe.TryAddLocationItemCount(DkLocations.PlayersBackyard, DkItems.BronzeChunk, 9).Wait();
            GrainClusterClient.Universe.TryAddLocationItemCount(DkLocations.PlayersBackyard, DkItems.Money, 2).Wait();
            GrainClusterClient.Universe.TryAddLocationItemCount(DkLocations.BlackSmithShop, DkItems.BronzeBar, 1).Wait();
            GrainClusterClient.Universe.TryAddLocationItemCount(DkLocations.PlayersLivingRoom, DkItems.PlasticChunk, 23).Wait();
            GrainClusterClient.Universe.TryAddLocationItemCount(DkLocations.PlayersRoom, DkItems.PlayersRoomLight, 1).Wait();
            GrainClusterClient.Universe.TryAddLocationItemCount(DkLocations.PlayersLivingRoom, DkItems.PlayersLivingRoomLight, 1).Wait();
            GrainClusterClient.Universe.TryAddLocationItemCount(DkLocations.ESStreet, DkItems.BronzeTalisman, 1).Wait();
            GrainClusterClient.Universe.TryAddLocationItemCount(DkLocations.PlayersLivingRoom, DkItems.Apple, 10).Wait();
            GrainClusterClient.Universe.TryAddLocationItemCount(DkLocations.PlayersBackyard, DkItems.DadsWallet, 1).Wait();
            #endregion

            #region Trade-Sets
            var tsBlackSmith = GrainClusterClient.Universe.AddTradeSet("Metal",
                new ItemRecipe(DkItems.BronzeBar,
                    new ItemRecipeIngredient(DkItems.BronzeChunk, 3),
                    new ItemRecipeIngredient(DkItems.Money, 50)
                    )).Result;
            #endregion

            #region Trade-Posts
            var tpBlackSmith = GrainClusterClient.Universe.AddTradePost(DkLocations.BlackSmithShop, "The Black-Smith",
                tsBlackSmith).Result;
            #endregion

            GrainClusterClient.Universe.SetCustom(this).Wait();
        }

        public static DragonKittySourceData Current()
        {
            return GrainClusterClient.Universe.GetCustom().Result as DragonKittySourceData;
        }
    }
}
