using GameEngine;
using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonrake
{
    public class MoonRakePlayersAndCharacters
    {
        public Guid Paige { get; private set; }

        public Guid Player { get; private set; }

        public void NewGame(MoonrakeGameData gameData)
        {
            Paige = GameState.CurrentGameState.AddCharacter(new Character("Paige", 100), gameData.MoonRakeLocations.TreeHouse);
            Player = GameState.CurrentGameState.AddCharacter(new PlayerCharacter("Player", 100), gameData.MoonRakeLocations.TreeHouse);
        }
    }
}
