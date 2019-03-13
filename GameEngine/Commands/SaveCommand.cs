using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Commands
{
    internal class SaveCommand : ICommand
    {
        public void Exceute(EngineInternal engine)
        {
            //TODO: Implement this.
            throw new NotImplementedException();
        }

        public bool IsActivatedBy(string word)
        {
            return word.Equals("save", StringComparison.OrdinalIgnoreCase);
        }
    }
}
