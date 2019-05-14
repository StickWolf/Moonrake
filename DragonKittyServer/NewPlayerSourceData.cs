using ServerEngine;
using ServerEngine.Characters;
using ServerEngine.Characters.Behaviors;
using System.Collections.Generic;

namespace DragonKittyServer
{
    public class NewPlayerSourceData
    {
        public Character NewPlayer()
        {
            var gameData = GameState.CurrentGameState.Custom as DragonKittySourceData;

            var playerCharacter = new Character("James", 50)
            {
                TurnBehaviors = new List<string>() { BuiltInTurnBehaviors.FocusedPlayer },
                MaxAttack = 10,
                CounterAttackPercent = 50
            };

            GameState.CurrentGameState.TryAddCharacterItemCount(playerCharacter.TrackingId, gameData.DkItems.Money, 200);
            GameState.CurrentGameState.AddCharacter(playerCharacter, gameData.DkLocations.PlayersRoom);
            return playerCharacter;
        }
    }
}
