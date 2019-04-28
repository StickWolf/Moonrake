using System;
using System.Collections.Generic;

namespace GameEngine.Commands
{
    class StatsCommand : ICommand
    {
        public void Exceute(GameSourceData gameData, List<string> extraWords)
        {
            var statSeekingCharacter = GameState.CurrentGameState.GetPlayerCharacter();
            Console.WriteLine("Here are your stats:");
            Console.WriteLine($"You have {statSeekingCharacter.Hp}/{statSeekingCharacter.FullHp} HP");
            Console.WriteLine($"Your max attack is {statSeekingCharacter.MaxAttack}.");
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("stats", StringComparison.OrdinalIgnoreCase);
        }
    }
}
