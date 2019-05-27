using ServerEngine.Characters;
using ServerEngine.Commands.GameCommands;
using System.Collections.Generic;

namespace ExampleGameServer.Commands
{
    public class DanceCommand : IGameCommand
    {
        public List<string> ActivatingWords => new List<string>() { "dance" };

        public string PermissionNeeded => null;

        public void Execute(List<string> extraWords, Character dancingCharacter)
        {
            dancingCharacter.SendDescriptiveTextDtoMessage("You dance around looking silly.");
            dancingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{dancingCharacter.Name} is dancing around.", dancingCharacter);
        }
    }
}
