using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class CharacterSubMenu
{
    public class Weapon : IItem
    {
        public int ItemClass { get; }
        public int Rank { get; }
        public string ItemName { get; }
        public int ItemAbility { get; set; }
        public bool IsPotion { get; }
        public string Information { get; }
        public int Price { get; }
        public int Enhance {  get; set; }
        public int EnhancePrice { get; set; }
        public int MaxEnhance { get; }
        /// <summary>
        /// 아이템 이름, 랭크, 능력치, 설명, 가격
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="rank"></param>
        /// <param name="itemAbility"></param>
        /// <param name="information"></param>
        /// <param name="price"></param>
        public Weapon(string itemName,string rank, string itemAbility, string information, string price)
        {
            Enhance = 0;
            ItemClass = 0;
            Rank = int.Parse(rank);
            ItemName = itemName;
            ItemAbility = new Random().Next(int.Parse(itemAbility) - (int.Parse(itemAbility) / 10), int.Parse(itemAbility) + (int.Parse(itemAbility) / 10));
            IsPotion = false;
            Information = information;
            Price = new Random().Next(int.Parse(price) - (int.Parse(price) / 10), int.Parse(price) + (int.Parse(price) / 10));
            EnhancePrice = Price / 10;
            switch (Rank)
            {
                case 0:
                    MaxEnhance = 3;
                    break;
                case 1:
                    MaxEnhance = 5;
                    break;
                case 2:
                    MaxEnhance = 10;
                    break;
                case 3:
                    MaxEnhance = 20;
                    break;
            }
        }
    }
    public class Armor : IItem
    {
        public int ItemClass { get; }
        public int Rank { get; }
        public string ItemName { get; }
        public int ItemAbility { get; set; }
        public bool IsPotion { get; }
        public string Information { get; }
        public int Price { get; set; }
        public int Enhance { get; set; }
        public int EnhancePrice { get; set; }
        public int MaxEnhance { get; }
        public Armor(string itemName, string rank, string itemAbility, string information, string price)
        {
            Enhance = 0;
            ItemClass = 1;
            Rank = int.Parse(rank);
            ItemName = itemName;
            ItemAbility = new Random().Next(int.Parse(itemAbility) - (int.Parse(itemAbility) / 10), int.Parse(itemAbility) + (int.Parse(itemAbility) / 10));
            IsPotion = false;
            Information = information;
            Price = new Random().Next(int.Parse(price)-(int.Parse(price) / 10), int.Parse(price) + (int.Parse(price)/10));
            EnhancePrice = Price / 10;
            switch (Rank)
            {
                case 0:
                    MaxEnhance = 3;
                    break;
                case 1:
                    MaxEnhance = 5;
                    break;
                case 2:
                    MaxEnhance = 10;
                    break;
                case 3:
                    MaxEnhance = 20;
                    break;
            }
        }
    }
    public class Potion : IItem
    {
        public int ItemClass { get; }
        public int Rank { get; }
        public string ItemName { get; }
        public int ItemAbility { get; }
        public bool IsPotion { get; }
        public string Information { get; }
        public int Price { get; }
        public Potion(string itemName, string itemAbility, string information, string price)
        {
            {
                ItemClass = 2;
                Rank = 0;
                ItemName = itemName;
                ItemAbility = int.Parse(itemAbility);
                IsPotion = true;
                Information = information;
                Price = int.Parse(price);
            }
        }
        public void Use(Character.Player player)
        {
            if(ItemName == "체력 포션")
            {
                player.Health += ItemAbility;
                if(player.Health > player.MaxHealth)
                {
                    player.Health = player.MaxHealth;
                }
            }
            else if(ItemName == "방어력 포션")
            {
                player.Defense += ItemAbility;
            }
            else if(ItemName == "공격력 포션")
            {
                player.AttackForce += ItemAbility;
            }
        }
    }
}

