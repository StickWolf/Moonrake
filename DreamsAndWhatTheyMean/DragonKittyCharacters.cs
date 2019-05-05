using DreamsAndWhatTheyMean.DragonKittyStrangeCharacters;
using GameEngine;
using GameEngine.Characters;
using System;

namespace DreamsAndWhatTheyMean
{
    class DragonKittyCharacters
    {
        public Guid Player { get; private set; }
        public Guid MomCharacter { get; private set; }
        public Guid DadCharacter { get; private set; }
        public Guid BlackSmithCharacter { get; private set; }
        public Guid HealingDroneInPlayersHouse { get; private set; }

        public DragonKittyCharacters(TheTaleOfTheDragonKittySourceData gameData)
        {
            Player = GameState.CurrentGameState.AddCharacter(new PlayerCharacter("James", 50) { MaxAttack = 10, CounterAttackChance = 50 }, gameData.DkLocations.PlayersRoom);
            MomCharacter = GameState.CurrentGameState.AddCharacter(new Character("Mom", 4000) { MaxAttack = 150, CounterAttackChance = 20 }, gameData.DkLocations.PlayersLivingRoom);
            DadCharacter = GameState.CurrentGameState.AddCharacter(new Character("Dad", 5000) { MaxAttack = 250, CounterAttackChance = 30 }, gameData.DkLocations.PlayersBackyard);
            BlackSmithCharacter = GameState.CurrentGameState.AddCharacter(new Character("The Black-Smith", 10000) { MaxAttack = 700, CounterAttackChance = 40 }, gameData.DkLocations.BlackSmithShop);
            HealingDroneInPlayersHouse = GameState.CurrentGameState.AddCharacter(new HealingDrone(gameData.DkLocations.PlayersLivingRoom), gameData.DkLocations.PlayersLivingRoom);
        }
    }
}
