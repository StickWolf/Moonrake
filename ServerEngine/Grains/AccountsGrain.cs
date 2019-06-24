using Orleans;
using ServerEngine.GrainInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerEngine.Grains
{
    public class AccountsGrain : Grain<AccountsState>, IAccountsGrain
    {
        public async Task<IAccountGrain> CreateAccount(string userName, string password)
        {
            // An account can only be created so often
            if ((State.LastAccountCreationTime + State.LastAccountCreationTimeSpanCooldown) > DateTime.Now)
            {
                return null; // TODO: create a better response here (be able to tell why we're returning null)
            }

            var account = await GetAccount(userName);
            if (await account.IsClaimed())
            {
                // This account is already claimed, it cannot be re-created.
                return null; // TODO: create a better response here (be able to tell why we're returning null)
            }

            // Remember the last time an account was created.
            State.LastAccountCreationTime = DateTime.Now;
            await this.WriteStateAsync();

            // Claim/"Create" the account
            await account.SetPassword(password);
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

            var account = GrainFactory.GetGrain<IAccountGrain>(userName);
            return Task.FromResult(account);
        }

    }
}
