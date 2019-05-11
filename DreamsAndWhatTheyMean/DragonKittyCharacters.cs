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
            Player1 = GameState.CurrentGameState.AddCharacter(new PlayerCharacter("James", 50) { MaxAttack = 10, CounterAttackPercent = 50 }, gameData.DkLocations.JamesRoom);
            Player2 = GameState.CurrentGameState.AddCharacter(new PlayerCharacter("Owen", 60) { MaxAttack = 15, CounterAttackPercent = 60 }, gameData.DkLocations.JamesLivingRoom);
            Player3 = GameState.CurrentGameState.AddCharacter(new PlayerCharacter("Matthew", 50) { MaxAttack = 10, CounterAttackPercent = 100 }, gameData.DkLocations.BlackSmithShop);
            Player4 = GameState.CurrentGameState.AddCharacter(new PlayerCharacter("Lily", 30) { MaxAttack = 5, CounterAttackPercent = 20 }, gameData.DkLocations.LilysSecretRoom);
        }
    }
}
