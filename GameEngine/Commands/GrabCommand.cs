using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class GrabCommand : ICommand
    {
        public void Exceute(EngineInternal engine, List<string> extraWords)
        {
            var grabbingCharacter = "Player";
            var characterLoc = GameState.CurrentGameState.GetCharacterLocation(grabbingCharacter);
            var locationItems = GameState.CurrentGameState.GetLocationItems(characterLoc);
            if (locationItems == null || locationItems.Count == 0)
            {
                Console.WriteLine("There is nothing to grab.");
                return;
            }

            var availableItems = locationItems
                .Select(i => new Tuple<Item, int>(engine.GameData.GetItem(i.Key), i.Value))
                .Where(i => i.Item1 != null && !i.Item1.IsBound && i.Item1.IsVisible) // Filter out bound and invisible items because these cannot be picked up
                .Select(i => new KeyValuePair<string, string>(
                                 i.Item1.TrackingName,
                                 i.Item1.GetDescription(i.Item2, GameState.CurrentGameState).UppercaseFirstChar()
                                 ))
                .ToDictionary(i => i.Key, i => i.Value);

            if (!availableItems.Any())
            {
                Console.WriteLine("There is nothing that can be grabed.");
                return;
            }

            availableItems.Add("CancelChoice", "Cancel");

            var itemToPickUp = Console.Choose("What do you want to pick up?", availableItems);

            if(itemToPickUp == "CancelChoice")
            {
                Console.WriteLine("Canceled Grab");
                return;
            }
            var itemAmount = locationItems[itemToPickUp];

            var item = engine.GameData.GetItem(itemToPickUp);
            item.Grab(itemAmount, grabbingCharacter, GameState.CurrentGameState);
        }

        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "grab", "take" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
