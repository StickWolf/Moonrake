using GameEngine;
using GameEngine.Characters;
using GameEngine.Characters.Behaviors;
using System;
using System.Collections.Generic;

namespace DreamsAndWhatTheyMean
{
    class DragonKittyCharacters
    {
        public Guid MomCharacter { get; private set; }
        public Guid DadCharacter { get; private set; }
        public Guid BlackSmithCharacter { get; private set; }
        public Guid HealingDroneInPlayersHouse { get; private set; }

        public void NewWorld(TheTaleOfTheDragonKittySourceData gameData)
        {
            MomCharacter = GameState.CurrentGameState.AddCharacter(new Character("Mom", 4000) { MaxAttack = 150, CounterAttackPercent = 20 }, gameData.DkLocations.PlayersLivingRoom);
            DadCharacter = GameState.CurrentGameState.AddCharacter(new Character("Dad", 5000) { MaxAttack = 250, CounterAttackPercent = 30 }, gameData.DkLocations.PlayersBackyard);
            BlackSmithCharacter = GameState.CurrentGameState.AddCharacter(new Character("The Black-Smith", 10000) { MaxAttack = 700, CounterAttackPercent = 40 }, gameData.DkLocations.BlackSmithShop);

            HealingDroneInPlayersHouse = GameState.CurrentGameState.AddCharacter(new Character("Drone", 20)
            {
                TurnBehaviors = new List<string>() { "TurnBehaviorRandomHeal" },
                MaxAttack = 10,
                CounterAttackPercent = 10,
                MaxHeal = 200
            }, gameData.DkLocations.PlayersLivingRoom);
        }
    }
}
