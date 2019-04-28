﻿using GameEngine.Characters;
using System;

namespace DreamsAndWhatTheyMean
{
    class DragonKittyCharacters
    {
        public Guid Player { get; private set; }
        public Guid MomCharacter { get; private set; }
        public Guid DadCharacter { get; private set; }
        public Guid BlackSmithCharacter { get; private set; }

        public DragonKittyCharacters(TheTaleOfTheDragonKittySourceData gameData)
        {
            Player = gameData.AddCharacter(new PlayerCharacter("James", 50) { MaxAttack = 10234, CounterAttackChance = 50 });
            MomCharacter = gameData.AddCharacter(new Character("Mom", 4000) { MaxAttack = 150, CounterAttackChance = 20 });
            DadCharacter = gameData.AddCharacter(new Character("Dad", 5000) { MaxAttack = 250, CounterAttackChance = 30 });
            BlackSmithCharacter = gameData.AddCharacter(new Character("The Black-Smith", 10000) { MaxAttack = 700, CounterAttackChance = 40 });
        }
    }
}
