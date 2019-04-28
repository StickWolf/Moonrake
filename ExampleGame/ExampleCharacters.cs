using ExampleGame.Characters;
using GameEngine;
using GameEngine.Characters;

namespace ExampleGame
{
    public class ExampleCharacters
    {
        public string Player { get; private set; }

        public string Rat1 { get; private set; }
        public string Rat2 { get; private set; }

        public ExampleCharacters(ExampleGameSourceData gameData)
        {
            Player = gameData.AddCharacter(new PlayerCharacter(50) { MaxAttack = 40, CounterAttackChance = 75 });
            Rat1 = gameData.AddCharacter(new Rat("Rat1", 7, 23) { MaxAttack = 10, CounterAttackChance = 15 });
            Rat2 = gameData.AddCharacter(new Rat("Rat2", 8, 15) { MaxAttack = 12, CounterAttackChance = 17 });

            // Default character locations
            gameData.AddDefaultCharacterLocation(Player, gameData.Locations.Start);

            gameData.AddDefaultCharacterLocation(Rat1, gameData.Locations.Start);
            gameData.AddDefaultCharacterLocation(Rat2, gameData.Locations.BanquetHall);
        }
    }
}
