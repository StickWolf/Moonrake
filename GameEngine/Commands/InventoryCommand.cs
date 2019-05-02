using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class InventoryCommand : ICommand
    {
        public void Exceute(List<string> extraWords)
        {
            var inventorySeekingCharacter = GameState.CurrentGameState.GetPlayerCharacter();
            var charaterItems = GameState.CurrentGameState.GetCharacterItems(inventorySeekingCharacter.TrackingId);
            if(charaterItems == null || !charaterItems.Any())
            {
                Console.WriteLine("You have no items.");
                return;
            }

            Console.WriteLine("You are currently holding:");
            foreach (var characterItem in charaterItems)
            {
                var item = characterItem.Key;
                // Don't show invisible items
                if (!item.IsVisible)
                {
                    continue;
                }

                var description = item.GetDescription(characterItem.Value).UppercaseFirstChar();
                Console.WriteLine(description);
            }
        }

        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "inv", "inventory" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
