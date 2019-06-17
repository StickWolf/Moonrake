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

        /// <summary>
        /// Gets a game variable by its full name
        /// </summary>
        /// <param name="gameVariableName">The name of the game variable to get</param>
        /// <returns>The value or null if it's not set.</returns>
        Task<string> GetGameVarValue(string gameVariableName);

        /// <summary>
        /// Sets the game variable to the specified value
        /// </summary>
        /// <param name="gameVariableName">The game variable to set</param>
        /// <param name="value">The value to set the game variable to</param>
        Task<string> SetGameVarValue(string gameVariableName, string value);
    }
}
