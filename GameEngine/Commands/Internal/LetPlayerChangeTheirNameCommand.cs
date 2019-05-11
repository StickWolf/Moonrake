using GameEngine.Characters;
using System;
using System.Collections.Generic;

namespace GameEngine.Commands.Internal
{
    internal class LetPlayerChangeTheirNameCommand : ICommandInternal
    {
        public List<string> ActivatingWords => new List<string>() { "namechange" };

        public void Execute(EngineInternal engine, List<string> extraWords, Character nameChangingCharacter)
        {
            nameChangingCharacter.SendMessage($"What would you like your name to be?: ");
            nameChangingCharacter.Name = Console.ReadLine();
            nameChangingCharacter.SendMessage($"Your new name is {nameChangingCharacter.Name}.");
        }
    }
}
