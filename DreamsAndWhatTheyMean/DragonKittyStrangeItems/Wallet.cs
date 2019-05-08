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
            IsUnique = false;
            IsVisible = true;
        }

        public override string GetDescription(int count)
        {
            var character = GameState.CurrentGameState.GetCharacter(CharacterTrackingId);
            return $"{character.Name}'s wallet";
        }

        public override void Grab(int count, Character grabbingCharacter)
        {
            var playerCharacter = GameState.CurrentGameState.GetPlayerCharacter();
            var attackingCharacter = GameState.CurrentGameState.GetCharacter(CharacterTrackingId);
            if (attackingCharacter.HitPoints > 0)
            {
                grabbingCharacter.SendMessage($"You have tried to steal {attackingCharacter.Name}'s wallet, now you will suffer,");
                playerCharacter.Attack(attackingCharacter);
                grabbingCharacter.SendMessage($"{attackingCharacter.Name} has hit you.");
            }
            if (attackingCharacter.HitPoints <= 0)
            {
                grabbingCharacter.SendMessage($"Since {attackingCharacter.Name} is dead, you get {MoneyWalletContains} dollars!");
                GameState.CurrentGameState.TryAddCharacterItemCount(playerCharacter.TrackingId, DollarItemTrackingId, MoneyWalletContains);
                var locationOfWallet = GameState.CurrentGameState.GetCharacterLocation(CharacterTrackingId);
                GameState.CurrentGameState.TryAddLocationItemCount(locationOfWallet.TrackingId, this.TrackingId, -1);
            }
        }
    }
}
