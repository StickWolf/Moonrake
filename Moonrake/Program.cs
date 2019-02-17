using SoundUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonrake
{
    class Program
    {
        static void Main(string[] args)
        {
            var sounds = new Sounds();
            sounds.AncientDrumBeeps();
            sounds.AncientHarpBeeps();
            sounds.AncientPianoBeeps();
            sounds.AttacksBeep();
            sounds.HealBeeps();
        }
    }
}
