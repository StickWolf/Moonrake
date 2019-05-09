using GameEngine.Characters;
using System;
using System.Collections.Generic;

namespace GameEngine.Commands
{
    internal class LoadCommand : ICommandInternal
    {
        public void Execute(EngineInternal engine, List<string> extraWords, Character loadingCharacter)
        {
            var validSlotNames = GameState.GetValidSaveSlotNames();
            validSlotNames.Add("Start a new game");

            bool includeCancel = false;
            if (GameState.CurrentGameState != null)
            {
                includeCancel = true;
            }

            var slotToLoad = loadingCharacter.Choose("Load or start new game?", validSlotNames, includeCancel: includeCancel);
            if (slotToLoad == null)
            {
                loadingCharacter.SendMessage("Canceled Load");
            }
            else if (slotToLoad.Equals("Start a new game"))
            {
                engine.StartNewGame();
            }
            else
            {
                loadingCharacter.SendMessage();
                loadingCharacter.SendMessage($"Loading {slotToLoad}.");
                GameState.LoadGameState(slotToLoad);
                loadingCharacter.SendMessage("Loading complete.");
            }
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("load", StringComparison.OrdinalIgnoreCase);
        }
    }
}
