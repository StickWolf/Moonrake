using ServerEngine;
using ServerEngine.Characters;
using ServerEngine.GrainSiloAndClient;
using System;

namespace DragonKittyServer
{
    public class NewPlayerSourceData
    {
        public Character NewPlayer()
        {
            var gameData = GrainClusterClient.Universe.GetCustom().Result as DragonKittySourceData;

            var playerCharacter = new Character("James", 50)
            {
                MaxAttack = 10,
                CounterAttackPercent = 50,
                TurnCooldown = TimeSpan.FromSeconds(5)
            };

            GrainClusterClient.Universe.TryAddCharacterItemCount(playerCharacter.TrackingId, gameData.DkItems.Money, 200).Wait();
            GrainClusterClient.Universe.AddCharacter(playerCharacter, gameData.DkLocations.PlayersRoom).Wait();
            return playerCharacter;
        }
    }
}
