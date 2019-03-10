﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Commands
{
    public class MoveCommand : ICommand
    {
        public bool IsActivatedBy(string word)
        {
            var validWords = new List<string>() { "go", "move", "travel" };
            return validWords.Contains(word.ToLower());
        }
    }
}