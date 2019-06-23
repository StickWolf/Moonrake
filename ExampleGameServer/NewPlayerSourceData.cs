using ServerEngine;
using ServerEngine.Characters;
using ServerEngine.GrainSiloAndClient;
using System;

namespace ExampleGameServer
{
    public class NewPlayerSourceData
    {
        public Character NewPlayer()
        {
            var gameData = GrainClusterClient.Universe.GetCustom().Result as ExampleGameSourceData;

            var playerCharacter = new Character("Sally", 50)
            {
                MaxAttack = 40,
                CounterAttackPercent = 75,
                TurnCooldown = TimeSpan.FromSeconds(5)
            };

            GrainClusterClient.Universe.AddCharacter(playerCharacter, gameData.EgLocations.Start).Wait();

            return playerCharacter;
        }
    }
}
