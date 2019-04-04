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

        private int Hp, FullHp, MaxAttack;

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
            if(Hp <= 0)
            {
                Console.WriteLine($"{Name} is dead.");
                return;
            }
            Hp = Hp - attackDamage;
            Console.WriteLine($"{Name} has been attacked for {attackDamage} dammage. {Name} now has {Hp}/{FullHp}");
        }

        private int GetAttackDamage(int maxAttack)
        {
            int damage = rnd.Next(0, MaxAttack);
            return damage;
        }
    }
}
