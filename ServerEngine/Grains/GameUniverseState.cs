﻿using ServerEngine.GrainInterfaces;
using System;
using System.Collections.Generic;

namespace ServerEngine.Grains
{
    public class GameUniverseState
    {
        //private bool Initialized = false;

        public DateTime CreationTime { get; set; } = DateTime.Now;

        public Dictionary<string, IAccountGrain> Accounts { get; set; } = new Dictionary<string, IAccountGrain>();

        // GameVars[{GameVarName}] = {GameVarValue}
        public Dictionary<string, string> GameVars { get; set; } = new Dictionary<string, string>();

        public DateTime LastAccountCreationTime { get; set; } = DateTime.MinValue;

        // Only 1 account can be created per 10 minutes
        // TODO: Temp limiters for testing, either expose as an option to be set by gamedata or come up with something better
        public TimeSpan LastAccountCreationTimeSpanCooldown = TimeSpan.FromMinutes(10);
    }
}
