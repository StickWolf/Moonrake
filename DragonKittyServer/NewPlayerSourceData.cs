using ServerEngine;
using ServerEngine.Characters;
using System;

namespace DragonKittyServer
{
    public class NewPlayerSourceData
    {
        public Character NewPlayer()
        {
            var gameData = GameState.CurrentGameState.Custom as DragonKittySourceData;

            var playerCharacter = new Character("James", 50)
            {
                MaxAttack = 10,
                CounterAttackPercent = 50,
                TurnCooldown = TimeSpan.FromSeconds(5)
            };

            GameState.CurrentGameState.TryAddCharacterItemCount(playerCharacter.TrackingId, gameData.DkItems.Money, 200);
            GameState.CurrentGameState.AddCharacter(playerCharacter, gameData.DkLocations.PlayersRoom);
            return playerCharacter;
        }
    }
}
