using GameEngine;
using GameEngine.Characters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamsAndWhatTheyMean.DragonKittyStrangeItems
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class Wallet : Item
    {
        [JsonProperty]
        private Guid CharacterTrackingId { get; set; }

        [JsonProperty]
        private Guid DollarItemTrackingId { get; set; }

        [JsonProperty]
        private int MoneyWalletContains { get; set; }

        public Wallet(Guid characterTrackingId, Guid dollarItemTrackingId, int moneyInWallet) : base($"{characterTrackingId}'s wallet")
        {
            CharacterTrackingId = characterTrackingId;
            DollarItemTrackingId = dollarItemTrackingId;
            MoneyWalletContains = moneyInWallet;
            IsBound = false;
            IsInteractable = false;
            IsUnique = false;
            IsVisible = true;
        }

        public override string GetDescription(int count)
        {
            var character = GameState.CurrentGameState.GetCharacter(CharacterTrackingId);
            return $"{character.Name}'s wallet";
        }

        public override void Grab(int count, Guid grabbingCharacterTrackingId)
        {
            var playerCharacter = GameState.CurrentGameState.GetPlayerCharacter();
            var attackingCharacter = GameState.CurrentGameState.GetCharacter(CharacterTrackingId);
            if (attackingCharacter.HitPoints > 0)
            {
                GameEngine.Console.WriteLine($"You have tried to steal {CharacterTrackingId}'s wallet, now you will suffer,"); // TODO: this probably doesn't say the right thing now
                playerCharacter.Attack(attackingCharacter);
                GameEngine.Console.WriteLine($"{CharacterTrackingId} has hit you."); // TODO: this probably doesn't say the right thing now
            }
            if (attackingCharacter.HitPoints <= 0)
            {
                GameEngine.Console.WriteLine($"Since {CharacterTrackingId} is dead, you get {MoneyWalletContains} dollars!");
                GameState.CurrentGameState.TryAddCharacterItemCount(playerCharacter.TrackingId, DollarItemTrackingId, MoneyWalletContains);
                var locationOfWallet = GameState.CurrentGameState.GetCharacterLocation(CharacterTrackingId);
                GameState.CurrentGameState.TryAddLocationItemCount(locationOfWallet.TrackingId, this.TrackingId, -1);
            }
        }
    }
}
