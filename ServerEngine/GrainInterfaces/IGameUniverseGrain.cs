using Orleans;
using System.Threading.Tasks;

namespace ServerEngine.GrainInterfaces
{
    public interface IGameUniverseGrain : IGrainWithStringKey
    {
        /// <summary>
        /// Gets a string that represents details about the game universe.
        /// These details contain information such as when the universe was created, how many accounts it has, etc.
        /// </summary>
        /// <returns></returns>
        Task<string> GetStats();

        Task<IAccountGrain> CreateAccount(string userName, string password);

        Task<IAccountGrain> GetAccount(string userName);

        Task<IAccountGrain> GetSysopAccount();
    }
}
