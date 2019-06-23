using DragonKittyServer.DragonKittyStrangeItems;
using ServerEngine;
using ServerEngine.GrainSiloAndClient;
using System;

namespace DragonKittyServer
{
    class DragonKittyItems
    {
        public Guid Money { get; private set; }
        public Guid Paper { get; private set; }
        public Guid BronzeChunk { get; private set; }
        public Guid BronzeBar { get; private set; }
        public Guid PlasticChunk { get; private set; }
        public Guid PlayersRoomLight { get; private set; }
        public Guid PlayersLivingRoomLight { get; private set; }
        public Guid BronzeTalisman { get; private set; }
        public Guid Apple { get; private set; }
        public Guid DadsWallet { get; private set; }

        public void NewWorld(DragonKittySourceData gameData)
        {
            Money = GrainClusterClient.Universe.AddItem(new Item("dollar")).Result;
            Paper = GrainClusterClient.Universe.AddItem(new Item("paper piece")).Result;
            BronzeChunk = GrainClusterClient.Universe.AddItem(new Item("bronze chunk")).Result;
            BronzeBar = GrainClusterClient.Universe.AddItem(new Item("bronze bar")).Result;
            PlasticChunk = GrainClusterClient.Universe.AddItem(new Item("leftover plastic part")).Result;
            PlayersRoomLight = GrainClusterClient.Universe.AddItem(new RoomLight("White", true, 1)).Result;
            PlayersLivingRoomLight = GrainClusterClient.Universe.AddItem(new RoomLight("White", true, 2)).Result;
            BronzeTalisman = GrainClusterClient.Universe.AddItem(new BronzeTalisman()).Result;
            Apple = GrainClusterClient.Universe.AddItem(new Apple()).Result;
            DadsWallet = GrainClusterClient.Universe.AddItem(new Wallet(gameData.DkCharacters.DadCharacter, 10000)).Result;
        }
    }
}
