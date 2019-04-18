using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Commands
{
    class StatsCommand : ICommand
    {
        public void Exceute(EngineInternal engine, List<string> extraWords)
        {
            engine.GameData.TryGetCharacter("Player", out Character charPlayer);
            Console.WriteLine("Here are your stats:");
            Console.WriteLine($"You have {charPlayer.Hp}/{charPlayer.FullHp} HP");
            Console.WriteLine($"Your max attack is {charPlayer.MaxAttack}.");
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("stats", StringComparison.OrdinalIgnoreCase);
        }
    }
}
