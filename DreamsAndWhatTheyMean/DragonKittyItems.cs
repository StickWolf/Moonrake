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

        public DragonKittyItems(TheTaleOfTheDragonKittySourceData gameData)
        {
            Money = gameData.AddItem(new Item("Dollar", "Dollar"));

            Paper = gameData.AddItem(new Item("Paper", "Paper"));

            BronzeChunk = gameData.AddItem(new Item("BronzeChunk", "Bronze Chunk"));

            BronzeBar = gameData.AddItem(new Item("BronzeBar", "Bronze Bar"));

            PlasticChunk = gameData.AddItem(new Item("LeftoverPlasticPart", "Leftover Plastic Part"));
        }
    }
}
