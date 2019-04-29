using DreamsAndWhatTheyMean.DragonKittyStrangeItems;
using GameEngine;
using System;

namespace DreamsAndWhatTheyMean
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

        public DragonKittyItems(TheTaleOfTheDragonKittySourceData gameData)
        {
            Money = gameData.AddItem(new Item("dollar"));
            Paper = gameData.AddItem(new Item("paper piece"));
            BronzeChunk = gameData.AddItem(new Item("bronze chunk"));
            BronzeBar = gameData.AddItem(new Item("bronze bar"));
            PlasticChunk = gameData.AddItem(new Item("leftover plastic part"));
            PlayersRoomLight = gameData.AddItem(new RoomLight("White", true, 1));
            PlayersLivingRoomLight = gameData.AddItem(new RoomLight("White", true, 2));
            BronzeTalisman = gameData.AddItem(new BronzeTalisman());
            Apple = gameData.AddItem(new Apple());
            DadsWallet = gameData.AddItem(new Wallet(gameData.DkCharacters.DadCharacter, gameData.DkItems.Money, 10000));
        }
    }
}
