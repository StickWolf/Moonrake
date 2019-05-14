using ExampleGameServer.Characters;
using ExampleGameServer.Characters.Behaviors;
using ServerEngine;
using ServerEngine.Characters;
using ServerEngine.Characters.Behaviors;
using System;
using System.Collections.Generic;

namespace ExampleGameServer
{
    public class ExampleCharacters
    {
        public Guid Rat1 { get; private set; }
        public Guid Rat2 { get; private set; }
        public Guid GoldenStatue { get; private set; }
        public Guid StuffedEagle { get; private set; }

        public void NewWorld(ExampleGameSourceData gameData)
        {
            Rat1 = GameState.CurrentGameState.AddCharacter(new Rat("Joe the rat", 7, 23) { MaxAttack = 10, CounterAttackPercent = 15 }, gameData.EgLocations.Start);
            Rat2 = GameState.CurrentGameState.AddCharacter(new Rat("Henry the rat", 8, 15) { MaxAttack = 12, CounterAttackPercent = 17 }, gameData.EgLocations.BanquetHall);
            GoldenStatue = GameState.CurrentGameState.AddCharacter(new Character("Goldie Plink", 15) { MaxAttack = 1, CounterAttackPercent = 0 }, gameData.EgLocations.Start);
            StuffedEagle = GameState.CurrentGameState.AddCharacter(new Character("Eagle Eyes", 15) { MaxAttack = 1, CounterAttackPercent = 0 }, gameData.EgLocations.Start);

            // Custom turn behaviors
            GameState.CurrentGameState.AddTurnBehavior(CustomTurnBehavior.Squeak, new SqueakTurnBehavior());
        }
    }
}
