using ServerEngine.GrainInterfaces;
using System;
using System.Collections.Generic;

namespace ServerEngine.Grains
{
    public class GameUniverseState
    {
        //private bool Initialized = false;

        public DateTime CreationTime { get; set; } = DateTime.Now;

        public Dictionary<string, IAccountGrain> Accounts { get; set; } = new Dictionary<string, IAccountGrain>();

        public DateTime LastAccountCreationTime { get; set; } = DateTime.MinValue;

        // Only 1 account can be created per 10 minutes
        // TODO: Temp limiters for testing, either expose as an option to be set by gamedata or come up with something better
        public TimeSpan LastAccountCreationTimeSpanCooldown = TimeSpan.FromMinutes(10);

        //public bool Initialize()
        //{
        //    if (!Initialized)
        //    {
        //        CreationTime = DateTime.Now;
        //        Accounts = new Dictionary<string, IAccountGrain>();
        //        Initialized = true; // TODO: if the autoprops work then just use that
        //        return true;
        //    }
        //    return false;
        //}
    }
}
