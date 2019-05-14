using System.Collections.Generic;

namespace GameEngine.Commands.Internal
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
                executingClient.SendMessage("The create new player command is currently unavailable.");
                return;
            }

            // TODO: All characters in this accounts should first be marked somehow as no longer present/visible in the world.

            var newPlayerCharacter = EngineInternal.NewPlayerCreator();
            executingClient.AttachedAccount.Characters.Add(newPlayerCharacter.TrackingId);
            AttachedClients.SetClientFocusedCharacter(executingClient.TrackingId, newPlayerCharacter.TrackingId);

            InternalCommandHelper.TryRunInternalCommand("clear", new List<string>(), newPlayerCharacter);
            newPlayerCharacter.SendMessage(GameState.CurrentGameState.GameIntroductionText);
            newPlayerCharacter.SendMessage();
            newPlayerCharacter.ExecuteLookCommand();
        }
    }
}
