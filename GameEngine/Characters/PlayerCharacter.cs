using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Characters
{
    public class PlayerCharacter : Character
    {
        public static string TrackingName { get; }  = "Player";

        public PlayerCharacter(int hp) : base(TrackingName, hp)
        {
        }
    }
}
