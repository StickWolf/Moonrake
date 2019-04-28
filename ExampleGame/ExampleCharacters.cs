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
            // TODO: change this add method to also require a location, remove addDefaultCharacterLocations() from gamedata
            Player = gameData.AddCharacter(new PlayerCharacter("Sally", 50) { MaxAttack = 40, CounterAttackChance = 75 });
            Rat1 = gameData.AddCharacter(new Rat("Rat", 7, 23) { MaxAttack = 10, CounterAttackChance = 15 });
            Rat2 = gameData.AddCharacter(new Rat("Rat", 8, 15) { MaxAttack = 12, CounterAttackChance = 17 });

            // Default character locations
            gameData.AddDefaultCharacterLocation(Player, gameData.Locations.Start);

            gameData.AddDefaultCharacterLocation(Rat1, gameData.Locations.Start);
            gameData.AddDefaultCharacterLocation(Rat2, gameData.Locations.BanquetHall);
        }
    }
}
