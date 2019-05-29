using ServerEngine.Characters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerEngine.Commands.GameCommands
{
    public class StatsCommand : IGameCommand
    {
        public List<string> ActivatingWords => new List<string>() { "stats" };

        public string PermissionNeeded => null;

        public void Execute(List<string> extraWords, Character statSeekingCharacter)
        {
            var statSeekingCharacterLocation = GameState.CurrentGameState.GetCharacterLocation(statSeekingCharacter.TrackingId);

            StringBuilder statsBuilder = new StringBuilder();

            statsBuilder.AppendLine("Here are your stats:");
            statsBuilder.AppendLine($"You have {statSeekingCharacter.HitPoints}/{statSeekingCharacter.MaxHitPoints} HP");
            statsBuilder.AppendLine($"Your max attack is {statSeekingCharacter.MaxAttack}.");

            statSeekingCharacter.SendDescriptiveTextDtoMessage(statsBuilder.ToString());
            statSeekingCharacter.GetLocation().SendDescriptiveTextDtoMessage($"{statSeekingCharacter.Name} looks lost in thought.", statSeekingCharacter);
        }
    }
}
