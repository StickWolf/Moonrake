﻿using ServerEngine.Characters;
using System;
using System.Collections.Generic;

namespace ServerEngine.Commands.Public
{
    public class StatsCommand : ICommand
    {
        public List<string> ActivatingWords => new List<string>() { "stats" };

        public void Execute(List<string> extraWords, Character statSeekingCharacter)
        {
            // TODO: rewrite as server/client

            //var statSeekingCharacterLocation = GameState.CurrentGameState.GetCharacterLocation(statSeekingCharacter.TrackingId);

            //statSeekingCharacter.SendMessage("Here are your stats:");
            //statSeekingCharacter.SendMessage($"You have {statSeekingCharacter.HitPoints}/{statSeekingCharacter.MaxHitPoints} HP");
            //statSeekingCharacter.SendMessage($"Your max attack is {statSeekingCharacter.MaxAttack}.");
            //statSeekingCharacter.GetLocation().SendMessage($"{statSeekingCharacter.Name} looks lost in thought.", statSeekingCharacter);
        }
    }
}
