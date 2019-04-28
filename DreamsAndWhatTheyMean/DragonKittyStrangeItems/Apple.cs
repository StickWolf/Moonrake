using GameEngine;
using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamsAndWhatTheyMean.DragonKittyStrangeItems
{
    class Apple : Item
    {
        public GameSourceData GameData { get; set; }
        public Apple(GameSourceData gameData) : base("Apple", "apple")
        {
            GameData = gameData;
            IsBound = false;
            IsInteractable = true;
            IsUnique = false;
            IsVisible = true;
        }

        public override void Interact(GameState gameState, string otherItemTrackingName)
        {
            Interacting(gameState, otherItemTrackingName, GameData);
        }

        public void Interacting(GameState gameState, string otherItemTrackingName, GameSourceData gameData)
        {
            var playersItems = GameState.CurrentGameState.GetCharacterItems(PlayerCharacter.TrackingName);
            var playersItemsNames = playersItems.Keys;
            if (playersItemsNames.Contains("Apple"))
            {
                var player = gameState.GetCharacter(PlayerCharacter.TrackingName);
                if(player.Hp == player.FullHp)
                {
                    GameEngine.Console.WriteLine("You can't eat the apple, you are at full health.");
                    return;
                }
                player.Hp = player.Hp + 20;
                if(player.Hp > player.FullHp)
                {
                    player.Hp = player.FullHp;
                }
                GameEngine.Console.WriteLine("You eat a apple, and you feel some of your health come back.");
                GameState.CurrentGameState.TryAddCharacterItemCount(PlayerCharacter.TrackingName, "Apple", -1, gameData);
            }
            else
            {
                GameEngine.Console.WriteLine("The apple is not in your inventory, try picking it up first.");
            }
        }
    }
}
