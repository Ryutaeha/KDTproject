using KDTproject;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class Character
{
    
    public class Player : ICharacter
    {

        public Player() { }

        //플래이어 캐릭터의 직업

        [JsonProperty(Order = 1)] public string JobClass { get; set; }
        [JsonProperty(Order = 2)] public int Level { get; set; }
        [JsonProperty(Order = 3)] public string Name { get; set; }
        [JsonProperty(Order = 4)] public int Attack => new Random().Next((JobClass == "전사" ? 5 : 0) + AttackForce, (JobClass == "전사" ? 10 : 20) + AttackForce);
        [JsonProperty(Order = 5)] public int AttackForce { get; set; }
        [JsonProperty(Order = 6)] public int Defense { get; set; }
        [JsonProperty(Order = 7)] public int Health { get; set; }
        [JsonProperty(Order = 8)] public int MaxHealth {  get; set; }
        [JsonProperty(Order = 9)] public int Gold { get; set; }
        [JsonProperty(Order = 10)] public bool IsDead => Health <= 0;
        [JsonProperty(Order = 11)] public string Ability { get; }
        [JsonProperty(Order = 12)] public int LiveDay { get; set; }
        [JsonProperty(Order = 13)] public int GoldDrop { get; set; }
        [JsonProperty(Order = 14)] public int EXP { get; set; }
        [JsonProperty(Order = 15)] public int NextEXP = 3 ;
        public int LevelUp()
        {
            int up = 0;
            while(EXP >= NextEXP)
            {
                NextEXP += (++Level+2);
                MaxHealth += JobClass== "전사"? 10 : 5 ;
                Health += 20;
                if(Health > MaxHealth)
                {
                    Health = MaxHealth;
                }
                GoldDrop += JobClass == "전사" ? 5 : 10;
                ++up;
            }
            return up;
        }
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
            Defense = jobClass == "전사" ? new Random().Next(14, 17) : new Random().Next(6, 8);
            AttackForce = JobClass == "전사" ? 15 : 30;
            LiveDay = 1;
            GoldDrop = JobClass == "전사" ? 0 : 10;
        }
        public Player(Player savedate)
        {
            JobClass = savedate.JobClass;
            Ability = JobClass == "전사" ? "레벨업시 최대체력이 증가합니다" : "레벨업시 골드 획득량이 증가합니다";
            Name = savedate.Name;
            Level = savedate.Level;
            Defense = savedate.Defense;
            MaxHealth = savedate.MaxHealth;
            Health = savedate.Health;
            Gold = savedate.Gold;
            GoldDrop = savedate.GoldDrop;
            EXP = savedate.EXP;
            NextEXP = savedate.NextEXP;
            AttackForce = savedate.AttackForce;
            LiveDay = savedate.LiveDay;
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
                            
                            Draw.WeaponList(enhance ,View, yPosition);
                            


                        }
                            break;

                        case 1:
                            if(armors.Count == 0)
                            {
                                Draw.WriteText("보유중인 방어구가 없습니다.", 20, 15);
                            }
                            else
                            {
                            
                            Draw.ArmorList(enhance,View, yPosition);

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

                    for (int i = 0; i < menuLength; i++)
                    {
                        if (i != selectEquipment)
                        {
                            Draw.WriteText($"   ", 55, yPosition + (i * 2));
                        }
                    }

                    if (GameScene.Equipment[itemClass] == selectEquipment)
                    {
                        GameScene.Equipment[itemClass] = -1;
                        Draw.WriteText($"   ", 55, yPosition + (selectEquipment * 2));
                    }
                    else if (selectEquipment != menuLength)
                    {
                         GameScene.Equipment[itemClass] = selectEquipment;
                        Draw.WriteText($"[E]", 55, yPosition + (selectEquipment * 2));
                    }
                    else
                    {
                        return;
                    }
                }

            }
        }
    }
}


