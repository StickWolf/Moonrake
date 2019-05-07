using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class GrabCommand : ICommand
    {
        public void Execute(List<string> extraWords, Guid grabbingCharacterTrackingId)
        {
            var grabbingCharacter = GameState.CurrentGameState.GetCharacter(grabbingCharacterTrackingId);
            var grabbingCharacterLocation = GameState.CurrentGameState.GetCharacterLocation(grabbingCharacter.TrackingId);
            var locationItems = GameState.CurrentGameState.GetLocationItems(grabbingCharacterLocation.TrackingId);
            if (locationItems == null || locationItems.Count == 0)
            {
                grabbingCharacter.SendMessage("There is nothing to grab.");
                grabbingCharacterLocation.SendMessage($"{grabbingCharacter.Name} is looking around for something.", grabbingCharacter.TrackingId);
                return;
            }

            var availableItems = locationItems
                .Where(i => !i.Key.IsBound && i.Key.IsVisible) // Filter out bound and invisible items because these cannot be picked up
                .Select(i => new KeyValuePair<Item, string>(
                                 i.Key,
                                 i.Key.GetDescription(i.Value).UppercaseFirstChar()
                                 ))
                .ToDictionary(i => i.Key, i => i.Value);

            if (!availableItems.Any())
            {
                grabbingCharacter.SendMessage("There is nothing that can be grabbed.");
                grabbingCharacterLocation.SendMessage($"{grabbingCharacter.Name} is looking around for something.", grabbingCharacter.TrackingId);
                return;
            }

            // Try to auto-determine what the player is trying to grab
            var wordItemMap = CommandHelper.WordsToItems(extraWords, availableItems.Keys.ToList());
            var foundItems = wordItemMap
                .Where(i => i.Value != null)
                .Select(i => i.Value)
                .ToList();
            Item itemToGrab;
            if (foundItems.Count > 0)
            {
                itemToGrab = foundItems[0];
            }
            else
            {
                itemToGrab = Console.Choose("What do you want to pick up?", availableItems, includeCancel: true);
                if (itemToGrab == null)
                {
                    grabbingCharacter.SendMessage("Canceled Grab");
                    grabbingCharacterLocation.SendMessage($"{grabbingCharacter.Name} looks indecisive.", grabbingCharacter.TrackingId);
                    return;
                }
            }

            var itemAmount = locationItems[itemToGrab];
            itemToGrab.Grab(itemAmount, grabbingCharacter.TrackingId);
        }

        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "grab", "take" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
