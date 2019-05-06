using System;
using System.Collections.Generic;

namespace GameEngine
{
    /// <summary>
    /// Specialized console for writing out game text
    /// </summary>
    public static class Console
    {
        public static void Write(string text)
        {
            System.Console.Write(text);
        }

        public static void WriteLine()
        {
            System.Console.WriteLine();
        }

        public static void WriteLine(string text)
        {
            System.Console.WriteLine(text.AddLineReturns(true));
        }

        /// <summary>
        /// Writes text that is visible only to those at the location
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="locationTrackingId">The location to write the text at</param>
        public static void LocationWriteLine(string text, Guid locationTrackingId)
        {
            // Since this is a single player game, really we only need to worry about if the player sees the text or not
            var playerCharacter = GameState.CurrentGameState.GetPlayerCharacter();
            var playerLocation = GameState.CurrentGameState.GetCharacterLocation(playerCharacter.TrackingId);
            if (playerLocation.TrackingId == locationTrackingId)
            {
                WriteLine(text);
            }
            else
            {
                // TODO: add an admin command that lets you see these "Inaudible" messages
                //var targetLocation = GameState.CurrentGameState.GetLocation(locationTrackingId);
                //WriteLine($"{{Inaudible}} at \"{targetLocation.LocationName}\" : {text}");
            }
        }

        /// <summary>
        /// Writes text as though the specified character is speaking it.
        /// The text will only be visible if the character and player are in the same location.
        /// Text does not need to be in 1st person.
        /// </summary>
        /// <param name="text">The text to write</param>
        /// <param name="characterTrackingId">The "speaking" character</param>
        public static void CharacterLocationWriteLine(string text, Guid characterTrackingId)
        {
            // Figure out the location where the specified character is
            var characterLocation = GameState.CurrentGameState.GetCharacterLocation(characterTrackingId);
            LocationWriteLine(text, characterLocation.TrackingId);
        }

        public static void Clear()
        {
            System.Console.Clear();
        }

        public static ConsoleKeyInfo ReadKey()
        {
            return System.Console.ReadKey();
        }

        public static string ReadLine()
        {
            return System.Console.ReadLine();
        }

        /// <summary>
        /// Writes out a list of items and lets the user choose 1 of them.
        /// </summary>
        /// <param name="choices">All the available choices</param>
        /// <param name="prompt">The text to display above the choices</param>
        /// <param name="includeCancel">Include the cancel option</param>
        /// <returns>The chosen item</returns>
        public static string Choose(string prompt, List<string> choices, bool includeCancel)
        {
            Dictionary<string, string> choicesDictionary = new Dictionary<string, string>();
            foreach (var choice in choices)
            {
                choicesDictionary.Add(choice, choice);
            }
            return Choose(prompt, choicesDictionary, includeCancel);
        }

        /// <summary>
        /// Writes out a list of items and lets the user choose 1 of them.
        /// </summary>
        /// <param name="choices">All the available choices</param>
        /// <param name="prompt">The text to display above the choices</param>
        /// <returns>The chosen key or default(T) if cancelled</returns>
        public static T Choose<T>(string prompt, Dictionary<T, string> choices, bool includeCancel)
        {
            WriteLine(prompt);

            Dictionary<int, T> numberedKeys = new Dictionary<int, T>();
            int itemIndex = 1;
            foreach (var choice in choices)
            {
                WriteLine();
                WriteLine($"{itemIndex}. {choice.Value}");
                numberedKeys.Add(itemIndex++, choice.Key);
            }
            if (includeCancel)
            {
                WriteLine();
                WriteLine($"{itemIndex}. Cancel");
            }

            WriteLine("-----------------------------------------");

            while (true)
            {
                Write("> ");
                if (int.TryParse(ReadLine(), out int userChoiceIndex))
                {
                    if (userChoiceIndex >= 1 && userChoiceIndex < itemIndex)
                    {
                        return numberedKeys[userChoiceIndex];
                    }
                    else if (userChoiceIndex == itemIndex)
                    {
                        return default(T);
                    }
                }
                WriteLine("Please pick one of the numbers above.");
            }
        }
    }
}
