using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Commands
{
    class LetPlayerChangeTheirNameCommand : ICommand
    {
        public void Exceute(EngineInternal engine)
        {
            Console.Write($"What would you like your name to be?: ");
            GameState.CurrentGameState.PlayerName = Console.ReadLine();
            Console.WriteLine($"Your new name is {GameState.CurrentGameState.PlayerName}.");
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("namechange", StringComparison.OrdinalIgnoreCase);
        }
    }
}
