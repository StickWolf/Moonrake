using System;
using System.Text;

namespace GameEngine
{
    public static class StringExtensionMethods
    {
        public static int MaxLineLength { get; set; } = 80;

        /// <summary>
        /// Adds line returns to a paragraph at the specified max line length.
        /// Line returns will be added between works only.
        /// </summary>
        /// <param name="input">The string to add line returns to</param>
        /// <param name="removeExistingLineReturns">True to first remove existing line returns.</param>
        /// <returns></returns>
        public static string AddLineReturns(this string input, bool removeExistingLineReturns)
        {
            StringBuilder sb = new StringBuilder(input);

            if (removeExistingLineReturns)
            {
                sb.Replace("\r", "");
                sb.Replace("\n", "");
            }

            // The last place that looked like a line return could go there if it was needed
            int lastPotentialCrlfPos = -1;

            // The current position within the string that we are examining
            int currentPos = -1;

            // The current line length
            int currentLineLength = -1;

            while (true)
            {
                currentPos++;
                currentLineLength++;

                // If we reached the end of the string builder.
                if (currentPos >= sb.Length)
                {
                    break;
                }

                // If we have exceeded the max line length for this line
                if (currentLineLength >= MaxLineLength)
                {
                    // We can only place a line return if there is a potential place to put one
                    if (lastPotentialCrlfPos > 0)
                    {
                        // Insert the line return and reset the tracking variables
                        sb.Insert(lastPotentialCrlfPos, Environment.NewLine);
                        lastPotentialCrlfPos = -1;
                        currentLineLength = -1;

                        // Also the current position we are looking at is now greater because of the insert
                        currentPos += Environment.NewLine.Length;
                        continue;
                    }
                }

                // If we find a space then mark this as the last potential line return position
                // This will keep getting updated for each space we find.
                if (sb[currentPos].Equals(' '))
                {
                    lastPotentialCrlfPos = currentPos + 1;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a string with the first character set to uppercase.
        /// </summary>
        /// <param name="input">The string to modify</param>
        /// <returns>a string with the first character set to uppercase.</returns>
        public static string UppercaseFirstChar(this string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length < 1)
            {
                return input;
            }

            return input[0].ToString().ToUpper() + input.Substring(1);
        }
    }
}
