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

        public void Attack(Character attackingCharacter, Character defendingCharacter)
        {
            var attackDamage = GetAttackDamage(attackingCharacter.MaxAttack);
            if(defendingCharacter.Hp <= 0)
            {
                Console.WriteLine($"{defendingCharacter.Name} is dead.");
                return;
            }
            defendingCharacter.Hp = defendingCharacter.Hp - attackDamage;
            Console.WriteLine($"{defendingCharacter.Name} has been attacked for {attackDamage} dammage. {defendingCharacter.Name} now has {defendingCharacter.Hp}/{defendingCharacter.FullHp}");
        }

        private int GetAttackDamage(int maxAttack)
        {
            int damage = rnd.Next(0, MaxAttack);
            return damage;
        }
    }
}
