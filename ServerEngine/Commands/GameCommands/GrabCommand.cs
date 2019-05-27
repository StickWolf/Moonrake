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
            // TODO: rewrite as server/client

            //var grabbingCharacterLocation = GameState.CurrentGameState.GetCharacterLocation(grabbingCharacter.TrackingId);
            //var locationItems = GameState.CurrentGameState.GetLocationItems(grabbingCharacterLocation.TrackingId);
            //if (locationItems == null || locationItems.Count == 0)
            //{
            //    grabbingCharacter.SendMessage("There is nothing to grab.");
            //    grabbingCharacter.GetLocation().SendMessage($"{grabbingCharacter.Name} is looking around for something.", grabbingCharacter);
            //    return;
            //}

            //var availableItems = locationItems
            //    .Where(i => !i.Key.IsBound && i.Key.IsVisible) // Filter out bound and invisible items because these cannot be picked up
            //    .Select(i => new KeyValuePair<Item, string>(
            //                     i.Key,
            //                     i.Key.GetDescription(i.Value).UppercaseFirstChar()
            //                     ))
            //    .ToDictionary(i => i.Key, i => i.Value);

            //if (!availableItems.Any())
            //{
            //    grabbingCharacter.SendMessage("There is nothing that can be grabbed.");
            //    grabbingCharacter.GetLocation().SendMessage($"{grabbingCharacter.Name} is looking around for something.", grabbingCharacter);
            //    return;
            //}

            //// Try to auto-determine what the player is trying to grab
            //var wordItemMap = PublicCommandHelper.WordsToItems(extraWords, availableItems.Keys.ToList());
            //var foundItems = wordItemMap
            //    .Where(i => i.Value != null)
            //    .Select(i => i.Value)
            //    .ToList();
            //Item itemToGrab;
            //if (foundItems.Count > 0)
            //{
            //    itemToGrab = foundItems[0];
            //}
            //// Don't prompt NPCs who are running actions
            //else if (!grabbingCharacter.HasPromptingBehaviors())
            //{
            //    return;
            //}
            //else
            //{
            //    itemToGrab = grabbingCharacter.Choose("What do you want to pick up?", availableItems, includeCancel: true);
            //    if (itemToGrab == null)
            //    {
            //        grabbingCharacter.SendMessage("Canceled Grab");
            //        grabbingCharacter.GetLocation().SendMessage($"{grabbingCharacter.Name} looks indecisive.", grabbingCharacter);
            //        return;
            //    }
            //}

            //var itemAmountToGrab = locationItems[itemToGrab];
            //if (itemAmountToGrab > 1)
            //{
            //    var leftWords = wordItemMap.Where(i => i.Value == null).Select(i => i.Key).ToList();
            //    var wordNumberMap = PublicCommandHelper.WordsToNumbers(leftWords);
            //    var foundNumbers = wordNumberMap.Where(i => i.Value.HasValue).Select(i => i.Value.Value).ToList();
            //    if (foundNumbers.Count > 0)
            //    {
            //        itemAmountToGrab = foundNumbers[0];
            //    }
            //    // Don't prompt NPCs who are running actions
            //    else if (!grabbingCharacter.HasPromptingBehaviors())
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        grabbingCharacter.SendMessage("How many do you want to grab?");
            //        itemAmountToGrab = int.Parse(Console.ReadLine());
            //    }

            //    if (itemAmountToGrab <= 0)
            //    {
            //        return;
            //    }
            //    if (itemAmountToGrab > locationItems[itemToGrab])
            //    {
            //        itemAmountToGrab = locationItems[itemToGrab];
            //    }
            //}

            //itemToGrab.Grab(itemAmountToGrab, grabbingCharacter);
        }
    }
}
