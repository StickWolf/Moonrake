using ServerEngine.Characters;
using System;
using System.Collections.Generic;

namespace ServerEngine.Commands.GameCommands
{
    internal class ChangePlayerNameCommand : IGameCommand
    {
        public List<string> ActivatingWords => new List<string>() { "changeplayername" };

        public string PermissionNeeded => null;

        public void Execute(List<string> extraWords, Character nameChangingCharacter)
        {
            if (extraWords.Count != 1)
            {
                return;
            }

            nameChangingCharacter.Name = extraWords[0];
            nameChangingCharacter.SendDescriptiveTextDtoMessage($"Your new name is {nameChangingCharacter.Name}.");
        }
    }
}
