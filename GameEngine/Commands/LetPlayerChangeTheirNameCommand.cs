using System;
using System.Collections.Generic;

namespace GameEngine.Commands
{
    internal class LetPlayerChangeTheirNameCommand : ICommand
    {
        public void Exceute(EngineInternal engine, List<string> extraWords)
        {
            Console.Write($"What would you like your name to be?: ");
            GameState.CurrentGameState.PlayerName = Console.ReadLine();
            Console.WriteLine($"Your new name is {GameState.CurrentGameState.PlayerName}.");
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("namechange", StringComparison.OrdinalIgnoreCase);
        }
    }
}
