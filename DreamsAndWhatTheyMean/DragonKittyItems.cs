using DreamsAndWhatTheyMean.DragonKittyStrangeItems;
using GameEngine;

namespace DreamsAndWhatTheyMean
{
    class DragonKittyItems
    {
        public string Money { get; private set; }
        public string Paper { get; private set; }
        public string BronzeChunk { get; private set; }
        public string BronzeBar { get; private set; }
        public string PlasticChunk { get; private set; }
        public string PlayersRoomLight { get; private set; }
        public string PlayersLivingRoomLight { get; private set; }
        public string BronzeTalisman { get; private set; }
        public string Apple { get; private set; }

        public DragonKittyItems(TheTaleOfTheDragonKittySourceData gameData)
        {
            Money = gameData.AddItem(new Item("Dollar", "dollar"));
            Paper = gameData.AddItem(new Item("PaperPiece", "paper piece"));
            BronzeChunk = gameData.AddItem(new Item("BronzeChunk", "bronze chunk"));
            BronzeBar = gameData.AddItem(new Item("BronzeBar", "bronze bar"));
            PlasticChunk = gameData.AddItem(new Item("LeftoverPlasticPart", "leftover plastic part"));
            PlayersRoomLight = gameData.AddItem(new RoomLight("White", true, 1));
            PlayersLivingRoomLight = gameData.AddItem(new RoomLight("White", true, 2));
            BronzeTalisman = gameData.AddItem(new BronzeTalisman(gameData));
            Apple = gameData.AddItem(new Apple(gameData));
        }
    }
}
