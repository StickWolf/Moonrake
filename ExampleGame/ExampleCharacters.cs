using ExampleGame.Characters;
using GameEngine;
using GameEngine.Characters;
using System;

namespace ExampleGame
{
    public class ExampleCharacters
    {
        public Guid Player { get; private set; }
        public Guid Rat1 { get; private set; }
        public Guid Rat2 { get; private set; }

        public ExampleCharacters(ExampleGameSourceData gameData)
        {
            Player = gameData.AddCharacter(new PlayerCharacter("Sally", 50) { MaxAttack = 40, CounterAttackChance = 75 }, gameData.Locations.Start);
            Rat1 = gameData.AddCharacter(new Rat("Rat", 7, 23) { MaxAttack = 10, CounterAttackChance = 15 }, gameData.Locations.Start);
            Rat2 = gameData.AddCharacter(new Rat("Rat", 8, 15) { MaxAttack = 12, CounterAttackChance = 17 }, gameData.Locations.BanquetHall);
        }
    }
}
