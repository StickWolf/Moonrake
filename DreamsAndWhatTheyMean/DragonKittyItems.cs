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
        public string PlayersSpareLight { get; private set; }

        public DragonKittyItems(TheTaleOfTheDragonKittySourceData gameData)
        {
            Money = gameData.AddItem(new Item("Dollar", "Dollar"));
            Paper = gameData.AddItem(new Item("PaperPiece", "Paper Piece"));
            BronzeChunk = gameData.AddItem(new Item("BronzeChunk", "Bronze Chunk"));
            BronzeBar = gameData.AddItem(new Item("BronzeBar", "Bronze Bar"));
            PlasticChunk = gameData.AddItem(new Item("LeftoverPlasticPart", "Leftover Plastic Part"));
            PlayersRoomLight = gameData.AddItem(new RoomLight("White", true, 1));
            PlayersLivingRoomLight = gameData.AddItem(new RoomLight("White", true, 2));
            PlayersSpareLight = gameData.AddItem(new LightMine("White", false));
        }
    }
}
