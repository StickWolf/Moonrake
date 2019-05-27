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
            // TODO: rewrite as server/client

            //nameChangingCharacter.SendMessage($"Your name is currently: {nameChangingCharacter.Name}.");
            //nameChangingCharacter.SendMessage($"What would you like your name to be?: ");
            //nameChangingCharacter.Name = Console.ReadLine();
            //nameChangingCharacter.SendMessage($"Your new name is {nameChangingCharacter.Name}.");
        }
    }
}
