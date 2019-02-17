using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;

namespace GameData
{
    public class Data : IGameData
    {
        public List<Character> CreateAllGameCharacters()
        {
            var allCharacters = new List<Character>();

            // TODO: create a list of characters that will be in this game here.

            return allCharacters;
        }
    }
}
