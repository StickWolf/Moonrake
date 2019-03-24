using System;
using System.Collections.Generic;

namespace GameEngine
{
    /// <summary>
    /// Specialized console for writing out game text
    /// </summary>
    internal static class Console
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
        /// <returns>The chosen item</returns>
        public static string Choose(string prompt, List<string> choices)
        {
            WriteLine(prompt);

            Dictionary<int, string> numberedChoices = new Dictionary<int, string>();
            int itemIndex = 1;
            foreach (var item in choices)
            {
                WriteLine();
                WriteLine($"{itemIndex}. {item}");
                numberedChoices.Add(itemIndex++, item);
            }
            WriteLine("-----------------------------------------");

            while (true)
            {
                Write("> ");
                if (int.TryParse(ReadLine(), out int userChoiceIndex) && userChoiceIndex >= 1 && userChoiceIndex <= itemIndex)
                {
                    return numberedChoices[userChoiceIndex];
                }
                WriteLine("Please pick one of the numbers above.");
            }
        }
    }
}
