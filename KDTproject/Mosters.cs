using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDTproject
{
    internal class Mosters
    {
        public class Moster : ICharacter
        {
            public int Level { get; }
            public string Name { get; }
            public int Attack => new Random().Next(AttackForce-5, AttackForce+10);
            public int AttackForce { get; set; }
            public int Defense { get; }
            public int Health { get; set; }
            public int Gold { get; }
            public string Infomation { get; }
            public bool IsDead => Health <= 0;
            public int DropExp;
            public void BeDamaged(int damage)
            {
                if (IsDead) { Console.WriteLine("사망씬 구현"); }
                else { Console.WriteLine($"{Name}의 남은 체력 : {Health}"); }
            }
            public Moster(string name, string attackForce, string defense, string health, string gold, string infomation, string dropExp)
            {
                Level = new Random().Next(1, 11);
                Name = name;
                AttackForce = new Random().Next(int.Parse(attackForce) - 5, int.Parse(attackForce) + 5);
                Defense = new Random().Next(int.Parse(defense) - 5, int.Parse(defense) + 5);
                Health = new Random().Next(int.Parse(health) - 10, int.Parse(health) + 10);
                Gold = new Random().Next(int.Parse(gold) - int.Parse(gold) / 10, int.Parse(gold) + int.Parse(gold) / 10);
                Infomation = infomation;
                DropExp = int.Parse(dropExp);  
            }

        }
    }
}
