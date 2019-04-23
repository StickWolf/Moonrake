using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal static class CommandHelper
    {
        private static List<ICommand> AllCommands { get; set; }

        /// <summary>
        /// If we see these words when parsing a sentence, we'll just skip over them instead of trying
        /// to match them to something.
        /// </summary>
        private static List<string> SkipWords { get; set; } = new List<string>() { "the", "on", "with", "in" };

        static CommandHelper()
        {
            AllCommands = new List<ICommand>()
            {
                new MoveCommand(),
                new SaveCommand(),
                new LoadCommand(),
                new ExitCommand(),
                new LetPlayerChangeTheirNameCommand(),
                new LookCommand(),
                new ClearCommand(),
                new InventoryCommand(),
                new GrabCommand(),
                new DropCommand(),
                new AttackCommand(),
                new StatsCommand(),
                new InteractCommand()
            };
        }

        public static ICommand GetCommand(string word)
        {
            var commandToRun = AllCommands.FirstOrDefault(c => c.IsActivatedBy(word));
            return commandToRun;
        }

        /// <summary>
        /// Attempts to create a list of words into a list of items.
        /// </summary>
        /// <param name="words">The words to convert</param>
        /// <param name="availableItemNames">The names of items that are possible</param>
        /// <param name="engine">Game engine</param>
        /// <returns>A list of items that were matched in the order they were found</returns>
        public static List<Item> WordsToItems(List<string> words, List<string> availableItemNames, EngineInternal engine)
        {
            // Get a list of actual items
            var availableItems = availableItemNames
                .Select(i => engine.GameData.GetItem(i))
                .ToList();

            // Create a list of words that we will try to map items to
            var wordItemMap = words
                .Except(SkipWords)
                .Select(w => new MutablePair<string, Item>(w.ToLower(), null))
                .ToList();

            bool itemsFound = true;
            while (itemsFound)
            {
                itemsFound = false;
                foreach (var wiMapping in wordItemMap)
                {
                    // If this word is already mapped to an item them skip it.
                    if (wiMapping.Value != null)
                    {
                        continue;
                    }

                    // Get a set of items that match this word
                    var wordItemMatches = availableItems
                        .Where(i => i.DisplayName.ToLower().Contains(wiMapping.Key))
                        .ToList();

                    // If there is just one item it matches then assign that word to
                    // that item and remove the item from the remaining choices
                    if (wordItemMatches.Count == 1)
                    {
                        itemsFound = true;
                        wiMapping.Value = wordItemMatches.First();
                        availableItems.Remove(wordItemMatches.First());
                    }
                }
            }

            var foundItems = wordItemMap
                .Where(i => i.Value != null)
                .Select(i => i.Value)
                .ToList();

            return foundItems;
        }
    }
}
