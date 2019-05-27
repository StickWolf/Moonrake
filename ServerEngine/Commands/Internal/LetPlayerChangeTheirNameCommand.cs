using ServerEngine.Characters;
using System;
using System.Collections.Generic;

namespace ServerEngine.Commands.Internal
{
    internal class LetPlayerChangeTheirNameCommand : ICommandInternal
    {
        public List<string> ActivatingWords => new List<string>() { "namechange" };

        public void Execute(List<string> extraWords, Character nameChangingCharacter)
        {
            // TODO: rewrite as server/client

            //nameChangingCharacter.SendMessage($"Your name is currently: {nameChangingCharacter.Name}.");
            //nameChangingCharacter.SendMessage($"What would you like your name to be?: ");
            //nameChangingCharacter.Name = Console.ReadLine();
            //nameChangingCharacter.SendMessage($"Your new name is {nameChangingCharacter.Name}.");
        }
    }
}
