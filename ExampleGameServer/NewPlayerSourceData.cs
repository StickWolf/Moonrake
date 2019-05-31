using ServerEngine;
using ServerEngine.Characters;
using System;

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
                CounterAttackPercent = 75,
                TurnCooldown = TimeSpan.FromSeconds(5)
            };

            GameState.CurrentGameState.AddCharacter(playerCharacter, gameData.EgLocations.Start);

            return playerCharacter;
        }
    }
}
