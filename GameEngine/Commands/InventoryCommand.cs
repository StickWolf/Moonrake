using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class InventoryCommand : ICommand
    {
        public void Execute(List<string> extraWords, Character inventorySeekingCharacter)
        {
            var characterItems = GameState.CurrentGameState.GetCharacterItems(inventorySeekingCharacter.TrackingId);
            if(characterItems == null || !characterItems.Any())
            {
                inventorySeekingCharacter.SendMessage("You have no items.");
                inventorySeekingCharacter.GetLocation().SendMessage($"{inventorySeekingCharacter.Name} is looking at their inventory.", inventorySeekingCharacter);
                return;
            }

            inventorySeekingCharacter.SendMessage("You are currently holding:");
            foreach (var characterItem in characterItems)
            {
                var item = characterItem.Key;
                // Don't show invisible items
                if (!item.IsVisible)
                {
                    continue;
                }

                var description = item.GetDescription(characterItem.Value).UppercaseFirstChar();
                inventorySeekingCharacter.SendMessage(description);
            }
        }

        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "inv", "inventory" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
