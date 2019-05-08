using GameEngine.Characters;
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

        public static void WriteLine(string text)
        {
            System.Console.WriteLine(text.AddLineReturns(true));
        }

        public static void Clear()
        {
            System.Console.Clear();
        }

        public static ConsoleKeyInfo ReadKey() // TODO: make sure calls to readkey and readline are no-ops if they are for a NPC
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
        public static string Choose(string prompt, List<string> choices, Character choosingCharacter, bool includeCancel)
        {
            Dictionary<string, string> choicesDictionary = new Dictionary<string, string>();
            foreach (var choice in choices)
            {
                choicesDictionary.Add(choice, choice);
            }
            return Choose(prompt, choicesDictionary, choosingCharacter, includeCancel);
        }

        /// <summary>
        /// Writes out a list of items and lets the user choose 1 of them.
        /// </summary>
        /// <param name="choices">All the available choices</param>
        /// <param name="prompt">The text to display above the choices</param>
        /// <returns>The chosen key or default(T) if cancelled</returns>
        public static T Choose<T>(string prompt, Dictionary<T, string> choices, Character choosingCharacter, bool includeCancel)
        {
            choosingCharacter.SendMessage(prompt);

            Dictionary<int, T> numberedKeys = new Dictionary<int, T>();
            int itemIndex = 1;
            foreach (var choice in choices)
            {
                choosingCharacter.SendMessage(string.Empty);
                choosingCharacter.SendMessage($"{itemIndex}. {choice.Value}");
                numberedKeys.Add(itemIndex++, choice.Key);
            }
            if (includeCancel)
            {
                choosingCharacter.SendMessage(string.Empty);
                choosingCharacter.SendMessage($"{itemIndex}. Cancel");
            }

            choosingCharacter.SendMessage("-----------------------------------------");

            while (true)
            {
                choosingCharacter.SendMessage("> ", false);
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
                choosingCharacter.SendMessage("Please pick one of the numbers above.");
            }
        }
    }
}
