using Konscious.Security.Cryptography;
using Newtonsoft.Json;
using ServerEngine.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerEngine
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Account : TrackableInstance
    {
        [JsonProperty]
        public string UserName { get; set; }

        [JsonProperty]
        public byte[] HashedPassword { get; set; }

        [JsonProperty]
        private byte[] PasswordSalt { get; set; }

        [JsonProperty]
        public List<string> Permissions { get; set; }

        [JsonProperty]
        private List<Guid> Characters { get; set; } = new List<Guid>();
        private object charactersLock = new object();

        [JsonProperty]
        public DateTime LastPlayerCreationTime { get; set; } = DateTime.MinValue;

        [JsonProperty]
        public TimeSpan LastPlayerCreationTimeSpanCooldown = TimeSpan.FromMinutes(10);

        public bool CanCreateNewPlayer()
        {
            if ((LastPlayerCreationTime + LastPlayerCreationTimeSpanCooldown) > DateTime.Now)
            {
                return false;
            }
            return true;
        }

        public Account()
        {
            PasswordSalt = Guid.NewGuid().ToByteArray();
        }

        private byte[] GetPasswordHash(string password)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var usernameBytes = Encoding.UTF8.GetBytes(UserName);
            var argon2 = new Argon2i(passwordBytes);
            argon2.DegreeOfParallelism = 16;
            argon2.MemorySize = 8192;
            argon2.Iterations = 64;
            argon2.Salt = PasswordSalt;
            argon2.AssociatedData = usernameBytes;
            return argon2.GetBytes(128);
        }

        public void SetPassword(string password)
        {
            HashedPassword = GetPasswordHash(password);
        }

        public bool ValidatePassword(string password)
        {
            var proposedHash = GetPasswordHash(password);
            return proposedHash.SequenceEqual(HashedPassword);
        }

        public void AddCharacter(Guid characterTrackingId)
        {
            lock (charactersLock)
            {
                Characters.Add(characterTrackingId);
            }
        }

        public Character GetCharacter(string characterName)
        {
            lock (charactersLock)
            {
                var allAccountCharacters = Characters.Select(c => GameState.CurrentGameState.GetCharacter(c));
                var foundCharacter = allAccountCharacters.FirstOrDefault(c => c.Name.Equals(characterName, StringComparison.OrdinalIgnoreCase));
                return foundCharacter;
            }
        }
    }
}
