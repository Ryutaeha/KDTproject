using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Character;
using static Items;

namespace KDTproject
{
    internal static class Draw
    {
        internal static void WriteText(string text, int xOffset, int yOffset)
        {
            Console.SetCursorPosition(xOffset, yOffset);
            Console.Write(text);
        }

        /// <summary>
        /// 메뉴배열 ,선택되어 있는 메뉴, 문자열 위치, Y위치, 메뉴 사이간의 여백-(0)입력시 겹쳐서 출력
        /// </summary>
        /// <param name="menuString"></param>
        /// <param name="selectMenu"></param>
        /// <param name="menuPosition"></param>
        /// <param name="yPosition"></param>
        /// <param name="yBlank"></param>
        internal static void SelectMenu(string[] menuString, int selectMenu, int menuPosition, int yPosition, int yBlank, bool endSelect)
        {
            for (int i = 0; i < menuString.Length; i++)
            {
                if (i == selectMenu)
                {
                    if (i == menuString.Length - 1 && endSelect) WriteText(">", 68, 22);
                    else WriteText(">", menuPosition - 2, yPosition + i * yBlank);

                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                if (i == menuString.Length - 1 && endSelect) WriteText(menuString[i], 70, 22);
                else WriteText(menuString[i], menuPosition, yPosition + i * yBlank);
                Console.ResetColor();
            }
        }

        internal static void selectMap(int[,] mapPosition, int selectMap)
        {
            for (int i = 2; i < mapPosition.GetLength(0) + 2; i++)
            {
                if (i == selectMap)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                WriteText(Date.campMap[i - 2 + 2], mapPosition[i - 2, 0], mapPosition[i - 2, 1]);
                Console.ResetColor();
            }
        }


        /// <summary>
        /// int 배열[4] 너비, 높이 , 시작 X축, 시작 Y축
        /// </summary>
        internal static void DrawBox(int[] sceneArea)
        {
            // 상하 벽 그리기
            for (int i = 0; i < sceneArea[0] - 1; i++)
            {
                // i+시작 X, Y
                Console.SetCursorPosition(i + sceneArea[2], sceneArea[3]);
                Console.Write("-");
                // i+시작 X, Y시작점 + 높이
                Console.SetCursorPosition(i + sceneArea[2], sceneArea[3] + sceneArea[1]);
                Console.Write("-");
            }
            // 좌우 벽 그리기
            for (int i = 0; i < sceneArea[1] - 1; i++)
            {
                // 시작 X - 1 , i + 시작 Y축 +1
                Console.SetCursorPosition(sceneArea[2] - 1, i + sceneArea[3] + 1);
                Console.Write("|");
                // (시작 X -1) + 너비 , i +  시작 Y + 1
                Console.SetCursorPosition(sceneArea[2] - 1 + sceneArea[0], i + sceneArea[3] + 1);
                Console.Write("|");

            }
            Console.WriteLine();
        }
        internal static void ClearBox(int[] sceneArea)
        {
            for (int i = 1; i < sceneArea[1]; i++)
            {
                string clearText = "";
                for (int j = 1; j < sceneArea[0]; j++)
                {
                    clearText += " ";
                }
                WriteText(clearText, sceneArea[2], sceneArea[3] + i);
            }
        }
        /// <summary>
        /// 하나의 아이템을 꺼내는 메서드
        /// </summary>
        /// <param name="item"></param>
        /// <param name="y"></param>
        /// <param name="yMove"></param>
        /// <param name="position"></param>
        //아이템 배열, 시작 Y값 Y여백, 샵인지 인벤토리인지 
        internal static void ItemList(object item, int y, int yMove, string position)
        {
            IItem itemInterface = null;
            string viewTxt = null;
            if (item is Weapon)
            {
                itemInterface = (Weapon)item;
                Weapon weapon = (Weapon)item;
                WriteText($"{itemInterface.ItemName}+({weapon.Enhance})", 6, y + (yMove * 2));
                if (position == "Enhance") viewTxt = $"{weapon.EnhancePrice}";
            }
            if (item is Armor)
            {
                itemInterface = (Armor)item;
                Armor armor = (Armor)item;
                WriteText($"{itemInterface.ItemName}+({armor.Enhance})", 6, y + (yMove * 2));
                if (position == "Enhance") viewTxt = $"{armor.EnhancePrice}";
            }
            if (item is Potion)
            {
                itemInterface = (Potion)item;
                WriteText($"{itemInterface.ItemName}", 6, y + (yMove * 2));
            }
            WriteText($"{itemInterface.ItemAbility}", 19, y + (yMove * 2));
            WriteText($"{itemInterface.Information}", 26, y + (yMove * 2));

            if (position == "inventory") viewTxt = $"{itemInterface.Price / 10 * 7}";
            if (position == "sellShop") viewTxt = $"{itemInterface.Price / 10 * 7}";
            if (position == "buyShop") viewTxt = $"{itemInterface.Price}";

            WriteText(viewTxt, 50, y + (yMove * 2));

        }
        internal static void ItemListHeader(string state)
        {
            WriteText("장비 명", 6, 7);
            WriteText("능력치", 16, 7);
            WriteText("설명", 26, 7);
            WriteText("가격", 50, 7);
            WriteText(state, 55, 7);
            WriteText("===========================================================", 2, 9);
        }

        internal static void MonsterInfo(Mosters.Moster moster)
        {
            WriteText("Lv. " + moster.Level + " " + moster.Name, 6, 7);
            WriteText("체력 : " + moster.Health.ToString(), 6, 9);
            WriteText("공격력 : " + moster.AttackForce.ToString(), 20, 9);
            WriteText("방어력 : " + moster.Defense.ToString(), 34, 9);
            WriteText("한줄요약 : " + moster.Infomation, 6, 11);
        }

        internal static int BattleInfo(Mosters.Moster moster, int diceNum)
        {
            int mosterDemage = 0;
            double victoryPoint;
            //몬스터 데미지 : 몬스터 공격력 - 플레이어 방어력 + 방어구
            if (GameScene.Equipment[1] > -1)
                mosterDemage = Math.Max(moster.AttackForce - (int)((GameScene.player.Defense + GameScene.armors[GameScene.Equipment[1]].ItemAbility) * ((double)diceNum / 10)), 0);
            else
                mosterDemage = Math.Max(moster.AttackForce - (int)(GameScene.player.Defense * ((double)diceNum / 10)), 0);

            //플레이어 데미지 : 플레이어 공격력+무기 - 몬스터 방어력
            int PlayerDemage = 0;
            if (GameScene.Equipment[0] > -1)
                PlayerDemage = Math.Max((int)((GameScene.player.AttackForce + GameScene.weapons[GameScene.Equipment[0]].ItemAbility) * ((double)diceNum / 10)) - moster.Defense, 0);
            else
                PlayerDemage = Math.Max((int)(GameScene.player.AttackForce * ((double)diceNum / 10)) - moster.Defense, 0);



            if (PlayerDemage < 0) PlayerDemage = 0;

            int sumDamege = mosterDemage + PlayerDemage;
            if (sumDamege == 0) victoryPoint = 50;
            else victoryPoint = Math.Round(((double)PlayerDemage / (mosterDemage + PlayerDemage)) * 100, 2);

            WriteText("이길 확률 : ", 6, 15);
            WriteText("%", 25, 15);
            WriteText("[   ] : ", 6, 18);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            WriteText("TIP", 7, 18);
            Console.ResetColor();

            if (victoryPoint == 0)
            {

                Console.ForegroundColor = ConsoleColor.Red;
                WriteText("몬스터!!! 피해욧!!!!", 14, 18);
                WriteText($"{victoryPoint.ToString("N2"),6}", 18, 15);
                Console.ResetColor();
            }
            else if (victoryPoint < 31)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                WriteText("도망가는게 현명할수도 있습니다.", 14, 18);
                WriteText($"{victoryPoint.ToString("N2"),6}", 18, 15);
                Console.ResetColor();
            }
            else if (victoryPoint < 61)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                WriteText("갈땐 가더라도 주사위 한번은 괜찮잖아?", 14, 18);
                WriteText($"{victoryPoint.ToString("N2"),6}", 18, 15);
                Console.ResetColor();
            }
            else if (victoryPoint < 100)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                WriteText("이런 나 좀 쌜지도?", 14, 18);
                WriteText($"{victoryPoint.ToString("N2"),6}", 18, 15);
                Console.ResetColor();
            }
            else if (victoryPoint == 100)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                WriteText("이녀석 한대만 맞어 안되겠어 두대", 14, 18);
                WriteText($"{victoryPoint.ToString("N2"),6}", 18, 15);
                Console.ResetColor();
            }
            return (int)victoryPoint * 100;
        }

        internal static void BattleEndScene(int victoryProbability, Mosters.Moster moster, ConsoleKeyInfo keyInfo)
        {
            ClearBox(Date.map);

            bool victory;

            int spoils;

            int dropExp;
            int victoryCkeck = new Random().Next(0, 10001);
            if (victoryProbability < victoryCkeck)
            {
                victory = false;
                dropExp = (moster.DropExp / 2);
                GameScene.player.EXP += dropExp;
                spoils = (moster.Attack - GameScene.player.Defense < 10) ? 10 : (moster.Attack - GameScene.player.Defense);
                GameScene.player.Health -= spoils;
                if (GameScene.player.Health < 0) GameScene.player.Health = 0;
            }
            else
            {
                victory = true;
                dropExp = moster.DropExp;
                GameScene.player.EXP += dropExp;
                spoils = (moster.Gold * GameScene.player.GoldDrop / 100);
                GameScene.player.Gold += spoils + moster.Gold;


            }

            int LevelUp = GameScene.player.LevelUp();

            string battleResult = victory ? "Victory" : "Defeat";

            string playerLevel = $"Level : {GameScene.player.Level} {(LevelUp == 0 ? "" : "(+" + LevelUp + ")")}            EXP : {GameScene.player.EXP} {(dropExp == 0 ? "" : "(+" + dropExp + ")")}/{GameScene.player.NextEXP}";
            string playerHealth = $"체력 : {GameScene.player.Health} {(victory ? "" : "(-" + spoils + ")")} / {GameScene.player.MaxHealth} {(LevelUp == 0 ? "" : "(+" + LevelUp * (GameScene.player.JobClass == "전사" ? 10 : 5) + ")")} ";
            string playerGold = $"골드 : {GameScene.player.Gold} {(victory ? "(+" + spoils + ")" : "")}       추가 골드 획득량 : {GameScene.player.GoldDrop} {(LevelUp == 0 ? "" : "(+" + LevelUp * (GameScene.player.JobClass == "전사" ? 5 : 10) + ")")}%"; ;


            Console.ForegroundColor = victory ? ConsoleColor.Blue : ConsoleColor.Red;
            WriteText(battleResult.PadLeft((62 + battleResult.Length) / 2), 2, 8);
            Console.ResetColor();


            WriteText(playerLevel.PadLeft((62 + playerLevel.Length) / 2), 2, 11);

            WriteText(playerHealth.PadLeft((62 + playerHealth.Length) / 2), 2, 13);

            WriteText(playerGold.PadLeft((53 + playerGold.Length) / 2), 2, 15);

            SimplePlayerInfo(GameScene.player);

            WriteText("계속 하시려면 'Enter'키를 입력하세요!", 15, 22);

            while (true)
            {
                keyInfo = Console.ReadKey(true);
                // X 키가 입력됨
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    return;
                }

            }

        }

        internal static void WeaponList(string enhance, string view, int yPosition)
        {
            List<Weapon> weapons = GameScene.weapons;
            ItemListHeader(enhance);
            for (int i = 0; i < weapons.Count; i++)
            {
                ItemList(weapons[i], yPosition, i, view);
                if (GameScene.Equipment[0] == i && view != "Enhance") WriteText($"[E]", 55, yPosition + (i * 2));
                if (view == "Enhance") WriteText($"[{weapons[i].MaxEnhance}]", 55, yPosition + (i * 2));
            }
            WriteText("뒤로가기", 46, 22);
        }

        internal static void ArmorList(string enhance, string view, int yPosition)
        {
            List<Armor> armors = GameScene.armors;

            ItemListHeader(enhance);
            for (int i = 0; i < armors.Count; i++)
            {
                ItemList(armors[i], yPosition, i, view);
                if (GameScene.Equipment[1] == i) WriteText($"[E]", 55, yPosition + (i * 2));
                if (view == "Enhance") WriteText($"[{armors[i].MaxEnhance}]", 55, yPosition + (i * 2));
            }
            WriteText("뒤로가기", 46, 22);
        }

        internal static void selectItemCorsor(int menuLength, int selectItem, int yPosition)
        {
            for (int i = 0; i < menuLength + 1; i++)
            {
                if (i == menuLength && selectItem == i)
                {
                    WriteText(">", 44, 22);
                }

                if (i == selectItem && selectItem != menuLength)
                {
                    WriteText($">", 4, yPosition + (i * 2));
                    Console.ForegroundColor = ConsoleColor.White;

                }

                if (i != selectItem)
                {
                    WriteText($" ", 44, 22);
                    WriteText($" ", 4, yPosition + (i * 2));
                }
            }
        }
        internal static void SimplePlayerInfo(Player player)
        {
            WriteText($"Lv.     '{player.Name}' ", 4, 2);
            UpdateLevel(player.Level);
            WriteText($"HEALTH : ", 28, 2);
            UpdateHp(player.Health);
            UpdateMaxHealth(player.MaxHealth);
            WriteText($"GOLD : ", 55, 2);
            UpdateGold(player.Gold);
            WriteText($"LIVE DAY : {player.LiveDay}", 72, 2);
        }

        private static void UpdateLevel(int level)
        {
            WriteText($"{level}", 8, 2);
        }

        private static void UpdateMaxHealth(int maxHealth)
        {
            WriteText($"/ {maxHealth}", 42, 2);
        }

        internal static void UpdateHp(int hp)
        {
            WriteText("   ", 38, 2);
            if (hp.ToString().Length == 2) WriteText($" {hp}", 38, 2);
            else if (hp.ToString().Length == 1) WriteText($"  {hp}", 38, 2);
            else WriteText($"{hp}", 38, 2);
        }
        internal static void UpdateGold(int gold)
        {
            WriteText("     ", 62, 2);
            WriteText($"{gold}", 62, 2);
        }
        internal static void UpdateInfo(string infoMsg, int x, int y)
        {
            DrawBox(Date.info);
            ClearBox(Date.info);
            WriteText(infoMsg, x, y);

        }
        internal static void PlayerInfo(Character.Player player, ConsoleKeyInfo keyInfo)
        {

            WriteText(">", 68, 22);

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;

            WriteText("뒤로 가기", 70, 22);

            Console.ResetColor();

            WriteText($"Lv. {player.Level}  {player.Name} ({player.JobClass})", 4, 7);
            WriteText($"체력 : {player.Health}/{player.MaxHealth}", 4, 9);
            WriteText($"능력 : {player.Ability}", 4, 11);

            if (GameScene.Equipment[0] > -1) WriteText($"공격력 : {player.AttackForce} + ({GameScene.weapons[GameScene.Equipment[0]].ItemAbility})", 4, 13);
            else WriteText($"공격력 : {player.AttackForce}", 4, 13);


            if (GameScene.Equipment[1] > -1) WriteText($"방어력 : {player.Defense} + ({GameScene.armors[GameScene.Equipment[1]].ItemAbility})", 29, 13);
            else WriteText($"방어력 : {player.Defense}", 29, 13);


            WriteText($"돈 : {player.Gold} 원", 4, 15);

            WriteText($"추가 골드 획득량 : {player.GoldDrop}%", 29, 15);
            WriteText($"생존 요일 : {player.LiveDay}일차", 4, 17);

            while (true)
            {
                keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
            }


        }
        internal static int[,] MapPosition()
        {
            Random random = new Random();
            int[,] mapPosition = new int[3, 2];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int position;
                    if (j == 0)
                    {
                        position = random.Next(7, 55);
                    }
                    else
                    {
                        position = random.Next(7, 22);

                        if (i != 0 && mapPosition[i - 1, 0] == mapPosition[i, 0] && mapPosition[i - 1, 1] == mapPosition[i, 1])
                        {
                            continue;
                        }
                    }
                    mapPosition[i, j] = position;
                }
            }
            return mapPosition;
        }
    }

}
