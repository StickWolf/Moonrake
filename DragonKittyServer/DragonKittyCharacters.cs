using ServerEngine.Characters;
using ServerEngine.GrainSiloAndClient;
using System;
using System.Collections.Generic;

namespace DragonKittyServer
{
    class DragonKittyCharacters
    {
        public Guid MomCharacter { get; private set; }
        public Guid DadCharacter { get; private set; }
        public Guid BlackSmithCharacter { get; private set; }
        public Guid HealingDroneInPlayersHouse { get; private set; }

        public void NewWorld(DragonKittySourceData gameData)
        {
            MomCharacter = GrainClusterClient.Universe.AddCharacter(new Character("Mom", 4000) { MaxAttack = 150, CounterAttackPercent = 20 }, gameData.DkLocations.PlayersLivingRoom).Result;
            DadCharacter = GrainClusterClient.Universe.AddCharacter(new Character("Dad", 5000) { MaxAttack = 250, CounterAttackPercent = 30 }, gameData.DkLocations.PlayersBackyard).Result;
            BlackSmithCharacter = GrainClusterClient.Universe.AddCharacter(new Character("The Black-Smith", 10000) { MaxAttack = 700, CounterAttackPercent = 40 }, gameData.DkLocations.BlackSmithShop).Result;

            HealingDroneInPlayersHouse = GrainClusterClient.Universe.AddCharacter(new Character("Drone", 20)
            {
                TurnBehaviors = new List<string>() { "TurnBehaviorRandomHeal" },
                MaxAttack = 10,
                CounterAttackPercent = 10,
                MaxHeal = 200
            }, gameData.DkLocations.PlayersLivingRoom).Result;
        }
    }
}
