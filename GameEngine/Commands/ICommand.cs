using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Commands
{
    internal interface ICommand
    {
        bool IsActivatedBy(string word);

        void Exceute(EngineInternal engine); 
    }
}
