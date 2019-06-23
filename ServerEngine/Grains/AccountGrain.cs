using Konscious.Security.Cryptography;
using Orleans;
using ServerEngine.Characters;
using ServerEngine.GrainInterfaces;
using ServerEngine.GrainSiloAndClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerEngine.Grains
{
    public class AccountGrain : Grain<AccountState>, IAccountGrain
    {
        private TimeSpan LastPlayerCreationTimeSpanCooldown = TimeSpan.FromMinutes(10);

        public Task<string> GetUserName()
        {
            return Task.FromResult(this.GetPrimaryKeyString());
        }

        public Task<bool> CanCreateNewPlayer()
        {
            if ((State.LastPlayerCreationTime + LastPlayerCreationTimeSpanCooldown) > DateTime.Now)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        public Task SetPassword(string password)
        {
            State.HashedPassword = GetPasswordHash(password);
            return WriteStateAsync();
        }

        public Task<bool> ValidatePassword(string password)
        {
            var proposedHash = GetPasswordHash(password);
            bool isCorrect = proposedHash.SequenceEqual(State.HashedPassword);
            return Task.FromResult(isCorrect);
        }

        public Task AddPermission(string permissionName)
        {
            permissionName = permissionName.ToLower();
            if (!State.Permissions.Contains(permissionName))
            {
                State.Permissions.Add(permissionName);
                return this.WriteStateAsync();
            }
            return Task.CompletedTask;
        }

        public Task<bool> HasPermission(string permissionName)
        {
            var has = State.Permissions.Contains(permissionName, StringComparer.OrdinalIgnoreCase);
            return Task.FromResult(has);
        }

        public Task<List<string>> GetPermissions()
        {
            return Task.FromResult(State.Permissions);
        }

        public Task AddCharacter(Guid characterTrackingId)
        {
            State.Characters.Add(characterTrackingId);
            State.LastPlayerCreationTime = DateTime.Now;
            return this.WriteStateAsync();
        }

        public Task<Character> GetCharacter(string characterName)
        {
            var allAccountCharacters = State.Characters.Select(c => GrainClusterClient.Universe.GetCharacter(c).Result);
            var foundCharacter = allAccountCharacters.FirstOrDefault(c => c.Name.Equals(characterName, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(foundCharacter);
        }

        private byte[] GetPasswordHash(string password)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var usernameBytes = Encoding.UTF8.GetBytes(GetUserName().Result);
            var argon2 = new Argon2i(passwordBytes);
            argon2.DegreeOfParallelism = 16;
            argon2.MemorySize = 8192;
            argon2.Iterations = 64;
            argon2.Salt = State.PasswordSalt;
            argon2.AssociatedData = usernameBytes;
            return argon2.GetBytes(128);
        }
    }
}
