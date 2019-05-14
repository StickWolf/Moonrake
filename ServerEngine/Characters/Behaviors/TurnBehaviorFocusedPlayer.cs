using ServerEngine.Commands.Internal;
using ServerEngine.Commands.Public;
using System.Collections.Generic;

namespace ServerEngine.Characters.Behaviors
{
    public class TurnBehaviorFocusedPlayer : ITurnBehavior
    {
        public bool HasPromps => true;

        public void Turn(Character turnTakingCharacter)
        {
            string input;
            turnTakingCharacter.SendMessage(">", false);
            input = Console.ReadLine();
            turnTakingCharacter.SendMessage();
            var extraWords = new List<string>(input.Split(' '));
            var word = extraWords[0];
            extraWords.RemoveAt(0);

            // Look for server commands to run
            if (InternalCommandHelper.TryRunServerCommand(word, extraWords, turnTakingCharacter.GetClient()))
            {
                return;
            }

            // Look for internal commands to run
            if (InternalCommandHelper.TryRunInternalCommand(word, extraWords, turnTakingCharacter))
            {
                return;
            }

            // Then look for public commands to run
            if (PublicCommandHelper.TryRunPublicCommand(word, extraWords, turnTakingCharacter))
            {
                return;
            }

            turnTakingCharacter.SendMessage($"I don't know what you mean by '{word}'.");
        }
    }
}
