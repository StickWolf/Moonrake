﻿using System;
using System.Collections.Generic;

namespace GameEngine.Commands
{
    internal class LoadCommand : ICommandInternal
    {
        public void Execute(EngineInternal engine, List<string> extraWords)
        {
            var validSlotNames = GameState.GetValidSaveSlotNames();
            validSlotNames.Add("Start a new game");

            bool includeCancel = false;
            if (GameState.CurrentGameState != null)
            {
                includeCancel = true;
            }

            var slotToLoad = Console.Choose("Load or start new game?", validSlotNames, includeCancel: includeCancel);
            if (slotToLoad == null)
            {
                Console.WriteLine("Canceled Load"); // TODO: convert all internal commands to still use Character.SendMessage, it's just that they'll be hardcoded to player
            }
            else if (slotToLoad.Equals("Start a new game"))
            {
                engine.StartNewGame();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"Loading {slotToLoad}.");
                GameState.LoadGameState(slotToLoad);
                Console.WriteLine("Loading complete.");
            }
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("load", StringComparison.OrdinalIgnoreCase);
        }
    }
}
