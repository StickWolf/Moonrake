using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    class InteractCommand : ICommand
    {
        public void Exceute(EngineInternal engine)
        {
            var playersLoc = GameState.CurrentGameState.GetCharacterLocation("Player");
            var locationItems = GameState.CurrentGameState.GetLocationItems(playersLoc) ?? new Dictionary<string, int>();
            var characterItems = GameState.CurrentGameState.GetCharacterItems("Player") ?? new Dictionary<string, int>();

            var interactableLocationItems = locationItems
                .Select(i => new Tuple<Item, int>(engine.GameData.GetItem(i.Key), i.Value))
                .Where(i => i.Item1 != null && i.Item1.IsInteractable)
                .Select(i => new KeyValuePair<string, string>(
                                 i.Item1.TrackingName,
                                 i.Item1.GetDescription(i.Item2, engine.GameData, GameState.CurrentGameState).UppercaseFirstChar()
                                 ));

            var interactableCharacterItems = characterItems
                .Select(i => new Tuple<Item, int>(engine.GameData.GetItem(i.Key), i.Value))
                .Where(i => i.Item1 != null && i.Item1.IsInteractable)
                .Select(i => new KeyValuePair<string, string>(
                                 i.Item1.TrackingName,
                                 i.Item1.GetDescription(i.Item2, engine.GameData, GameState.CurrentGameState).UppercaseFirstChar()
                                 ));

            var allInteractableItems = interactableLocationItems
                .Union(interactableCharacterItems)
                .ToDictionary(i => i.Key, i => i.Value); // This also removes duplicates

            if (!allInteractableItems.Any())
            {
                Console.WriteLine("There is nothing available to interact with.");
                return;
            }

            allInteractableItems.Add("CancelChoice", "Cancel");

            var itemToInteractWith = Console.Choose("What do you want to interact with?", allInteractableItems);

            if(itemToInteractWith == "CancelChoice")
            {
                Console.WriteLine("Canceled interaction");
                return;
            }

            var item = engine.GameData.GetItem(itemToInteractWith);
            item.Interact(engine.GameData, GameState.CurrentGameState);
        }

        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "use", "interact" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
