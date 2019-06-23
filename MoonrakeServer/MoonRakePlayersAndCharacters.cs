using ServerEngine;
using ServerEngine.Characters;
using ServerEngine.Characters.Behaviors;
using ServerEngine.GrainSiloAndClient;
using System;
using System.Collections.Generic;

namespace Moonrake
{
    public class MoonRakePlayersAndCharacters
    {
        public Guid Paige { get; private set; }

        public void NewWorld(MoonrakeGameData gameData)
        {
            Paige = GrainClusterClient.Universe.AddCharacter(new Character("Paige", 100), gameData.MoonRakeLocations.TreeHouse).Result;
        }
    }
}
