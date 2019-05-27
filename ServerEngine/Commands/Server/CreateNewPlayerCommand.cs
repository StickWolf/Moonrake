using BaseClientServerDtos.ToClient;
using System.Collections.Generic;

namespace ServerEngine.Commands.Internal
{
    internal class CreateNewPlayerCommand : ICommandServer
    {
        public List<string> ActivatingWords => new List<string>() { "createnewplayer" };

        public void Execute(List<string> extraWords, Client executingClient)
        {
            if (executingClient == null)
            {
                return;
            }

            if (GameState.CurrentGameState == null || executingClient.AttachedAccount == null)
            {
                var errorMsgDto = new DescriptiveTextDto("The create new player command is currently unavailable.");
                executingClient.SendDtoMessage(errorMsgDto);
                return;
            }

            var newPlayerCharacter = EngineInternal.NewPlayerCreator();

            // Mark all player characters as needing focus to stay in the world
            newPlayerCharacter.NeedsFocus = true;
            executingClient.AttachedAccount.Characters.Add(newPlayerCharacter.TrackingId);
        }
    }
}
