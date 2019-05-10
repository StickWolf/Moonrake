using DreamsAndWhatTheyMean.DragonKittyStrangeCharacters;
using GameEngine;
using GameEngine.Characters;
using System;

namespace DreamsAndWhatTheyMean
{
    class DragonKittyCharacters
    {
        public Guid Player1 { get; private set; }
        public Guid Player2 { get; private set; }
        public Guid Player3 { get; private set; }
        public Guid Player4 { get; private set; }
        public Guid MomCharacter { get; private set; }
        public Guid DadCharacter { get; private set; }
        public Guid BlackSmithCharacter { get; private set; }
        public Guid HealingDroneInPlayersHouse { get; private set; }

        public void NewGame(TheTaleOfTheDragonKittySourceData gameData)
        {
            Player1 = GameState.CurrentGameState.AddCharacter(new PlayerCharacter("James", 50) { MaxAttack = 10, CounterAttackChance = 50 }, gameData.DkLocations.PlayersRoom);
            Player2 = GameState.CurrentGameState.AddCharacter(new PlayerCharacter("Owen", 60) { MaxAttack = 15, CounterAttackChance = 60 }, gameData.DkLocations.PlayersLivingRoom);
            Player3 = GameState.CurrentGameState.AddCharacter(new PlayerCharacter("Matthew", 50) { MaxAttack = 10, CounterAttackChance = 100 }, gameData.DkLocations.BlackSmithShop);
            Player4 = GameState.CurrentGameState.AddCharacter(new PlayerCharacter("Lily", 30) { MaxAttack = 5, CounterAttackChance = 20 }, gameData.DkLocations.BRStreet);
            MomCharacter = GameState.CurrentGameState.AddCharacter(new Character("Mom", 4000) { MaxAttack = 150, CounterAttackChance = 20 }, gameData.DkLocations.PlayersLivingRoom);
            DadCharacter = GameState.CurrentGameState.AddCharacter(new Character("Dad", 5000) { MaxAttack = 250, CounterAttackChance = 30 }, gameData.DkLocations.PlayersBackyard);
            BlackSmithCharacter = GameState.CurrentGameState.AddCharacter(new Character("The Black-Smith", 10000) { MaxAttack = 700, CounterAttackChance = 40 }, gameData.DkLocations.BlackSmithShop);
            HealingDroneInPlayersHouse = GameState.CurrentGameState.AddCharacter(new HealingDrone(gameData.DkLocations.PlayersLivingRoom), gameData.DkLocations.PlayersLivingRoom);
        }
    }
}
