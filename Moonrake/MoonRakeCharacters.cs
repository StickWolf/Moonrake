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
            Player = gameData.AddCharacter(new GameEngine.Character("Player", 50, 100, gameData));
        }
    }
}
