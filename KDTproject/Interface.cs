using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



internal interface ICharacter
{
    int Level { get; set; }
    string Name { get; }
    int Attack { get; }
    int Defense { get; set; }
    int Health { get; set; }
    int Gold { get; set; }
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

