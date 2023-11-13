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
    internal string[] PlayerCreate()
    {
        string[] PlayerInfo = new string[2];
        bool check = false;
        while (!check)
        {
            Draw.DrawBox(Date.info);
            Draw.ClearBox(Date.info);
            Draw.WriteText("당신의 이름은 무엇입니까? (최대 4글자)", 26, 10);
            Draw.WriteText("이름 : ", 35, 15);
            Console.SetCursorPosition(42, 15);
            string playerName = null;
            playerName = Console.ReadLine();
            if (playerName != "" && playerName.Length <= 4)
            {
                PlayerInfo[0] = playerName;
                check = true;
            }
        }
        check = false;
        while (!check)
        {
            Draw.DrawBox(Date.info);
            Draw.ClearBox(Date.info);
            Draw.WriteText("당신의 직업은 무엇입니까?", 32, 10);
            Draw.WriteText("전사 or 사냥꾼", 38, 12);
            Draw.WriteText("직업 : ", 35, 15);
            Console.SetCursorPosition(42, 15);
            string playerJob = null;
            playerJob = Console.ReadLine();
            if (playerJob == "전사" || playerJob == "사냥꾼")
            {
                PlayerInfo[1] = playerJob;
                check = true;

            }
        }

        return PlayerInfo;
    }
}


