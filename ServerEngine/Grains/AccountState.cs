using System;
using System.Collections.Generic;

namespace ServerEngine.Grains
{
    public class AccountState
    {
        public byte[] HashedPassword { get; set; }
        public byte[] PasswordSalt { get; set; } = Guid.NewGuid().ToByteArray();
        public List<string> Permissions { get; set; } = new List<string>();
        public List<Guid> Characters { get; set; } = new List<Guid>();
        public DateTime LastPlayerCreationTime { get; set; } = DateTime.MinValue;
    }
}
