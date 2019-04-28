using GameEngine.Characters;
using System;

namespace Moonrake
{
    public class MoonRakeCharacters
    {
        public Guid Player { get; private set; }

        public MoonRakeCharacters(MoonrakeGameData gameData)
        {
            Player = gameData.AddCharacter(new PlayerCharacter("Eric", 50) { MaxAttack = 100, CounterAttackChance = 75 });
        }
    }
}
