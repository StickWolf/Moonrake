using Orleans;
using ServerEngine.GrainInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerEngine.Grains
{
    public class GameUniverseGrain : Grain<GameUniverseState>, IGameUniverseGrain
    {
        public Task<string> GetStats()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Id: {this.GetPrimaryKeyString()}");
            sb.AppendLine($"Number accounts: {State.Accounts.Count}");
            return Task.FromResult(sb.ToString());
        }

        public async Task<IAccountGrain> CreateAccount(string userName, string password)
        {
            // An account can only be created so often
            if ((State.LastAccountCreationTime + State.LastAccountCreationTimeSpanCooldown) > DateTime.Now)
            {
                return null; // TODO: create a better response here (be able to tell why we're returning null)
            }
            State.LastAccountCreationTime = DateTime.Now;

            var alreadyExistsAccount = GetAccount(userName).Result;
            if (alreadyExistsAccount != null)
            {
                // This account already exists.
                // Only direct GetAccount calls will return an account like this.
                return null; // TODO: create a better response here (be able to tell why we're returning null)
            }

            var account = GrainFactory.GetGrain<IAccountGrain>(userName);
            await account.SetPassword(password);
            State.Accounts.Add(userName, account);

            // Save the newly updated data
            await this.WriteStateAsync();

            return account;
        }

        public async Task<IAccountGrain> GetSysopAccount()
        {
            var account = GrainFactory.GetGrain<IAccountGrain>("sysop");
            await account.AddPermission("Sysop");
            return account;
        }

        public Task<IAccountGrain> GetAccount(string userName)
        {
            // The sysop account is special and cannot be retrieved through normal means
            if (userName.Equals("sysop", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult<IAccountGrain>(null);
            }

            if (State.Accounts.ContainsKey(userName))
            {
                return Task.FromResult(State.Accounts[userName]);
            }
            return Task.FromResult<IAccountGrain>(null);
        }

        /// <summary>
        /// Gets a game variable by its full name
        /// </summary>
        /// <param name="gameVariableName">The name of the game variable to get</param>
        /// <returns>The value or null if it's not set.</returns>
        public Task<string> GetGameVarValue(string gameVariableName)
        {
            if (State.GameVars.ContainsKey(gameVariableName))
            {
                return Task.FromResult(State.GameVars[gameVariableName]);
            }
            return Task.FromResult<string>(null);
        }

        /// <summary>
        /// Sets the game variable to the specified value
        /// </summary>
        /// <param name="gameVariableName">The game variable to set</param>
        /// <param name="value">The value to set the game variable to</param>
        public Task<string> SetGameVarValue(string gameVariableName, string value)
        {
            // Only set and save if the value is changing
            var currentValue = GetGameVarValue(gameVariableName).Result;
            if (!value.Equals(currentValue))
            {
                State.GameVars[gameVariableName] = value;
                this.WriteStateAsync().Wait();
            }
            return Task.FromResult(gameVariableName);
        }
    }
}
