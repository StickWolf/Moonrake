using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamsAndWhatTheyMean
{
    class DragonKittyItems
    {
        public string Money { get; private set; }

        public string Paper { get; private set; }

        public string BronzeChunk { get; private set; }

        public string BronzeBar { get; private set; }

        public DragonKittyItems(TheTaleOfTheDragonKittySourceData gameData)
        {
            Money = gameData.AddItem(new Item("Dollar") { IsUnique = false });

            Paper = gameData.AddItem(new Item("Paper") { IsUnique = false });

            BronzeChunk = gameData.AddItem(new Item("Bronze Chunk") { IsUnique = false });

            BronzeBar = gameData.AddItem(new Item("Bronze Bar") { IsUnique = false });
        }
    }
}
