using GameEngine;
using GameEngine.Characters;
using GameEngine.Characters.Behaviors;
using System.Collections.Generic;

namespace Moonrake
{
    public class NewPlayerSourceData
    {
        public Character NewPlayer()
        {
            var gameData = GameState.CurrentGameState.Custom as MoonrakeGameData;

            var playerCharacter = new Character("Player", 100)
            {
                TurnBehaviors = new List<string>() { BuiltInTurnBehaviors.FocusedPlayer },
            };

            GameState.CurrentGameState.AddCharacter(playerCharacter, gameData.MoonRakeLocations.TreeHouse);

            return playerCharacter;
        }
    }
}
