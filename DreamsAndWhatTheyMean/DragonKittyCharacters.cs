using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamsAndWhatTheyMean
{
    class DragonKittyCharacters
    {
        public string Player { get; private set; }
        public string MomCharacter { get; private set; }
        public string DadCharacter { get; private set; }
        public string BlackSmithCharacter { get; private set; }

        public DragonKittyCharacters(TheTaleOfTheDragonKittySourceData gameData)
        {
            Player = gameData.AddCharacter(new Character("Player", 50, 10));
            MomCharacter = gameData.AddCharacter(new Character("Mom", 4000, 150));
            DadCharacter = gameData.AddCharacter(new Character("Dad", 5000, 250));
            BlackSmithCharacter = gameData.AddCharacter(new Character("The Black-Smith", 10000, 700));
        }
    }
}
