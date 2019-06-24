using System;
using System.Collections.Generic;

namespace ServerEngine.GrainStates
{
    public class AccountState
    {
        public bool Claimed { get; set; } = false;
        public byte[] HashedPassword { get; set; }
        public byte[] PasswordSalt { get; set; } = Guid.NewGuid().ToByteArray();
        public List<string> Permissions { get; set; } = new List<string>();
        public List<Guid> Characters { get; set; } = new List<Guid>();
        public DateTime LastPlayerCreationTime { get; set; } = DateTime.MinValue;
    }
}
