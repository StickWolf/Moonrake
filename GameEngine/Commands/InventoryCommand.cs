using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class InventoryCommand : ICommand
    {
        public void Execute(List<string> extraWords, Guid inventorySeekingCharacterTrackingId)
        {
            var inventorySeekingCharacter = GameState.CurrentGameState.GetCharacter(inventorySeekingCharacterTrackingId);
            var inventorySeekingCharacterLocation = GameState.CurrentGameState.GetCharacterLocation(inventorySeekingCharacterTrackingId);
            var charaterItems = GameState.CurrentGameState.GetCharacterItems(inventorySeekingCharacter.TrackingId);
            if(charaterItems == null || !charaterItems.Any())
            {
                inventorySeekingCharacter.SendMessage("You have no items.");
                inventorySeekingCharacterLocation.SendMessage($"{inventorySeekingCharacter.Name} is looking at their inventory.", inventorySeekingCharacter.TrackingId);
                return;
            }

            inventorySeekingCharacter.SendMessage("You are currently holding:");
            foreach (var characterItem in charaterItems)
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
