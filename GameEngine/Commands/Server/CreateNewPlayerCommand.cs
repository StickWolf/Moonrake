using System.Collections.Generic;
using System.Linq;
using GameEngine.Characters;
using GameEngine.Commands.Public;

namespace GameEngine.Commands.Internal
{
    internal class CreateNewPlayerCommand : ICommandServer
    {
        public List<string> ActivatingWords => new List<string>() { "createnewplayer" }; // TODO: seek out places that used to call "load" command and fix them

        public void Execute(List<string> extraWords, Client executingClient)
        {
            if (executingClient == null)
            {
                return;
            }

            if (GameState.CurrentGameState == null)
            {
                executingClient.SendMessage("The create new player command is currently unavailable.");
                return;
            }

            // TODO: When a new player is created, it should be attached to an account... Create accounts!
            var newPlayerCharacter = EngineInternal.NewPlayerCreator();
            AttachedClients.SetClientFocusedCharacter(executingClient.TrackingId, newPlayerCharacter.TrackingId);

            InternalCommandHelper.TryRunInternalCommand("clear", new List<string>(), newPlayerCharacter);
            newPlayerCharacter.SendMessage(GameState.CurrentGameState.GameIntroductionText);
            newPlayerCharacter.SendMessage();
            newPlayerCharacter.ExecuteLookCommand();
        }
    }
}
