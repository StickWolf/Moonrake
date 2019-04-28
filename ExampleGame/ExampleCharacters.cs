using GameEngine;
using GameEngine.Characters;

namespace ExampleGame
{
    public class ExampleCharacters
    {
        public string Player { get; private set; } 

        public ExampleCharacters(ExampleGameSourceData gameData)
        {
            Player = gameData.AddCharacter(new PlayerCharacter(50) { MaxAttack = 40, CounterAttackChance = 75 });

            // Default character locations
            gameData.AddDefaultCharacterLocation(Player, gameData.Locations.Start);
        }
    }
}
