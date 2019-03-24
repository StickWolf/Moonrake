﻿using System;
using System.Collections.Generic;

namespace GameEngine.Commands
{
    internal class LoadCommand : ICommand
    {
        public void Exceute(EngineInternal engine)
        {
            var validSlotNames = GameState.GetValidSaveSlotNames();
            validSlotNames.Add("Start a new game");

            var slotToLoad = Console.Choose("Load or start new game?", validSlotNames);
            if (slotToLoad.Equals("Start a new game"))
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
