using NetworkUtils;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ServerEngine
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Client : TrackableInstance
    {
        /// <summary>
        /// The account that this client is currently logged into
        /// </summary>
        [JsonIgnore]
        public Account AttachedAccount { get; set; }

        [JsonIgnore]
        public TcpClientHelper ClientHelper { get; set; }

        public void SendMessage(string text, bool newLine = true) // TODO: remove option for newlines
        {
            if (ClientHelper != null)
            {
                ClientHelper?.SendMessage(text);
            }
            else
            {
                // TODO: how do we keep line returns between descriptive text paragraphs and still remove all line returns for json blobs sent?

                // TODO: keeping for now until the server/client are fully separated.
                if (newLine)
                {
                    System.Console.WriteLine(text.AddLineReturns(true));
                }
                else
                {
                    System.Console.Write(text);
                }
            }
        }

        /// <summary>
        /// Sends a blank line
        /// </summary>
        public void SendMessage()
        {
            SendMessage(string.Empty);
        }

        /// <summary>
        /// Writes out a list of items and lets the user choose 1 of them.
        /// </summary>
        /// <param name="choices">All the available choices</param>
        /// <param name="prompt">The text to display above the choices</param>
        /// <param name="includeCancel">Include the cancel option</param>
        /// <returns>The chosen item</returns>
        public string Choose(string prompt, List<string> choices, bool includeCancel)
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
        public T Choose<T>(string prompt, Dictionary<T, string> choices, bool includeCancel)
        {
            SendMessage(prompt);

            Dictionary<int, T> numberedKeys = new Dictionary<int, T>();
            int itemIndex = 1;
            foreach (var choice in choices)
            {
                SendMessage(string.Empty);
                SendMessage($"{itemIndex}. {choice.Value}");
                numberedKeys.Add(itemIndex++, choice.Key);
            }
            if (includeCancel)
            {
                SendMessage(string.Empty);
                SendMessage($"{itemIndex}. Cancel");
            }

            SendMessage("-----------------------------------------");

            while (true)
            {
                SendMessage("> ", false);
                if (int.TryParse(Console.ReadLine(), out int userChoiceIndex)) // TODO: The choose functionality needs to be re-written as a client ability
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
                SendMessage("Please pick one of the numbers above.");
            }
        }
    }
}
