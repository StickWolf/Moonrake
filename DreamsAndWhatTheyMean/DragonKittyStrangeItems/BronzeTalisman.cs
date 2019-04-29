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
        public BronzeTalisman() : base("bronze talisman")
        {
            IsUnique = false;
            IsVisible = true;
            IsInteractable = true;
            IsBound = false;
        }

        public override void Interact(Item otherItem)
        {
            var player = GameState.CurrentGameState.GetPlayerCharacter();
            var playersItems = GameState.CurrentGameState.GetCharacterItems(player.TrackingId);

            var playersItemsNames = playersItems.Keys.Select(i => i.DisplayName);
            if (playersItemsNames.Contains("BronzeTalisman"))
            {
                player.MaxAttack = player.MaxAttack + 20;
                player.FullHp = player.FullHp + 30;
                GameEngine.Console.WriteLine("You put the talisman on, it vanishes and you feel stronger.");
                GameState.CurrentGameState.TryAddCharacterItemCount(player.TrackingId, this.TrackingId, -1);
            }
            else
            {
                GameEngine.Console.WriteLine("The talisman is not in your inventory, try picking it up first.");
            }
        }
    }
}
