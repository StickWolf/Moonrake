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
        public Apple() : base("apple")
        {
            IsBound = false;
            IsInteractable = true;
            IsUnique = false;
            IsVisible = true;
        }

        public override void Interact(Item otherItem)
        {
            var player = GameState.CurrentGameState.GetPlayerCharacter();
            var playersItems = GameState.CurrentGameState.GetCharacterItems(player.TrackingId);

            var playersItemsNames = playersItems.Keys.Select(i => i.DisplayName);
            if (playersItemsNames.Contains("Apple"))
            {
                if (player.HitPoints == player.MaxHitPoints)
                {
                    GameEngine.Console.WriteLine("You can't eat the apple, you are at full health.");
                    return;
                }
                player.HitPoints = player.HitPoints + 20;
                if (player.HitPoints > player.MaxHitPoints)
                {
                    player.HitPoints = player.MaxHitPoints;
                }
                GameEngine.Console.WriteLine("You eat a apple, and you feel some of your health come back.");
                GameState.CurrentGameState.TryAddCharacterItemCount(player.TrackingId, this.TrackingId, -1);
            }
            else
            {
                GameEngine.Console.WriteLine("The apple is not in your inventory, try picking it up first.");
            }
        }
    }
}
