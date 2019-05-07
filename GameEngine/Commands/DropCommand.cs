using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class DropCommand : ICommand
    {
        public void Execute(List<string> extraWords, Guid droppingCharacterTrackingId)
        {
            var droppingCharacter = GameState.CurrentGameState.GetCharacter(droppingCharacterTrackingId);
            var droppingCharacterLocation = GameState.CurrentGameState.GetCharacterLocation(droppingCharacter.TrackingId);
            var droppingCharacterItems = GameState.CurrentGameState.GetCharacterItems(droppingCharacter.TrackingId);
            if (droppingCharacterItems == null || droppingCharacterItems.Count == 0)
            {
                droppingCharacter.SendMessage("You have no items.");
                droppingCharacter.SendMessage($"{droppingCharacter.Name} is happy that they have no items.");
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
                droppingCharacter.SendMessage("You have nothing that can be dropped.");
                droppingCharacter.SendMessage($"{droppingCharacter.Name} is digging around in their inventory looking for something."); // TODO: he/she/pronoun ?
                return;
            }

            // Try to auto-determine what the character is trying to drop
            var wordItemMap = CommandHelper.WordsToItems(extraWords, availableItems.Keys.ToList());
            var foundItems = wordItemMap
                .Where(i => i.Value != null)
                .Select(i => i.Value)
                .ToList();
            Item itemToDrop;
            if (foundItems.Count > 0)
            {
                itemToDrop = foundItems[0];
            }
            else
            {

                // TODO: upgrade all commands to fully process the command if extra text is provided or fail out and never prompt when extra text is present

                // TODO: also upgrade all commands to only go into prompt mode if it is the player who is executing the command

                // TODO: add a special parsing ability to sentence parsing where if we see a guid appear that it auto-converts to the item/thing represented
                // TODO: npcs would use this mode to assure the right thing happened

                itemToDrop = Console.Choose("What do you want to drop?", availableItems, includeCancel: true);
                if (itemToDrop == null)
                {
                    droppingCharacter.SendMessage("Canceled Drop");
                    droppingCharacter.SendMessage($"{droppingCharacter.Name} looks indecisive.");
                    return;
                }
            }

            var itemAmountToDrop = droppingCharacterItems[itemToDrop];
            if (itemAmountToDrop > 1)
            {
                var leftWords = wordItemMap.Where(i => i.Value == null).Select(i => i.Key).ToList();
                var wordNumberMap = CommandHelper.WordsToNumbers(leftWords);
                var foundNumbers = wordNumberMap.Where(i => i.Value.HasValue).Select(i => i.Value.Value).ToList();
                if (foundNumbers.Count > 0)
                {
                    itemAmountToDrop = foundNumbers[0];
                }
                else
                {
                    droppingCharacter.SendMessage("How many do you want to drop?");
                    itemAmountToDrop = int.Parse(Console.ReadLine());
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

            itemToDrop.Drop(itemAmountToDrop, droppingCharacter.TrackingId);
        }

        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "drop" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
