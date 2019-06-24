using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerEngine.GrainInterfaces
{
    public interface IAccountsGrain : IGrainWithStringKey
    {
        Task<IAccountGrain> CreateAccount(string userName, string password);

        Task<IAccountGrain> GetAccount(string userName);

        Task<IAccountGrain> GetSysopAccount();
    }
}
