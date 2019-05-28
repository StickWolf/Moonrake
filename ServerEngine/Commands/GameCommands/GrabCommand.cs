using ServerEngine.Characters;
using System.Collections.Generic;
using System.Linq;

namespace ServerEngine.Commands.GameCommands
{
    public class GrabCommand : IGameCommand
    {
        public List<string> ActivatingWords => new List<string>() { "grab", "take" };

        public string PermissionNeeded => null;

        public void Execute(List<string> extraWords, Character grabbingCharacter)
        {
            var grabbingCharacterLocation = GameState.CurrentGameState.GetCharacterLocation(grabbingCharacter.TrackingId);
            var locationItems = GameState.CurrentGameState.GetLocationItems(grabbingCharacterLocation.TrackingId);
            if (locationItems == null || locationItems.Count == 0)
            {
                grabbingCharacter.SendDescriptiveTextDtoMessage("There is nothing to grab.");
                grabbingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{grabbingCharacter.Name} is looking around for something.", grabbingCharacter);
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
                grabbingCharacter.SendDescriptiveTextDtoMessage("There is nothing that can be grabbed.");
                grabbingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{grabbingCharacter.Name} is looking around for something.", grabbingCharacter);
                return;
            }

            // Try to auto-determine what the player is trying to grab
            var wordItemMap = WordTranslator.WordsToItems(extraWords, availableItems.Keys.ToList());
            var foundItems = wordItemMap
                .Where(i => i.Value != null)
                .Select(i => i.Value)
                .ToList();
            Item itemToGrab;
            if (foundItems.Count == 0)
            {
                grabbingCharacter.SendDescriptiveTextDtoMessage("There is nothing like that here.");
                grabbingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{grabbingCharacter.Name} is looking around for something.", grabbingCharacter);
                return;
            }
            itemToGrab = foundItems[0];

            var itemAmountToGrab = locationItems[itemToGrab];
            if (itemAmountToGrab > 1)
            {
                var leftWords = wordItemMap.Where(i => i.Value == null).Select(i => i.Key).ToList();
                var wordNumberMap = WordTranslator.WordsToNumbers(leftWords);
                var foundNumbers = wordNumberMap.Where(i => i.Value.HasValue).Select(i => i.Value.Value).ToList();
                if (foundNumbers.Count > 0)
                {
                    itemAmountToGrab = foundNumbers[0];
                }

                if (itemAmountToGrab <= 0)
                {
                    return;
                }
                if (itemAmountToGrab > locationItems[itemToGrab])
                {
                    itemAmountToGrab = locationItems[itemToGrab];
                }
            }

            itemToGrab.Grab(itemAmountToGrab, grabbingCharacter);
        }
    }
}
