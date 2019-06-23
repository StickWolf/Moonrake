using ServerEngine;
using ServerEngine.Characters;
using ServerEngine.GrainSiloAndClient;
using System;

namespace Moonrake
{
    public class NewPlayerSourceData
    {
        public Character NewPlayer()
        {
            var gameData = GrainClusterClient.Universe.GetCustom().Result as MoonrakeGameData;

            var playerCharacter = new Character("Player", 100)
            {
                TurnCooldown = TimeSpan.FromSeconds(5)
            };

            GrainClusterClient.Universe.AddCharacter(playerCharacter, gameData.MoonRakeLocations.TreeHouse).Wait();

            return playerCharacter;
        }
    }
}
