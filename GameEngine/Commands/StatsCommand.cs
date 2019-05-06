using System;
using System.Collections.Generic;

namespace GameEngine.Commands
{
    class StatsCommand : ICommand
    {
        public void Exceute(List<string> extraWords)
        {
            // TODO: Instead pass this in from the character that is using the command
            var statSeekingCharacter = GameState.CurrentGameState.GetPlayerCharacter();
            Console.WriteLine("Here are your stats:");
            Console.WriteLine($"You have {statSeekingCharacter.HitPoints}/{statSeekingCharacter.MaxHitPoints} HP");
            Console.WriteLine($"Your max attack is {statSeekingCharacter.MaxAttack}.");
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("stats", StringComparison.OrdinalIgnoreCase);
        }
    }
}
