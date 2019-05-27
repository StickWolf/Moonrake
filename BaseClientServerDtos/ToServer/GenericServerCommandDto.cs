using System.Collections.Generic;

namespace BaseClientServerDtos.ToServer
{
    public class GenericServerCommandDto : FiniteDto
    {
        public string Command { get; set; }

        public List<string> ExtraWords { get; set; }
    }
}
