using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundUtils
{
    public class Sounds
    {
        public void AttacksBeep()
        {
            Console.Beep(400, 100);
            Console.Beep(500, 100);
            Console.Beep(600, 100);
            Console.Beep(400, 100);
        }

        public void HealBeeps()
        {
            Console.Beep(800, 100);
            Console.Beep(700, 100);
            Console.Beep(600, 100);
            Console.Beep(700, 100);
        }

        public void AncientPianoBeeps()
        {
            Console.Beep(100, 1000);
            Console.Beep(200, 1000);
            Console.Beep(300, 1000);
            Console.Beep(200, 1000);
            Console.Beep(100, 1000);
        }

        public void AncientHarpBeeps()
        {
            Console.Beep(800, 100);
            Console.Beep(700, 100);
            Console.Beep(600, 100);
            Console.Beep(700, 100);
            Console.Beep(800, 100);
            Console.Beep(800, 100);
        }

        public void AncientDrumBeeps()
        {
            Console.Beep(100, 100);
            Console.Beep(100, 100);
            Console.Beep(100, 100);
            Console.Beep(90, 100);
            Console.Beep(100, 100);
            Console.Beep(100, 100);
        }
    }
}
