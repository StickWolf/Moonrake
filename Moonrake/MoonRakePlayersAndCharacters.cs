using GameEngine;
using GameEngine.Characters;
using GameEngine.Characters.Behaviors;
using System;
using System.Collections.Generic;

namespace Moonrake
{
    public class MoonRakePlayersAndCharacters
    {
        public Guid Paige { get; private set; }

        public void NewWorld(MoonrakeGameData gameData)
        {
            Paige = GameState.CurrentGameState.AddCharacter(new Character("Paige", 100), gameData.MoonRakeLocations.TreeHouse);
        }
    }
}
