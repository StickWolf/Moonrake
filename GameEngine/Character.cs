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

        public int Hp, FullHp, MaxAttack;

        private static Random rnd = new Random();

        public Character(string name, int hp, int attack)
        {
            Name = name;
            Hp = hp;
            FullHp = hp;
            MaxAttack = attack;
        }

        public void Attack(Character attackingCharacter)
        {
            var attackDamage = GetAttackDamage(attackingCharacter.MaxAttack);
            if (attackingCharacter.Hp == 0)
            {
                return;
            }
            if (Hp <= 0)
            {
                Console.WriteLine($"{Name} is dead.");
                return;
            }
            Console.WriteLine($"{Name} has {Hp}/{FullHp} left");
            Hp = Hp - attackDamage;
        }

        private int GetAttackDamage(int maxAttack)
        {
            int damage = rnd.Next(0, maxAttack);
            return damage;
        }
    }
}
