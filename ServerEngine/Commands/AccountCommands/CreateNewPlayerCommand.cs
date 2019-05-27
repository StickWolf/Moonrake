using BaseClientServerDtos.ToClient;
using System.Collections.Generic;

namespace ServerEngine.Commands.AccountCommands
{
    internal class CreateNewPlayerCommand : IAccountCommand
    {
        public List<string> ActivatingWords => new List<string>() { "createnewplayer" };

        public string PermissionNeeded => null;

        public void Execute(List<string> extraWords, Account executingAccount)
        {
            Client executingClient = null; // TODO: Get this, maybe pass it in as an optional parameter for account commands

            if (executingAccount == null)
            {
                return;
            }

            if (GameState.CurrentGameState == null)
            {
                var errorMsgDto = new DescriptiveTextDto("The create new player command is currently unavailable.");
                executingClient?.SendDtoMessage(errorMsgDto);
                return;
            }

            var newPlayerCharacter = EngineInternal.NewPlayerCreator();

            // Mark all player characters as needing focus to stay in the world
            newPlayerCharacter.NeedsFocus = true;
            executingAccount.Characters.Add(newPlayerCharacter.TrackingId);
        }
    }
}
