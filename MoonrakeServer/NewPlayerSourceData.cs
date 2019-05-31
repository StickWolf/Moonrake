using ServerEngine;
using ServerEngine.Characters;
using System;

namespace Moonrake
{
    public class NewPlayerSourceData
    {
        public Character NewPlayer()
        {
            var gameData = GameState.CurrentGameState.Custom as MoonrakeGameData;

            var playerCharacter = new Character("Player", 100)
            {
                TurnCooldown = TimeSpan.FromSeconds(5)
            };

            GameState.CurrentGameState.AddCharacter(playerCharacter, gameData.MoonRakeLocations.TreeHouse);

            return playerCharacter;
        }
    }
}
