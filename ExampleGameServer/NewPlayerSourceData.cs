using ServerEngine;
using ServerEngine.Characters;
using ServerEngine.Characters.Behaviors;
using System.Collections.Generic;

namespace ExampleGameServer
{
    public class NewPlayerSourceData
    {
        public Character NewPlayer()
        {
            var gameData = GameState.CurrentGameState.Custom as ExampleGameSourceData;

            var playerCharacter = new Character("Sally", 50)
            {
                MaxAttack = 40,
                CounterAttackPercent = 75
            };

            GameState.CurrentGameState.AddCharacter(playerCharacter, gameData.EgLocations.Start);

            return playerCharacter;
        }
    }
}
