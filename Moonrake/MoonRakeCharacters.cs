using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonrake
{
    public class MoonRakeCharacters
    {
        public string Player { get; private set; }

        public MoonRakeCharacters(MoonrakeGameData gameData)
        {
            Player = gameData.AddCharacter(new PlayerCharacter(50) { MaxAttack = 100, CounterAttackChance = 75 });
        }
    }
}
