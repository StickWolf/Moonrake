using BaseClientServerDtos.ToServer;
using System.Collections.Generic;
using System.Linq;

namespace GameClient.Commands
{
    public class GenericServerCommand : ICommand
    {
        public List<string> ActivatingWords => new List<string>() { "generic" };

        public void Execute(List<string> extraWords)
        {
            if (extraWords.Count == 0)
            {
                return;
            }

            var genericCommand = new GenericServerCommandDto();
            genericCommand.Command = extraWords.First();
            genericCommand.ExtraWords = extraWords.Skip(1).ToList();
            ServerConnection.SendDtoMessage(genericCommand);
        }
    }
}
