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
        private List<Guid> CharacterId = new List<Guid>();

        [JsonProperty]
        private Guid DollarItemTrackingId { get; set; }

        [JsonProperty]
        private int MoneyWalletContains { get; set; }

        public Wallet(Guid characterTrackingId, Guid dollarItemTrackingId, int moneyInWallet) : base($"{characterTrackingId}'s wallet")
        {
            CharacterTrackingId = characterTrackingId;
            DollarItemTrackingId = dollarItemTrackingId;
            MoneyWalletContains = moneyInWallet;
            Id.Add(CharacterTrackingId);
            IsBound = false;
            IsUnique = false;
            IsVisible = true;
        }

        public override string GetDescription(int count)
        {
            var character = GameState.CurrentGameState.GetCharacters(Id);
            return $"{character[0].Name}'s wallet";
        }

        public override void Grab(int count, Character grabbingCharacter)
        {
            var playerCharacter = GameState.CurrentGameState.GetPlayerCharacters();
            var attackingCharacter = GameState.CurrentGameState.GetCharacters(Id);
            if (attackingCharacter[0].HitPoints > 0)
            {
                grabbingCharacter.SendMessage($"You can't take {attackingCharacter[0].Name}'s wallet.");
            }
        }
    }
}
