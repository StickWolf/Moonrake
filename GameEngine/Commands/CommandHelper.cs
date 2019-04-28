using GameEngine.Characters;
using GameEngine.Locations;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Commands
{
    internal static class CommandHelper
    {
        private static List<ICommand> AllPublicCommands { get; set; }
        private static List<ICommandInternal> AllInternalCommands { get; set; }

        /// <summary>
        /// If we see these words when parsing a sentence, we'll just skip over them instead of trying
        /// to match them to something.
        /// </summary>
        private static List<string> SkipWords { get; set; } = new List<string>() { "the", "on", "with", "in" };

        static CommandHelper()
        {
            // Public commands do not get passed the engine internal when they are called
            // Public commands are available to the player and npcs
            AllPublicCommands = new List<ICommand>()
            {
                new MoveCommand(),
                new LookCommand(),
                new InventoryCommand(),
                new GrabCommand(),
                new DropCommand(),
                new AttackCommand(),
                new StatsCommand(),
                new InteractCommand()
            };

            // Internal commands gain access to the EngineInternal when they are executed
            // The intention is that internal commands are only available to the player
            AllInternalCommands = new List<ICommandInternal>()
            {
                new ClearCommand(),
                new LetPlayerChangeTheirNameCommand(),
                new LoadCommand(),
                new SaveCommand(),
                new ExitCommand(),
            };
        }

        /// <summary>
        /// Runs a public command
        /// </summary>
        /// <param name="word">The command word</param>
        /// <param name="extraWords">Extra words to pass along to the command</param>
        /// <param name="gameData">The game source data</param>
        /// <returns>True if the command was found and ran, false if the command was not found</returns>
        public static bool TryRunPublicCommand(string word, List<string> extraWords, GameSourceData gameData)
        {
            var commandToRun = AllPublicCommands.FirstOrDefault(c => c.IsActivatedBy(word));
            if (commandToRun == null)
            {
                return false;
            }

            // The command is a real command if we got this far
            commandToRun.Exceute(gameData, extraWords);
            return true;
        }

        /// <summary>
        /// Runs a public command
        /// </summary>
        /// <param name="word">The command word</param>
        /// <param name="extraWords">Extra words to pass along to the command</param>
        /// <param name="gameData">The game source data</param>
        /// <returns>True if the command was found and ran, false if the command was not found</returns>
        internal static bool TryRunInternalCommand(string word, List<string> extraWords, EngineInternal engine)
        {
            var commandToRun = AllInternalCommands.FirstOrDefault(c => c.IsActivatedBy(word));
            if (commandToRun == null)
            {
                return false;
            }

            // The command is a real command if we got this far
            commandToRun.Exceute(engine, extraWords);
            return true;
        }

        /// <summary>
        /// Attempts to create a list of words into a list of items.
        /// </summary>
        /// <param name="words">The words to convert</param>
        /// <param name="availableItemNames">The names of items that are possible</param>
        /// <param name="engine">Game engine</param>
        /// <returns>A list of items that were matched in the order they were found</returns>
        public static List<MutablePair<string, Item>> WordsToItems(List<string> words, List<string> availableItemNames, GameSourceData gameData)
        {
            // Get a list of actual items
            var availableItems = availableItemNames
                .Select(i => gameData.GetItem(i))
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

            return wordItemMap;
        }

        /// <summary>
        /// Looks at a list of words, converts to numbers where possible.
        /// </summary>
        /// <param name="words">The words to convert</param>
        /// <returns>A list of words with their related number if conversion was possible</returns>
        public static List<MutablePair<string, int?>> WordsToNumbers(List<string> words)
        {
            var wordNumberMap = words
                .Select(w => new MutablePair<string, int?>(w.ToLower(), null))
                .ToList();

            foreach (var mapping in wordNumberMap)
            {
                if (int.TryParse(mapping.Key, out int result))
                {
                    mapping.Value = result;
                }
            }

            return wordNumberMap;
        }

        /// <summary>
        /// Attempts to create a list of words into a list of locations.
        /// </summary>
        /// <param name="words">The words to convert</param>
        /// <param name="availableLocationNames">The names of locations that are possible</param>
        /// <param name="engine">Game engine</param>
        /// <returns>A list of locations that were matched in the order they were found</returns>
        public static List<MutablePair<string, Location>> WordsToLocations(List<string> words, List<string> availableLocationNames, GameSourceData gameData)
        {
            // Get a list of actual locations
            var availableLocations = availableLocationNames
                .Select(l => gameData.GetLocation(l))
                .ToList();

            // Create a list of words that we will try to map locations to
            var wordLocationMap = words
                .Except(SkipWords)
                .Select(w => new MutablePair<string, Location>(w.ToLower(), null))
                .ToList();

            bool locationsFound = true;
            while (locationsFound)
            {
                locationsFound = false;
                foreach (var wlMapping in wordLocationMap)
                {
                    // If this word is already mapped to a location them skip it.
                    if (wlMapping.Value != null)
                    {
                        continue;
                    }

                    // Get a set of locations that match this word
                    var wordLocationMatches = availableLocations
                        .Where(i => i.Name.ToLower().Contains(wlMapping.Key))
                        .ToList();

                    // If there is just one location it matches then assign that word to
                    // that location and remove the location from the remaining choices
                    if (wordLocationMatches.Count == 1)
                    {
                        locationsFound = true;
                        wlMapping.Value = wordLocationMatches.First();
                        availableLocations.Remove(wordLocationMatches.First());
                    }
                }
            }

            return wordLocationMap;
        }

        /// <summary>
        /// Attempts to create a list of words into a list of characters.
        /// </summary>
        /// <param name="words">The words to convert</param>
        /// <param name="availableCharacterNames">The names of character names that are possible</param>
        /// <param name="engine">Game engine</param>
        /// <returns>A list of characters that were matched in the order they were found</returns>
        public static List<MutablePair<string, Character>> WordsToCharacters(List<string> words, List<string> availableCharacterNames, GameSourceData gameData)
        {
            // Get a list of actual characters
            var availableCharacters = availableCharacterNames
                .Select(c => GameState.CurrentGameState.GetCharacter(c))
                .ToList();

            // Create a list of words that we will try to map characters to
            var wordCharacterMap = words
                .Except(SkipWords)
                .Select(w => new MutablePair<string, Character>(w.ToLower(), null))
                .ToList();

            bool charactersFound = true;
            while (charactersFound)
            {
                charactersFound = false;
                foreach (var wcMapping in wordCharacterMap)
                {
                    // If this word is already mapped to a character them skip it.
                    if (wcMapping.Value != null)
                    {
                        continue;
                    }

                    // Get a set of characters that match this word
                    var wordCharacterMatches = availableCharacters
                        .Where(i => i.Name.ToLower().Contains(wcMapping.Key))
                        .ToList();

                    // If there is just one character it matches then assign that word to
                    // that character and remove the character from the remaining choices
                    if (wordCharacterMatches.Count == 1)
                    {
                        charactersFound = true;
                        wcMapping.Value = wordCharacterMatches.First();
                        availableCharacters.Remove(wordCharacterMatches.First());
                    }
                }
            }

            return wordCharacterMap;
        }
    }
}
