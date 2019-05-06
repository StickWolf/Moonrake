using System;
using System.Collections.Generic;

namespace GameEngine.Commands
{
    class StatsCommand : ICommand
    {
        public void Execute(List<string> extraWords, Guid statSeekingCharacterTrackingId)
        {
            var statSeekingCharacter = GameState.CurrentGameState.GetCharacter(statSeekingCharacterTrackingId);
            var statSeekingCharacterLocation = GameState.CurrentGameState.GetCharacterLocation(statSeekingCharacterTrackingId);

            statSeekingCharacter.SendMessage("Here are your stats:");
            statSeekingCharacter.SendMessage($"You have {statSeekingCharacter.HitPoints}/{statSeekingCharacter.MaxHitPoints} HP");
            statSeekingCharacter.SendMessage($"Your max attack is {statSeekingCharacter.MaxAttack}.");
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("stats", StringComparison.OrdinalIgnoreCase);
        }
    }
}
