using GameEngine;
using GameEngine.Characters;
using System;

namespace Moonrake
{
    public class MoonRakeCharacters
    {
        public Guid Player { get; private set; }

        public void NewGame(MoonrakeGameData gameData)
        {
            Player = GameState.CurrentGameState.AddCharacter(new PlayerCharacter("Eric", 50) { MaxAttack = 100, CounterAttackChance = 75 }, gameData.MoonRakeLocations.TreeHouse);
        }
    }
}
