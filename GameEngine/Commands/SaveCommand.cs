using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Commands
{
    public class SaveCommand : ICommand
    {
        public bool IsActivatedBy(string word)
        {
            return word.Equals("save", StringComparison.OrdinalIgnoreCase);
        }
    }
}
