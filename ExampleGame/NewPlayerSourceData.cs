using GameEngine;
using GameEngine.Characters;
using GameEngine.Characters.Behaviors;
using System.Collections.Generic;

namespace ExampleGame
{
    public class NewPlayerSourceData
    {
        public Character NewPlayer()
        {
            var gameData = GameState.CurrentGameState.Custom as ExampleGameSourceData;

            var playerCharacter = new Character("Sally", 50)
            {
                TurnBehaviors = new List<string>() { BuiltInTurnBehaviors.FocusedPlayer },
                MaxAttack = 40,
                CounterAttackPercent = 75
            };

            GameState.CurrentGameState.AddCharacter(playerCharacter, gameData.EgLocations.Start);

            return playerCharacter;
        }
    }
}
