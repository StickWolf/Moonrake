using GameEngine;
using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamsAndWhatTheyMean.DragonKittyStrangeItems
{
    class BronzeTalisman : Item
    {
        public GameSourceData GameData { get; set; }
        public BronzeTalisman(GameSourceData gameData) : base("BronzeTalisman", "bronze talisman")
        {
            GameData = gameData;
            IsUnique = false;
            IsVisible = true;
            IsInteractable = true;
            IsBound = false;
        }

        public override void Interact(GameState gameState, string otherItemTrackingName)
        {
            Interacting(gameState, otherItemTrackingName, GameData);
        }

        public void Interacting(GameState gameState, string otherItemTrackingName, GameSourceData gameData)
        {
            var player = gameState.GetPlayerCharacter();
            var playersItems = GameState.CurrentGameState.GetCharacterItems(player.TrackingId);
            var playersItemsNames = playersItems.Keys;
            if (playersItemsNames.Contains("BronzeTalisman"))
            {
                player.MaxAttack = player.MaxAttack + 20;
                player.FullHp = player.FullHp + 30;
                GameEngine.Console.WriteLine("You put the talisman on, it vanishes and you feel stronger.");
                GameState.CurrentGameState.TryAddCharacterItemCount(player.TrackingId, "BronzeTalisman", -1, gameData);
            }
            else
            {
                GameEngine.Console.WriteLine("The talisman is not in your inventory, try picking it up first.");
            }
        }
    }
}
