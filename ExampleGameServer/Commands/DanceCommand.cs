using ServerEngine.Characters;
using ServerEngine.Commands.Public;
using System.Collections.Generic;

namespace ExampleGameServer.Commands
{
    public class DanceCommand : ICommand
    {
        public List<string> ActivatingWords => new List<string>() { "dance" };

        public void Execute(List<string> extraWords, Character grabbingCharacter)
        {
            grabbingCharacter.SendDescriptiveTextDtoMessage("You dance around looking silly.");
            grabbingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{grabbingCharacter.Name} is dancing around.", grabbingCharacter);
        }
    }
}
