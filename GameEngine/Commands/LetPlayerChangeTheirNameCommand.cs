using System;
using System.Collections.Generic;

namespace GameEngine.Commands
{
    internal class LetPlayerChangeTheirNameCommand : ICommandInternal
    {
        public void Execute(EngineInternal engine, List<string> extraWords)
        {
            var playerCharacter = GameState.CurrentGameState.GetPlayerCharacter();
            Console.Write($"What would you like your name to be?: ");
            playerCharacter.Name = Console.ReadLine();
            Console.WriteLine($"Your new name is {playerCharacter.Name}.");
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("namechange", StringComparison.OrdinalIgnoreCase);
        }
    }
}
