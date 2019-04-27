using GameEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal class InventoryCommand : ICommand
    {
        public void Exceute(EngineInternal engine, List<string> extraWords)
        {
            var charaterItems = GameState.CurrentGameState.GetCharacterItems(PlayerCharacter.TrackingName);
            // TODO: make the player character name constant or global.
            if(charaterItems == null || !charaterItems.Any())
            {
                Console.WriteLine("You have no items.");
                return;
            }

            Console.WriteLine("You are currently holding:");
            foreach (var characterItem in charaterItems)
            {
                if (engine.GameData.TryGetItem(characterItem.Key, out Item item))
                {
                    // Don't show invisible items
                    if (!item.IsVisible)
                    {
                        continue;
                    }

                    var description = item.GetDescription(characterItem.Value, GameState.CurrentGameState).UppercaseFirstChar();
                    Console.WriteLine(description);
                }
            }
        }

        public bool IsActivatedBy(string word)
        {
            var activators = new List<string>() { "inv", "inventory" };
            return activators.Any(a => a.Equals(word, StringComparison.OrdinalIgnoreCase));
        }
    }
}
