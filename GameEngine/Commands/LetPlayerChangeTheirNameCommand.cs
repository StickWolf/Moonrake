using GameEngine.Characters;
using System;
using System.Collections.Generic;

namespace GameEngine.Commands
{
    internal class LetPlayerChangeTheirNameCommand : ICommandInternal
    {
        public void Execute(EngineInternal engine, List<string> extraWords, Character nameChangingCharacter)
        {
            nameChangingCharacter.SendMessage($"What would you like your name to be?: ");
            nameChangingCharacter.Name = Console.ReadLine();
            nameChangingCharacter.SendMessage($"Your new name is {nameChangingCharacter.Name}.");
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("namechange", StringComparison.OrdinalIgnoreCase);
        }
    }
}
