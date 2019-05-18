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
                executingClient.SendMessage("The create new player command is currently unavailable.");
                return;
            }

            var newPlayerCharacter = EngineInternal.NewPlayerCreator();

            // Mark all player characters as needing focus to stay in the world
            newPlayerCharacter.NeedsFocus = true;

            executingClient.AttachedAccount.Characters.Add(newPlayerCharacter.TrackingId);
            AttachedClients.SetClientFocusedCharacter(executingClient.TrackingId, newPlayerCharacter.TrackingId);

            InternalCommandHelper.TryRunInternalCommand("clear", new List<string>(), newPlayerCharacter);
            newPlayerCharacter.SendMessage(GameState.CurrentGameState.GameIntroductionText);
            newPlayerCharacter.SendMessage();
            newPlayerCharacter.ExecuteLookCommand();
        }
    }
}
