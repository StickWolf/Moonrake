using ServerEngine.GrainInterfaces;
using System;
using System.Collections.Generic;

namespace ServerEngine.Grains
{
    public class AccountsState
    {
        public DateTime LastAccountCreationTime { get; set; } = DateTime.MinValue;

        // Only 1 account can be created per 10 minutes
        // TODO: Temp limiters for testing, either expose as an option to be set by gamedata or come up with something better
        public TimeSpan LastAccountCreationTimeSpanCooldown = TimeSpan.FromMinutes(10);
    }
}
