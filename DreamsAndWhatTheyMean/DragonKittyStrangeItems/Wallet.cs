using GameEngine;
using GameEngine.Characters;
using Newtonsoft.Json;
using System;

namespace DreamsAndWhatTheyMean.DragonKittyStrangeItems
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class Wallet : Item
    {
        [JsonProperty]
        private Guid WalletOwnerCharacterTrackingId { get; set; }

        [JsonProperty]
        private int MoneyWalletContains { get; set; }

        public Wallet(Guid walletOwnerCharacterTrackingId, int moneyInWallet) : base($"{walletOwnerCharacterTrackingId}'s wallet")
        {
            WalletOwnerCharacterTrackingId = walletOwnerCharacterTrackingId;
            MoneyWalletContains = moneyInWallet;
            IsBound = false;
            IsUnique = false;
            IsVisible = true;
        }

        public override string GetDescription(int count)
        {
            var character = GameState.CurrentGameState.GetCharacter(WalletOwnerCharacterTrackingId);
            return $"{character.Name}'s wallet";
        }

        public override void Grab(int count, Character grabbingCharacter)
        {
            var walletOwnerCharacter = GameState.CurrentGameState.GetCharacter(WalletOwnerCharacterTrackingId);
            if (walletOwnerCharacter.IsDead())
            {
                grabbingCharacter.SendMessage($"Since {walletOwnerCharacter.Name} is dead, you get {MoneyWalletContains} dollars!");

                var gameData = (GameState.CurrentGameState.Custom as TheTaleOfTheDragonKittySourceData);
                GameState.CurrentGameState.TryAddCharacterItemCount(grabbingCharacter.TrackingId, gameData.DkItems.Money, MoneyWalletContains);
                GameState.CurrentGameState.TryAddLocationItemCount(grabbingCharacter.GetLocation().TrackingId, this.TrackingId, -1);
            }
            else
            {
                grabbingCharacter.SendMessage($"You have tried to steal {walletOwnerCharacter.Name}'s wallet, now you will suffer.");
                grabbingCharacter.Attack(walletOwnerCharacter);
                grabbingCharacter.SendMessage($"{walletOwnerCharacter.Name} has hit you.");
            }
        }
    }
}
