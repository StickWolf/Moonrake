using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Commands
{
    public interface ICommand
    {
        bool IsActivatedBy(string word); 
    }
}
