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
                .Where(i => i.Item1 != null && i.Item1.IsInteractable && i.Item1.IsVisible) // Only choose items that are interactable and visible
                .Select(i => new KeyValuePair<string, string>(
                                 i.Item1.TrackingName,
                                 i.Item1.GetDescription(i.Item2, GameState.CurrentGameState).UppercaseFirstChar()
                                 ));

            var interactableCharacterItems = characterItems
                .Select(i => new Tuple<Item, int>(engine.GameData.GetItem(i.Key), i.Value))
                .Where(i => i.Item1 != null && i.Item1.IsInteractable && i.Item1.IsVisible) // Only choose items that are interactable and visible
                .Select(i => new KeyValuePair<string, string>(
                                 i.Item1.TrackingName,
                                 i.Item1.GetDescription(i.Item2, GameState.CurrentGameState).UppercaseFirstChar()
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
            var itemToInteractWith = Console.Choose("What do you want to use?", allInteractableItems);
            if (itemToInteractWith == "CancelChoice")
            {
                Console.WriteLine("Canceled interaction");
                return;
            }
            var item = engine.GameData.GetItem(itemToInteractWith);
            allInteractableItems.Remove(itemToInteractWith); // Remove the item being used

            while (true)
            {
                // If only the cancel item is left then auto-choose 'No'
                string answer = "N";
                if (allInteractableItems.Count != 1)
                {
                    Console.Write($"Do you want use the {item.DisplayName} on another item? (Yes, No or Cancel): ");
                    answer = Console.ReadKey().KeyChar.ToString();
                }

                Console.WriteLine();
                if (answer.Equals("C", StringComparison.OrdinalIgnoreCase))
                {
                    // Cancel
                    return;
                }
                else if (answer.Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    // Interact directly with the item
                    item.Interact(GameState.CurrentGameState, null);
                    return;
                }
                else if (answer.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    // Prompt for other item to interact with
                    
                    var secondItemToInteractWith = Console.Choose($"What do you want to use the {item.DisplayName} on?", allInteractableItems);
                    if (secondItemToInteractWith == "CancelChoice")
                    {
                        Console.WriteLine("Canceled interaction");
                        return;
                    }
                    var secondItem = engine.GameData.GetItem(secondItemToInteractWith);
                    secondItem.Interact(GameState.CurrentGameState, item.TrackingName);
                    return;
                }
                else
                {
                    Console.WriteLine($"Unknown response: {answer}");
                }
            }
        }

        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "use", "interact" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
