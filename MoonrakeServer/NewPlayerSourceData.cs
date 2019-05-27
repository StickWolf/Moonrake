using ServerEngine;
using ServerEngine.Characters;
using ServerEngine.Characters.Behaviors;
using System.Collections.Generic;

namespace Moonrake
{
    public class NewPlayerSourceData
    {
        public Character NewPlayer()
        {
            var gameData = GameState.CurrentGameState.Custom as MoonrakeGameData;

            var playerCharacter = new Character("Player", 100);

            GameState.CurrentGameState.AddCharacter(playerCharacter, gameData.MoonRakeLocations.TreeHouse);

            return playerCharacter;
        }
    }
}
