using GameEngine.Characters;
using GameEngine.Commands.Public;
using System.Collections.Generic;

namespace ExampleGame.Commands
{
    public class DanceCommand : ICommand
    {
        public List<string> ActivatingWords => new List<string>() { "dance" };

        public void Execute(List<string> extraWords, Character grabbingCharacter)
        {
            grabbingCharacter.SendMessage("You dance around looking silly.");
            grabbingCharacter.GetLocation().SendMessage($"{grabbingCharacter.Name} is dancing around.", grabbingCharacter);
        }
    }
}
