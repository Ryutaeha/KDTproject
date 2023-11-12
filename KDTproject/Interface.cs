













using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



internal interface ICharacter
{
    int Level { get; }
    string Name { get; }
    int Attack { get; }
    int AttackForce { get; }
    int Defense { get; }
    int Health { get; set; }
    int Gold { get; }
    bool IsDead { get; }
    void BeDamaged(int damage);
}
internal interface IItem
{
    int ItemClass {  get; }
    int Rank { get; }
    string ItemName {  get; }
    int ItemAbility { get; }
    bool IsPotion {  get; }
    string Information { get; }
    int Price {  get; }
}

