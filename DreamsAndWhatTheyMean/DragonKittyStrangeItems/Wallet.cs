using GameEngine;
using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamsAndWhatTheyMean.DragonKittyStrangeItems
{
    class Wallet : Item
    {
        public Guid CharacterTrackingId { get; set; }
        public GameSourceData GameData { get; set; }
        public int MoneyWalletContains { get; set; }
        public Wallet(Guid characterTrackingId, GameSourceData gameData, int moneyInWallet) : base($"{characterTrackingId}sWallet", $"{characterTrackingId}'s wallet")
        {
            GameData = gameData;
            CharacterTrackingId = characterTrackingId;
            MoneyWalletContains = moneyInWallet;
            IsBound = false;
            IsInteractable = false;
            IsUnique = false;
            IsVisible = true;
        }

        public override string GetDescription(int count, GameState gameState)
        {
            var character = gameState.GetCharacter(CharacterTrackingId);
            return $"{character.Name}'s wallet";
        }

        public override void Grab(int count, Guid grabbingCharacterTrackingId, GameState gameState)
        {
            var playerCharacter = gameState.GetPlayerCharacter();
            var attackingCharacter = gameState.GetCharacter(CharacterTrackingId);
            if (attackingCharacter.Hp > 0)
            {
                GameEngine.Console.WriteLine($"You have tried to steal {CharacterTrackingId}'s wallet, now you will suffer,");
                playerCharacter.Attack(attackingCharacter, GameData);
                GameEngine.Console.WriteLine($"{CharacterTrackingId} has hit you.");
            }
            if (attackingCharacter.Hp <= 0)
            {
                GameEngine.Console.WriteLine($"Since {CharacterTrackingId} is dead, you get {MoneyWalletContains} dollars!");
                gameState.TryAddCharacterItemCount(playerCharacter.TrackingId, "Dollar", MoneyWalletContains, GameData);
                var locationOfWallet = GameState.CurrentGameState.GetCharacterLocation(CharacterTrackingId);
                gameState.TryAddLocationItemCount(locationOfWallet.TrackingId, TrackingName, -1, GameData);
            }
        }
    }
}
