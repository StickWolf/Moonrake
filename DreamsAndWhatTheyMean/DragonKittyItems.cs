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

        public void NewGame(TheTaleOfTheDragonKittySourceData gameData)
        {
            Money = GameState.CurrentGameState.AddItem(new Item("dollar"));
            Paper = GameState.CurrentGameState.AddItem(new Item("paper piece"));
            BronzeChunk = GameState.CurrentGameState.AddItem(new Item("bronze chunk"));
            BronzeBar = GameState.CurrentGameState.AddItem(new Item("bronze bar"));
            PlasticChunk = GameState.CurrentGameState.AddItem(new Item("leftover plastic part"));
            PlayersRoomLight = GameState.CurrentGameState.AddItem(new RoomLight("White", true, 1));
            PlayersLivingRoomLight = GameState.CurrentGameState.AddItem(new RoomLight("White", true, 2));
            BronzeTalisman = GameState.CurrentGameState.AddItem(new BronzeTalisman());
            Apple = GameState.CurrentGameState.AddItem(new Apple());
            DadsWallet = GameState.CurrentGameState.AddItem(new Wallet(gameData.DkCharacters.DadCharacter, gameData.DkItems.Money, 10000));
        }
    }
}
