using ServerEngine.Characters;
using ServerEngine.GrainSiloAndClient;
using System.Collections.Generic;
using System.Linq;

namespace ServerEngine.Commands.GameCommands
{
    public class DropCommand : IGameCommand
    {
        public List<string> ActivatingWords => new List<string>() { "drop" };

        public string PermissionNeeded => null;

        public void Execute(List<string> extraWords, Character droppingCharacter)
        {
            var droppingCharacterLocation = GrainClusterClient.Universe.GetCharacterLocation(droppingCharacter.TrackingId).Result;
            var droppingCharacterItems = GrainClusterClient.Universe.GetCharacterItems(droppingCharacter.TrackingId).Result;
            if (droppingCharacterItems == null || droppingCharacterItems.Count == 0)
            {
                droppingCharacter.SendDescriptiveTextDtoMessage("You have no items.");
                droppingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{droppingCharacter.Name} is happy that they have no items.", droppingCharacter);
                return;
            }

            var availableItems = droppingCharacterItems
                .Where(i => !i.Key.IsBound && i.Key.IsVisible) // Filter out bound and invisible items because these cannot be dropped
                .Select(i => new KeyValuePair<Item, string>(
                                 i.Key,
                                 i.Key.GetDescription(i.Value).UppercaseFirstChar()
                                 ))
                .ToDictionary(i => i.Key, i => i.Value);

            if (!availableItems.Any())
            {
                droppingCharacter.SendDescriptiveTextDtoMessage("You have nothing that can be dropped.");
                droppingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{droppingCharacter.Name} is digging around in their inventory looking for something.", droppingCharacter); // TODO: he/she/pronoun ?
                return;
            }

            // Try to auto-determine what the character is trying to drop
            var wordItemMap = WordTranslator.WordsToItems(extraWords, availableItems.Keys.ToList());
            var foundItems = wordItemMap
                .Where(i => i.Value != null)
                .Select(i => i.Value)
                .ToList();
            Item itemToDrop;
            if (foundItems.Count == 0)
            {
                droppingCharacter.SendDescriptiveTextDtoMessage("You don't have anything like that.");
                droppingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{droppingCharacter.Name} is digging around in their inventory looking for something.", droppingCharacter); // TODO: he/she/pronoun ?
                return;
            }
            itemToDrop = foundItems[0];

            var itemAmountToDrop = droppingCharacterItems[itemToDrop];
            if (itemAmountToDrop > 1)
            {
                var leftWords = wordItemMap.Where(i => i.Value == null).Select(i => i.Key).ToList();
                var wordNumberMap = WordTranslator.WordsToNumbers(leftWords);
                var foundNumbers = wordNumberMap.Where(i => i.Value.HasValue).Select(i => i.Value.Value).ToList();
                if (foundNumbers.Count > 0)
                {
                    itemAmountToDrop = foundNumbers[0];
                }

                if (itemAmountToDrop <= 0)
                {
                    return;
                }
                if (itemAmountToDrop > droppingCharacterItems[itemToDrop])
                {
                    itemAmountToDrop = droppingCharacterItems[itemToDrop];
                }
            }

            itemToDrop.Drop(itemAmountToDrop, droppingCharacter);
        }
    }
}
