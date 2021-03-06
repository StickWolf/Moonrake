﻿using ServerEngine;
using ServerEngine.Characters;
using Newtonsoft.Json;
using System;
using ServerEngine.GrainSiloAndClient;

namespace DragonKittyServer.DragonKittyStrangeItems
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
            var character = GrainClusterClient.Universe.GetCharacter(WalletOwnerCharacterTrackingId).Result;
            return $"{character.Name}'s wallet";
        }

        public override void Grab(int count, Character grabbingCharacter)
        {
            var walletOwnerCharacter = GrainClusterClient.Universe.GetCharacter(WalletOwnerCharacterTrackingId).Result;
            if (walletOwnerCharacter.IsDead())
            {
                grabbingCharacter.SendDescriptiveTextDtoMessage($"Since {walletOwnerCharacter.Name} is dead, you get {MoneyWalletContains} dollars!");

                var gameData = (GrainClusterClient.Universe.GetCustom().Result as DragonKittySourceData);
                GrainClusterClient.Universe.TryAddCharacterItemCount(grabbingCharacter.TrackingId, gameData.DkItems.Money, MoneyWalletContains);
                GrainClusterClient.Universe.TryAddLocationItemCount(grabbingCharacter.GetLocation().TrackingId, this.TrackingId, -1);
            }
            else
            {
                grabbingCharacter.SendDescriptiveTextDtoMessage($"You have tried to steal {walletOwnerCharacter.Name}'s wallet, now you will suffer.");
                grabbingCharacter.Attack(walletOwnerCharacter);
                grabbingCharacter.SendDescriptiveTextDtoMessage($"{walletOwnerCharacter.Name} has hit you.");
            }
        }
    }
}
