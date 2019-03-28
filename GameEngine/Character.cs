using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Character
    {
        public string Name { get; private set; }

        private int Hp, FullHp;

        public Character(string name, int hp)
        {
            Name = name;
            Hp = hp;
            FullHp = hp;
        }
    }
}
