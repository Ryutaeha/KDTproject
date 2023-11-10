using KDTproject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class Character
{

    public class Player : ICharacter
    {
        //플래이어 캐릭터의 직업
        public string JobClass { get; }
        public int Level { get; set; }
        public string Name { get; }
        public int Attack => new Random().Next((JobClass == "전사" ? 5 : 0) + AttackForce, (JobClass == "전사" ? 10 : 20) + AttackForce);
        public int AttackForce { get; set; }
        public int Defense { get; set; }
        public int Health { get; set; }
        public int MaxHealth {  get; set; }
        public int Gold { get; set; }
        public bool IsDead => Health <= 0;
        public string Ability { get; }
        public int LiveDay { get; set; } 

        public void BeDamaged(int damage)
        {
            if (IsDead) { Console.WriteLine("사망씬 구현"); }
            else { Console.WriteLine($"{Name}의 남은 체력 : {Health}"); }
        }
        public Player(string name, string jobClass)
        {
            JobClass = jobClass;
            Ability = JobClass == "전사" ? "레벨업시 최대체력이 증가합니다" : "레벨업시 골드 획득량이 증가합니다";
            Name = name;
            Level = 1;
            MaxHealth = 100;
            Health = 100;
            Gold = 100;
            Defense = jobClass == "전사" ? new Random().Next(4, 7) : new Random().Next(0, 3);
            AttackForce = JobClass == "전사" ? 5 : 10;
            LiveDay = 1;
        }
        public Player(string name, string jobClass, int level, int health, int maxHealth, int gold, int liveDay)
        {
            JobClass = jobClass;
            Ability = JobClass == "전사" ? "레벨업시 최대체력이 증가합니다" : "레벨업시 골드 획득량이 증가합니다";
            Name = name;
            Level = level;
            MaxHealth = maxHealth;
            Health = health;
            Gold = gold;
            AttackForce = JobClass == "전사" ? 5 : 0;
            LiveDay = liveDay;
        }
    }
    internal class Inventory
    {
        
        public static void MyInventory(ConsoleKeyInfo keyInfo, int[] map, int[] selectCampMenu, List<CharacterSubMenu.Weapon> weapons, List<CharacterSubMenu.Armor> armors,string View)
        {

            bool methodEnd = false;
            int inventorySelect= 0 ;
            string[] inventoryMenu = Date.inventory;
            string enhance = View == "Enhance" ? "MAX" : "착용";
            while (!methodEnd)
            {
                    Draw.ClearBox(map);
                    int yPosition = 11;
                    switch (inventorySelect)
                    {
                        case 0:
                            if(weapons.Count == 0)
                            {
                                Draw.WriteText("보유중인 무기가 없습니다.", 20,15);
                            }
                            else
                            {

                                Draw.ItemListHeader(enhance);
                                for (int i = 0; i < weapons.Count; i++)
                                {
                                    Draw.ItemList(weapons[i], yPosition, i, View);
                                    if (Program.Equipment[0]==i && View != "Enhance") Draw.WriteText($"[E]", 55, yPosition + (i * 2));
                                    if(View == "Enhance") Draw.WriteText($"[{weapons[i].MaxEnhance}]", 55, yPosition + (i * 2));
                            }

                            Draw.WriteText("뒤로가기", 46, 22);
                            }
                            break;

                        case 1:
                            if(armors.Count == 0)
                            {
                                Draw.WriteText("보유중인 방어구가 없습니다.", 20, 15);
                            }
                            else
                            {
                                Draw.ItemListHeader(enhance);
                                for (int i = 0; i < armors.Count; i++)
                                    {
                                        Draw.ItemList(armors[i], yPosition, i, View);
                                        if (Program.Equipment[1] == i) Draw.WriteText($"[E]", 55, yPosition + (i * 2));
                                        if (View == "Enhance") Draw.WriteText($"[{armors[i].MaxEnhance}]", 55, yPosition + (i * 2));
                            }
                            Draw.WriteText("뒤로가기", 46, 22);
                            }
                            break;

                        
                    }

                Draw.ClearBox(selectCampMenu);

                keyEvent.SelectMenu(inventoryMenu, inventorySelect, 70, 7, 3, true);

                (bool selectEnter, int selectMenu) = keyEvent.keyCheck(keyInfo, inventorySelect, inventoryMenu.Length);

                inventorySelect = selectMenu;

                if (selectEnter)
                {
                    switch (selectMenu)
                    {
                        case 0:
                            if(View == "inventory" && weapons.Count > 0) Equipment(keyInfo, weapons.Count, inventorySelect);
                            if (View == "sellShop" && weapons.Count > 0) SubMethod.Sell(keyInfo, weapons.Count, inventorySelect);
                            if (View == "Enhance" && weapons.Count > 0) SubMethod.Enhance(keyInfo, weapons.Count, inventorySelect);
                            break;
                        case 1:
                            if (View == "inventory" && armors.Count > 0) Equipment(keyInfo, armors.Count, inventorySelect);
                            if (View == "sellShop" && armors.Count > 0) SubMethod.Sell(keyInfo, armors.Count, inventorySelect);
                            if (View == "Enhance" && armors.Count > 0) SubMethod.Enhance(keyInfo, armors.Count, inventorySelect);
                            break;
                        case 2:
                            methodEnd = true;
                            break;
                    }
                }

                
                
            }
        }

        public static void Equipment(ConsoleKeyInfo keyInfo, int menuLength, int itemClass)
        {
            bool methodEnd=false;
            int yPosition = 11;
            int selectEquipment = 0;

            while (!methodEnd)
            {
                SubMethod.selectItemCorsor(menuLength, selectEquipment, yPosition);
                

                (bool selectEnter, int selectMenu) = keyEvent.keyCheck(keyInfo, selectEquipment, menuLength+1);

                selectEquipment = selectMenu;
                if (selectEnter)
                {
                    if(Program.Equipment[itemClass]== selectEquipment) Program.Equipment[itemClass] = -1;
                    else if(selectEquipment != menuLength) Program.Equipment[itemClass] = selectEquipment;
                    
                    methodEnd = true;
                }
            }
        }
    }
}


