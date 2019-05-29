﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ServerEngine
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Account : TrackableInstance
    {
        [JsonProperty]
        public string UserName { get; set; }

        [JsonProperty]
        public List<string> Permissions { get; set; }

        [JsonProperty]
        public List<Guid> Characters { get; set; } = new List<Guid>();
    }
}