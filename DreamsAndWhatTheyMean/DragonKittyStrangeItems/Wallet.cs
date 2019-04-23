using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamsAndWhatTheyMean.DragonKittyStrangeItems
{
    class Wallet : Item
    {
        public string CharacterName { get; set; }
        public GameSourceData GameData { get; set; }
        public int MoneyWalletContains { get; set; }
        public Wallet(string characterName, GameSourceData gameData, int moneyInWallet) : base($"{characterName}sWallet", $"{characterName}'s wallet")
        {
            GameData = gameData;
            CharacterName = characterName;
            MoneyWalletContains = moneyInWallet;
            IsBound = false;
            IsInteractable = false;
            IsUnique = false;
            IsVisible = true;
        }

        public override string GetDescription(int count, GameState gameState)
        {
            return $"{CharacterName}'s wallet";
        }

        public override void Grab(int count, string grabbingCharacterName, GameState gameState)
        {
            GameData.TryGetCharacter(CharacterName, out Character attackingCharacter);
            GameData.TryGetCharacter("Player", out Character playerCharacter);
            if (attackingCharacter.Hp > 0)
            {
                GameEngine.Console.WriteLine($"You have tried to steal {CharacterName}'s wallet, now you will suffer,");
                playerCharacter.Attack(attackingCharacter, GameData);
                GameEngine.Console.WriteLine($"{CharacterName} has hit you.");
            }
            if (attackingCharacter.Hp <= 0)
            {
                GameEngine.Console.WriteLine($"Since {CharacterName} is dead, you get {MoneyWalletContains} dollars!");
                gameState.TryAddCharacterItemCount("Player", "Dollar", MoneyWalletContains, GameData);
            }
        }
    }
}
