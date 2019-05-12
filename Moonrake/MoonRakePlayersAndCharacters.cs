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

        public Guid Player { get; private set; }

        public void NewGame(MoonrakeGameData gameData)
        {
            Player = GameState.CurrentGameState.AddCharacter(new Character("Player", 100)
            {
                TurnBehaviors = new List<string>() { BuiltInTurnBehaviors.FocusedPlayer },
            }, gameData.MoonRakeLocations.TreeHouse);

            Paige = GameState.CurrentGameState.AddCharacter(new Character("Paige", 100), gameData.MoonRakeLocations.TreeHouse);
        }
    }
}
