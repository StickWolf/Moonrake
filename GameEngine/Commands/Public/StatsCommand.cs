using GameEngine.Characters;
using System;
using System.Collections.Generic;

namespace GameEngine.Commands.Public
{
    class StatsCommand : ICommand
    {
        public void Execute(List<string> extraWords, Character statSeekingCharacter)
        {
            var statSeekingCharacterLocation = GameState.CurrentGameState.GetCharacterLocation(statSeekingCharacter.TrackingId);

            statSeekingCharacter.SendMessage("Here are your stats:");
            statSeekingCharacter.SendMessage($"You have {statSeekingCharacter.HitPoints}/{statSeekingCharacter.MaxHitPoints} HP");
            statSeekingCharacter.SendMessage($"Your max attack is {statSeekingCharacter.MaxAttack}.");
            statSeekingCharacter.GetLocation().SendMessage($"{statSeekingCharacter.Name} looks lost in thought.", statSeekingCharacter);
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("stats", StringComparison.OrdinalIgnoreCase);
        }
    }
}
