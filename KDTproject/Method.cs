using KDTproject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Character;
using static CharacterSubMenu;
using static KDTproject.Mosters;


internal class keyEvent
{


    /// <summary>
    /// 메뉴배열 ,선택되어 있는 메뉴, 문자열 위치, Y위치, 메뉴 사이간의 여백-(0)입력시 겹쳐서 출력
    /// </summary>
    /// <param name="menuString"></param>
    /// <param name="selectMenu"></param>
    /// <param name="menuPosition"></param>
    /// <param name="yPosition"></param>
    /// <param name="yBlank"></param>
    internal static void SelectMenu(string[] menuString,int selectMenu, int menuPosition, int yPosition, int yBlank, bool endSelect)
    {
        for (int i = 0; i < menuString.Length; i++)
        {
            if (i == selectMenu)
            {
                if(i == menuString.Length - 1 && endSelect) Draw.WriteText(">", 68, 22);
                else Draw.WriteText(">", menuPosition - 2, yPosition + i * yBlank);

                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
            }
            if(i == menuString.Length - 1 && endSelect) Draw.WriteText(menuString[i], 70, 22);
            else  Draw.WriteText(menuString[i], menuPosition, yPosition + i* yBlank);
            Console.ResetColor();
        }
    }

    internal static void selectMap(int[,] mapPosition, int selectMap)
    {
        for (int i =2; i < mapPosition.GetLength(0)+2; i++)
        {
            if(i== selectMap)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
            }
            Draw.WriteText(Date.campMap[i-2 + 2], mapPosition[i-2, 0], mapPosition[i-2, 1]);
            Console.ResetColor();
        }
    }

    /// <summary>
    /// 키 이벤트, 현재 선택되어 있는 메뉴, 메뉴 총 길이
    /// </summary>
    /// <param name="keyInfo"></param>
    /// <param name="selectMenu"></param>
    /// <param name="menuLength"></param>
    /// <returns></returns>
    internal static (bool, int) keyCheck(ConsoleKeyInfo keyInfo, int selectMenu, int menuLength)
    {

        keyInfo = Console.ReadKey();

        //
        if (keyInfo.Key == ConsoleKey.UpArrow)
        {
            if (selectMenu > 0)
            {
                selectMenu--;
            }
        }

        // ↓ 키 처리
        if (keyInfo.Key == ConsoleKey.DownArrow)
        {
            if (selectMenu < menuLength -1)
            {
                selectMenu++;
            }
        }
        if (keyInfo.Key == ConsoleKey.Enter)
        {
            return (true, selectMenu);
        }
        return (false, selectMenu);
    }

    internal static void Skip(string boolName)
    {
        if (Console.KeyAvailable)
        {
            
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            // X 키가 입력됨
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                typeof(AnimationSkip).GetField(boolName).SetValue(null, false);
            }
        }
    }
}

internal class Draw
{
    internal static void WriteText(string text, int xOffset, int yOffset)
    {
        Console.SetCursorPosition(xOffset, yOffset);
        Console.Write(text);
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
        if(item is Armor)
        {
            itemInterface = (Armor)item;
            Armor armor = (Armor)item;
            WriteText($"{itemInterface.ItemName}+({armor.Enhance})", 6, y + (yMove * 2));
            if (position == "Enhance") viewTxt = $"{armor.EnhancePrice}";
        }
        if(item is Potion)
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
        Draw.WriteText("장비 명", 6, 7);
        Draw.WriteText("능력치", 16, 7);
        Draw.WriteText("설명", 26, 7);
        Draw.WriteText("가격", 50, 7);
        Draw.WriteText(state, 55, 7);
        Draw.WriteText("===========================================================", 2, 9);
    }

    internal static void MonsterInfo(Mosters.Moster moster)
    {
        Draw.WriteText("Lv. "+moster.Level+" "+moster.Name, 6, 7);
        Draw.WriteText("체력 : "+moster.Health.ToString(), 6, 9);
        Draw.WriteText("공격력 : " + moster.AttackForce.ToString(), 20, 9);
        Draw.WriteText("방어력 : " + moster.Defense.ToString(), 34, 9);
        Draw.WriteText("한줄요약 : " + moster.Infomation, 6, 11);
    }

    internal static int BattleInfo(Mosters.Moster moster, int diceNum)
    {
        int mosterDemage=0;
        double victoryPoint;
        //몬스터 데미지 : 몬스터 공격력 - 플레이어 방어력 + 방어구
        if (GameScene.Equipment[1] > -1)
            mosterDemage = Math.Max(moster.AttackForce - (int)((GameScene.player.Defense + GameScene.armors[GameScene.Equipment[1]].ItemAbility)*((double)diceNum/10)), 0);
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
        if (sumDamege == 0)  victoryPoint = 50;
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
        else if(victoryPoint < 61)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            WriteText("갈땐 가더라도 주사위 한번은 괜찮잖아?", 14, 18);
            WriteText($"{victoryPoint.ToString("N2"),6}", 18, 15);
            Console.ResetColor();
        }
        else if(victoryPoint < 100)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            WriteText("이런 나 좀 쌜지도?", 14, 18);
            WriteText($"{victoryPoint.ToString("N2"),6}", 18, 15);
            Console.ResetColor();
        }
        else if(victoryPoint == 100)
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
            dropExp =  moster.DropExp;
            GameScene.player.EXP += dropExp;
            spoils =  (moster.Gold * GameScene.player.GoldDrop / 100);
            GameScene.player.Gold += spoils + moster.Gold;


        }

        int LevelUp = GameScene.player.LevelUp();

        string battleResult = victory ? "Victory" : "Defeat";

        string playerLevel = $"Level : {GameScene.player.Level} {(LevelUp == 0 ? "" : "(+" + LevelUp + ")")}            EXP : {GameScene.player.EXP} {(dropExp == 0 ? "" : "(+" + dropExp + ")")}/{GameScene.player.NextEXP}";
        string playerHealth = $"체력 : {GameScene.player.Health} {(victory ? "" : "(-" + spoils + ")")} / {GameScene.player.MaxHealth} {(LevelUp == 0 ? "" : "(+" + LevelUp * (GameScene.player.JobClass == "전사" ? 10 : 5) + ")")} ";
        string playerGold = $"골드 : {GameScene.player.Gold} {(victory ? "(+" + spoils + ")" : "" )}       추가 골드 획득량 : {GameScene.player.GoldDrop} {(LevelUp == 0 ? "" : "(+" + LevelUp * (GameScene.player.JobClass == "전사" ? 5 : 10) + ")")}%"; ;


        Console.ForegroundColor = victory ? ConsoleColor.Blue : ConsoleColor.Red;
        WriteText(battleResult.PadLeft((62+battleResult.Length)/2), 2,8);
        Console.ResetColor();


        WriteText(playerLevel.PadLeft((62 + playerLevel.Length) / 2), 2, 11);

        WriteText(playerHealth.PadLeft((62 + playerHealth.Length) / 2), 2, 13);

        WriteText(playerGold.PadLeft((53 + playerGold.Length) / 2), 2, 15);

        SubMethod.SimplePlayerInfo(GameScene.player);

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

    internal static void WeaponList( string enhance, string view, int yPosition)
    {
        List<Weapon> weapons = GameScene.weapons;
        Draw.ItemListHeader(enhance);
        for (int i = 0; i < weapons.Count; i++)
        {
            ItemList(weapons[i], yPosition, i, view);
            if (GameScene.Equipment[0] == i && view != "Enhance") WriteText($"[E]", 55, yPosition + (i * 2));
            if (view == "Enhance") WriteText($"[{weapons[i].MaxEnhance}]", 55, yPosition + (i * 2));
        }
        Draw.WriteText("뒤로가기", 46, 22);
    }

    internal static void ArmorList(string enhance, string view, int yPosition)
    {
        List<Armor> armors = GameScene.armors;

        Draw.ItemListHeader(enhance);
        for (int i = 0; i < armors.Count; i++)
        {
            ItemList(armors[i], yPosition, i, view);
            if (GameScene.Equipment[1] == i) WriteText($"[E]", 55, yPosition + (i * 2));
            if (view == "Enhance") WriteText($"[{armors[i].MaxEnhance}]", 55, yPosition + (i * 2));
        }
        Draw.WriteText("뒤로가기", 46, 22);
    }
}

internal class subScene
{
    /// <summary>
    /// 전투 or 휴식
    /// </summary>
    /// <param name="keyInfo"></param>
    /// <returns></returns>
    internal static bool BattleKindSelect(ConsoleKeyInfo keyInfo)
    {
        
        int selectMenu = 0;
        while (true)
        {
            Draw.ClearBox(Date.map);
            Draw.ClearBox(Date.selectCampMenu);
            keyEvent.SelectMenu(Date.BattleKindSelect, selectMenu, 70, 7, 3, true);

            (bool sceneCheck, int selectMenuCheck) = keyEvent.keyCheck(keyInfo, selectMenu, Date.BattleKindSelect.Length);

            selectMenu = selectMenuCheck;
            if (sceneCheck)
            {
                switch (selectMenu)
                {
                    case 0:
                        if (BattleSceneSelect(keyInfo)) return true;
                        else break;
                        
                    case 1:
                        if(Rest(keyInfo)) return true;
                        else return false;                        
                    case 2:
                        return false;
                }
            }
        }

    }

    /// <summary>
    /// 맵고르기
    /// </summary>
    /// <param name="keyInfo"></param>
    internal static bool BattleSceneSelect(ConsoleKeyInfo keyInfo)
    {
        string[] battleSceneSelect = Date.BattleSceneSelect;
        int selectMenu = 0;
        bool clear;
        while (true)
        {
            Draw.ClearBox(Date.map);
            Draw.ClearBox(Date.selectCampMenu);
            keyEvent.SelectMenu(battleSceneSelect, selectMenu, 70, 7, 3, true);

            (bool sceneCheck, int selectMenuCheck) = keyEvent.keyCheck(keyInfo, selectMenu, battleSceneSelect.Length);

            selectMenu = selectMenuCheck;
            if (sceneCheck)
            {
                switch (selectMenu)
                {
                    case 0:
                        BattleScene(keyInfo, selectMenu);
                        return true;
                    case 1:
                        BattleScene(keyInfo, selectMenu);
                        return true;
                    case 2:
                        BattleScene(keyInfo, selectMenu);
                        return true;
                    case 3:
                        return false;
                }
            }
        }
    }


    private static void BattleScene(ConsoleKeyInfo keyInfo, int battleScene)
    {

        Moster moster = null;
        string[] mosterSelect;
        int randomMonster = RandomMonster();

        switch (battleScene)
        {
            case 0:
                mosterSelect = Date.Evil_Rins[randomMonster];
                moster = new Moster(mosterSelect[0], mosterSelect[1], mosterSelect[2], mosterSelect[3], mosterSelect[4],mosterSelect[5], mosterSelect[6]);
                break;
            case 1:
                mosterSelect = Date.Undead[0];
                moster = new Moster(mosterSelect[0], mosterSelect[1], mosterSelect[2], mosterSelect[3], mosterSelect[4], mosterSelect[5], mosterSelect[6]);
                break;
            case 2:
                mosterSelect = Date.Devil[0];
                moster = new Moster(mosterSelect[0], mosterSelect[1], mosterSelect[2], mosterSelect[3], mosterSelect[4], mosterSelect[5], mosterSelect[6]);
                break;
        }
        
        bool sceneEnd = false;
        string[] battleSceneSelect = Date.BattleScene;
        int selectMenu = 0;
        int diceNum = 10;
        int diceRollLimit = 2;
        bool endSelect = true;

        while (!sceneEnd)
        {

            Draw.ClearBox(Date.map);
            Draw.MonsterInfo(moster);
            int victoryProbability = Draw.BattleInfo(moster,diceNum);

            Draw.ClearBox(Date.selectCampMenu);

            

            keyEvent.SelectMenu(battleSceneSelect, selectMenu, 70, 7, 3, endSelect);

            (bool sceneCheck, int selectMenuCheck) = keyEvent.keyCheck(keyInfo, selectMenu, battleSceneSelect.Length);

            selectMenu = selectMenuCheck;
            if (sceneCheck)
            {
                switch (selectMenu)
                {
                    case 0:
                        Draw.BattleEndScene(victoryProbability, moster, keyInfo);
                        return;

                    case 1:
                        if(diceRollLimit > 0)
                        {
                            diceNum = SubMethod.RandomDice();
                            
                            if(--diceRollLimit == 0) selectMenu = 0;
                            endSelect = false;
                            Array.Resize(ref battleSceneSelect, battleSceneSelect.Length - 1);
                        
                        }
                        break;
                    case 2:
                        //도망가기는 레벨에 따라 다름 확률이 다름
                        //0~10까지 랜덤수를 뽑아 몬스터보다 높으면 도망 성공 플레이어 레벨따위는 장식임 ㅋㅋ
                        Draw.ClearBox(Date.map);
                        int runPoint = new Random().Next(0, 11);
                        if (runPoint > moster.Level)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Draw.WriteText("당신은 무사히 도망쳐 나와 야영지로 돌아왔습니다.", 9, 13);
                            Console.ResetColor();
                            Draw.WriteText("계속 하시려면 'Enter'키를 입력하세요!", 15, 22);

                            while (true)
                            {
                                keyInfo = Console.ReadKey(true);
                                // X 키가 입력됨
                                if (keyInfo.Key == ConsoleKey.Enter)
                                {
                                    break;
                                }

                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Draw.WriteText("당신은 도망에 실패하셨습니다. 전투가 시작됩니다.", 9, 13);
                            Console.ResetColor();
                            Draw.WriteText("계속 하시려면 'Enter'키를 입력하세요!", 15, 22);

                            while (true)
                            {
                                keyInfo = Console.ReadKey(true);
                                // X 키가 입력됨
                                if (keyInfo.Key == ConsoleKey.Enter)
                                {
                                    break;
                                }

                            }
                            Draw.BattleEndScene(victoryProbability, moster, keyInfo);
                        }

                        return;
                }
            }
           
        }
        
        return;
    }



    private static int RandomMonster()
    {
        Random random = new Random();
        int selectMoster = random.Next(0,20);
        if (selectMoster < 12) return 0;
        else if (selectMoster < 18) return 1;
        else return 2;
    }

    private static bool Rest(ConsoleKeyInfo keyInfo)
    {
        int xPositoin = 22;
        int yPosition = 10;
        int ypositionInfo = 13;
        int selectMenu = 0;
        int RestCondition = new Random().Next(0,111);
        while (true)
        {
            Draw.ClearBox(Date.map);
            Draw.ClearBox(Date.selectCampMenu);
            keyEvent.SelectMenu(Date.Rest, selectMenu, 70, 7, 3, true);
            (bool sceneCheck, int selectMenuCheck) = keyEvent.keyCheck(keyInfo, selectMenu, Date.campMenu.Length);
            selectMenu = selectMenuCheck;
            if (sceneCheck)
            {
                
                switch (selectMenu)
                {
                    case 0:
                        if(RestCondition == 110)
                        {
                            Draw.WriteText("너무 개운하다!", xPositoin, yPosition);

                            Draw.WriteText($"최대 체력 증가! +{20}", xPositoin, ypositionInfo);
                            GameScene.player.MaxHealth += 20;
                            GameScene.player.Health += 30;
                            if(GameScene.player.Health> GameScene.player.MaxHealth) GameScene.player.Health = GameScene.player.MaxHealth;
                        }
                        else if(RestCondition >10)
                        {
                            Draw.WriteText("정신이 맑아졌다!", xPositoin, yPosition);

                            Draw.WriteText($"최대 체력 증가! +{RestCondition / 10}", xPositoin, ypositionInfo);
                            GameScene.player.MaxHealth += RestCondition/10;
                            GameScene.player.Health += 10+(RestCondition/10);
                            if (GameScene.player.Health > GameScene.player.MaxHealth) GameScene.player.Health = GameScene.player.MaxHealth;
                        }
                        else
                        {
                            Draw.WriteText("아무일 없었다.!", xPositoin, yPosition);
                            GameScene.player.Health += 10 + (RestCondition / 10);
                            if (GameScene.player.Health > GameScene.player.MaxHealth) GameScene.player.Health = GameScene.player.MaxHealth;
                        }
                        break;
                    case 1:
                        if (RestCondition == 110)
                        {
                            Draw.WriteText("너무 개운하다!", xPositoin, yPosition);

                            Draw.WriteText($"방어력 증가! +{10}", xPositoin, ypositionInfo);
                            GameScene.player.Defense += 10;
                            GameScene.player.Health += 10;
                            if (GameScene.player.Health > GameScene.player.MaxHealth) GameScene.player.Health = GameScene.player.MaxHealth;
                        }
                        else if (RestCondition > 20)
                        {
                            Draw.WriteText("정신이 맑아졌다!", xPositoin, yPosition);

                            Draw.WriteText($"방어력 증가! +{RestCondition / 20}", xPositoin, ypositionInfo);
                            GameScene.player.Defense +=  RestCondition / 20;
                            GameScene.player.Health += 10;
                            if (GameScene.player.Health > GameScene.player.MaxHealth) GameScene.player.Health = GameScene.player.MaxHealth;
                        }
                        else
                        {
                            Draw.WriteText("아무일 없었다.!", xPositoin, yPosition);
                            GameScene.player.Health += 10;
                            if (GameScene.player.Health > GameScene.player.MaxHealth) GameScene.player.Health = GameScene.player.MaxHealth;
                        }
                        break;
                    case 2:
                        if (RestCondition == 110)
                        {
                            Draw.WriteText("너무 개운하다!", xPositoin, yPosition);

                            Draw.WriteText($"공격력 증가! +{10}", xPositoin, ypositionInfo);
                            GameScene.player.AttackForce += 10;
                            GameScene.player.Health += 10;
                            if (GameScene.player.Health > GameScene.player.MaxHealth) GameScene.player.Health = GameScene.player.MaxHealth;
                        }
                        else if (RestCondition > 20)
                        {
                            Draw.WriteText("정신이 맑아졌다!", xPositoin, yPosition);

                            Draw.WriteText($"공격력 증가! +{RestCondition / 10}", xPositoin, ypositionInfo);
                            GameScene.player.AttackForce += RestCondition / 20;
                            GameScene.player.Health += 10;
                            if (GameScene.player.Health > GameScene.player.MaxHealth) GameScene.player.Health = GameScene.player.MaxHealth;
                        }
                        else
                        {
                            Draw.WriteText("아무일 없었다.!", xPositoin, yPosition);
                            GameScene.player.Health += 10;
                            if (GameScene.player.Health > GameScene.player.MaxHealth) GameScene.player.Health = GameScene.player.MaxHealth;
                        }
                        break;
                    case 3:
                        if (RestCondition == 110)
                        {
                            Draw.WriteText("너무 개운하다!", xPositoin, yPosition);

                            Draw.WriteText($"체력 대폭 회복! +{GameScene.player.MaxHealth-GameScene.player.Health}", xPositoin, ypositionInfo);
                            GameScene.player.Health = GameScene.player.MaxHealth;
                        }
                        else if (RestCondition > 5)
                        {
                            Draw.WriteText("정신이 맑아졌다!", xPositoin, yPosition);

                            Draw.WriteText($"체력 회복! +{RestCondition / 10}", xPositoin, ypositionInfo);
                            GameScene.player.Health += (20+ (RestCondition / 5));
                            if (GameScene.player.Health > GameScene.player.MaxHealth) GameScene.player.Health = GameScene.player.MaxHealth;
                        }
                        else
                        {
                            Draw.WriteText("아무일 없었다.!", xPositoin, yPosition);
                            GameScene.player.Health += 20;
                            if (GameScene.player.Health > GameScene.player.MaxHealth) GameScene.player.Health = GameScene.player.MaxHealth;
                        }
                        break;
                    case 4:

                        return false;
                }
                if (selectMenu != 4)
                {
                    SubMethod.SimplePlayerInfo(GameScene.player);
                    Draw.WriteText("계속 하시려면 'Enter'키를 입력하세요!", 15, 22);

                    while (true)
                    {
                        keyInfo = Console.ReadKey(true);
                        // X 키가 입력됨
                        if (keyInfo.Key == ConsoleKey.Enter)
                        {
                            return true;
                        }

                    }

                }
            }
            
        }
    }

    internal static void Dead(ConsoleKeyInfo keyInfo)
    {
        Draw.ClearBox (Date.map);
        Console.ForegroundColor = ConsoleColor.Red;
        Draw.WriteText("당신은 죽었습니다", 20, 13);
        Console.ResetColor();
        Draw.WriteText("계속 하시려면 'Enter'키를 입력하세요!", 15, 22);

        while (true)
        {
            keyInfo = Console.ReadKey(true);
            // X 키가 입력됨
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                break;
            }

        }
    }




    internal static void PlayerInfo(Character.Player player, ConsoleKeyInfo keyInfo)
    {

        Draw.WriteText(">", 68, 22);

        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.White;

        Draw.WriteText("뒤로 가기", 70, 22);

        Console.ResetColor();

        Draw.WriteText($"Lv. {player.Level}  {player.Name} ({player.JobClass})", 4, 7);
        Draw.WriteText($"체력 : {player.Health}/{player.MaxHealth}", 4, 9);
        Draw.WriteText($"능력 : {player.Ability}", 4, 11);

        if (GameScene.Equipment[0] > -1) Draw.WriteText($"공격력 : {player.AttackForce} + ({GameScene.weapons[GameScene.Equipment[0]].ItemAbility})", 4, 13);
        else Draw.WriteText($"공격력 : {player.AttackForce}", 4, 13);


        if (GameScene.Equipment[1] > -1) Draw.WriteText($"방어력 : {player.Defense} + ({GameScene.armors[GameScene.Equipment[1]].ItemAbility})", 29, 13);
        else Draw.WriteText($"방어력 : {player.Defense}", 29, 13);


        Draw.WriteText($"돈 : {player.Gold} 원", 4, 15);

        Draw.WriteText($"추가 골드 획득량 : {player.GoldDrop}%",29,15);
        Draw.WriteText($"생존 요일 : {player.LiveDay}일차", 4, 17);

        while (true)
        {
            keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                break;
            }
        }

    }

    internal static void Shop(ConsoleKeyInfo keyInfo, int[] map, int[] selectCampMenu, List<object> shop, bool[] buyCheck)
    {
        bool methodEnd = false;
        int shopSelect = 0;
        string[] shopMenu = Date.shop;
        string[] merchant = Date.Merchant;
        int[] merchantTxt = { 30, 6, 4, 9 };
        while (!methodEnd)
        {
            Draw.DrawBox(map);
            Draw.DrawBox(selectCampMenu);
            Draw.ClearBox(map);
            int randomTex = new Random().Next(0,20);
            switch (shopSelect)
            {
                case 0:
                    for(int i = 0; i < merchant.Length; i++)
                    {
                        Draw.WriteText(merchant[i], 30, 6+i);
                    }
                    if (GameScene.player.LiveDay == 1) Draw.WriteText(Date.MerchantTxt[0], 5, 12);
                    else
                    {
                        if (randomTex >= 15)
                        {
                            for(int i = 0;i < 3; i++)
                            {
                                Draw.WriteText(Date.MerchantTxt[i+2], 4, 11+i);
                            }
                        }
                        else
                        {
                            Draw.WriteText(Date.MerchantTxt[1], 5, 12);
                        }
                    }
                    Draw.DrawBox(merchantTxt);
           

                    break;

                case 1:
                    for (int i = 0; i < merchant.Length; i++)
                    {
                        Draw.WriteText(merchant[i], 30, 6 + i);
                    }
                    if (GameScene.player.LiveDay == 1) Draw.WriteText(Date.MerchantTxt[0], 5, 12);
                    else
                    {
                        if (randomTex <=10) Draw.WriteText(Date.MerchantTxt[5], 5, 12);
                        
                        else Draw.WriteText(Date.MerchantTxt[6], 5, 12);

                    }
                    Draw.DrawBox(merchantTxt);
                    

                    break;

            }
            Draw.ClearBox(selectCampMenu);

            keyEvent.SelectMenu(shopMenu, shopSelect, 70, 7, 3, true);

            (bool selectEnter, int selectMenu) = keyEvent.keyCheck(keyInfo, shopSelect, shopMenu.Length);

            shopSelect = selectMenu;

            if (selectEnter)
            {
                Draw.ClearBox(Date.map);
                switch (selectMenu)
                {
                    case 0:
                        if (shop.Count > 0) SubMethod.BuyItem(keyInfo, shop, buyCheck);
                        
                        break;
                    case 1:
                        Inventory.MyInventory(keyInfo, map, selectCampMenu, GameScene.weapons, GameScene.armors, "sellShop");
                        break;
                    case 2:
                        methodEnd = true;
                        break;
                }
            }
        }
    }


}
internal class SubMethod
{
    internal static int[,] MapPosition()
    {
        Random random= new Random();
        int[,] mapPosition = new int[3,2];

        for(int i = 0;i < 3; i++)
        {
            for(int j = 0;j < 2; j++)
            {  
                int position;
                if (j== 0)
                {
                    position = random.Next(7, 55);
                }
                else
                {
                    position = random.Next(7,22);
                    /**/
                    if (i!=0 && mapPosition[i-1,0] == mapPosition[i, 0]&& mapPosition[i - 1, 1] == mapPosition[i, 1])
                    {
                        continue;
                    }
                }
                mapPosition[i,j] = position;
            }
        }
        return mapPosition;
    }
    /// <summary>
    /// 안내 메세지 x좌표 y좌표
    /// </summary>
    /// <param name="infoMsg"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    internal static void UpdateInfo(string infoMsg, int x, int y)
    {
        Draw.DrawBox(Date.info);
        Draw.ClearBox(Date.info);
        Draw.WriteText(infoMsg, x, y);
        
    }

    internal static void SimplePlayerInfo(Player player)
    {
        Draw.WriteText($"Lv.     '{player.Name}' ", 4, 2);
        UpdateLevel(player.Level);
        Draw.WriteText($"HEALTH : ", 28, 2);
        UpdateHp(player.Health);
        UpdateMaxHealth(player.MaxHealth);
        Draw.WriteText($"GOLD : ", 55, 2);
        UpdateGold(player.Gold);
        Draw.WriteText($"LIVE DAY : {player.LiveDay}", 72, 2);
    }

    private static void UpdateLevel(int level)
    {
        Draw.WriteText($"{level}", 8, 2);
    }

    private static void UpdateMaxHealth(int maxHealth)
    {
        Draw.WriteText($"/ {maxHealth}", 42, 2);
    }

    internal static void UpdateHp(int hp)
    {
        Draw.WriteText("   ", 38, 2);
        if (hp.ToString().Length == 2) Draw.WriteText($" {hp}", 38, 2);
        else if(hp.ToString().Length == 1) Draw.WriteText($"  {hp}", 38, 2);
        else Draw.WriteText($"{hp}", 38, 2);
    }
    internal static void UpdateGold(int gold)
    {
        Draw.WriteText("     ", 62, 2);
        Draw.WriteText($"{gold}", 62, 2);
    }

    internal static int UpdateShop(List<object> shop)
    {
        int randomWeapon = new Random().Next(0,5);
        string[] weaponItem = Date.Weapon[randomWeapon];
        Weapon weapon = new Weapon(weaponItem[0], weaponItem[1], weaponItem[2], weaponItem[3], weaponItem[4]);
        shop.Add(weapon);


        int randomArmor = new Random().Next(0,5);
        string[] armorItem = Date.Armor[randomArmor];
        Armor armor = new Armor(armorItem[0], armorItem[1], armorItem[2], armorItem[3], armorItem[4]);
        shop.Add(armor);


        for (int i = 0; i < 3; i++)
        {
            Potion potion = null;
            int randomPotion = new Random().Next(0,101);
            if (i == 0)
            {
                potion = ShopAddPotion(0);
            }
            else
            {
                if (randomPotion > 90)
                {
                    potion = ShopAddPotion(4);
                }
                else if(randomPotion > 80 && randomPotion < 91)
                {
                    potion = ShopAddPotion(3);
                }
                else if (randomPotion > 60 && randomPotion < 81)
                {
                    potion = ShopAddPotion(2);
                }
                else if (randomPotion > 61 && randomPotion < 40)
                {
                    potion = ShopAddPotion(1);
                }
            }
            if(potion != null)shop.Add(potion);
        }

        return shop.Count;
    }
    internal static Potion ShopAddPotion(int dateIndex)
    {
        string[] potionItem = Date.Potion[dateIndex];
        return new Potion(potionItem[0], potionItem[1], potionItem[2], potionItem[3]);
    }

    internal static void BuyItem(ConsoleKeyInfo keyInfo, List<object> shop, bool[] buyCheck)
    {
        bool methodEnd = false;
        int yPosition = 11;
        int selectItem = 0;


        
        

        while (!methodEnd)
        {
            Draw.DrawBox(Date.selectCampMenu);
            Draw.DrawBox(Date.map);
            Draw.ClearBox(Date.selectCampMenu);
            Draw.ClearBox(Date.map);
            Draw.ItemListHeader("재고");
            for (int i = 0; i < shop.Count; i++)
            {
                if (i == 0)
                {
                    Draw.ItemList(shop[i], yPosition, i, "buyShop");
                    if (buyCheck[i] == true) Draw.WriteText("품절", 55, yPosition + (i * 2));
                }
                else if (i == 1)
                {
                    Draw.ItemList(shop[i], yPosition, i, "buyShop");
                    if (buyCheck[i] == true) Draw.WriteText("품절", 55, yPosition + (i * 2));
                }
                else
                {
                    Draw.ItemList(shop[i], yPosition, i, "buyShop");
                    if (buyCheck[i] == true) Draw.WriteText("품절", 55, yPosition + (i * 2));
                }
            }
            Draw.WriteText("뒤로가기", 46, 22);
            selectItemCorsor(shop.Count, selectItem, yPosition);
            

            (bool selectEnter, int selectMenu) = keyEvent.keyCheck(keyInfo, selectItem, shop.Count +1);

            selectItem = selectMenu;
            if (selectEnter)
            {
                if (selectItem != shop.Count)
                {
                    buyCheck[selectItem] = Buy(shop[selectItem], buyCheck[selectItem]);
                }
                else methodEnd = true;

            }
        }
    }



    private static bool Buy(object shop, bool buyCheck)
    {
        if(shop is Weapon)
        {
            Weapon shopItem = (Weapon)shop;
            if(GameScene.player.Gold > shopItem.Price && buyCheck == false)
            {
                if(GameScene.weapons.Count < 5)
                {
                    if(buyCheck == false)
                    {
                    GameScene.player.Gold -= shopItem.Price;
                    GameScene.weapons.Add(shopItem);
                    buyCheck = true;
                    }
                    else
                    {
                        UpdateInfo("품절된 상품입니다.", 33, 12);
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    UpdateInfo("장비칸이 꽉 찻습니다.",33,12);
                    Thread.Sleep(1000);
                }
            }
            else if(buyCheck==false)
            {
                UpdateInfo("골드가 모자랍니다.", 33, 12);
                Thread.Sleep(1000);
            }
            else
            {
                UpdateInfo("품절된 상품입니다.", 33, 12);
                Thread.Sleep(1000);
            }
        }
        if(shop is Armor)
        {
            Armor shopItem = (Armor)shop;
            if (GameScene.player.Gold > shopItem.Price && buyCheck == false)
            {
                if (GameScene.armors.Count < 5)
                {
                    if (buyCheck == false)
                    {
                        GameScene.player.Gold -= shopItem.Price;
                        GameScene.armors.Add(shopItem);
                        buyCheck = true;
                    }
                    else
                    {
                        UpdateInfo("품절된 상품입니다.", 33, 12);
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    UpdateInfo("장비칸이 꽉 찻습니다.", 33, 12);
                    Thread.Sleep(1000);
                }
            }
            else if (buyCheck == false)
            {
                UpdateInfo("골드가 모자랍니다.", 33, 12);
                Thread.Sleep(1000);
            }
            else
            {
                UpdateInfo("품절된 상품입니다.", 33, 12);
                Thread.Sleep(1000);
            }

        }
        if(shop is Potion)
        {
            Potion shopItem = (Potion)shop;
            if (GameScene.player.Gold > shopItem.Price && buyCheck == false)
            {
                if (buyCheck == false)
                { 
                GameScene.player.Gold -= shopItem.Price;
                shopItem.Use(GameScene.player);
                buyCheck = true;
                }
                else
                {
                    UpdateInfo("품절된 상품입니다.", 33, 12);
                    Thread.Sleep(1000);
                }
            }
            else if (buyCheck == false)
            {
                UpdateInfo("골드가 모자랍니다.", 33, 12);
                Thread.Sleep(1000);
            }
            else
            {
                UpdateInfo("품절된 상품입니다.", 33, 12);
                Thread.Sleep(1000);
            }
        }
        SimplePlayerInfo(GameScene.player);
        return buyCheck;
    }

    internal static void selectItemCorsor(int menuLength, int selectItem, int yPosition)
    {
        for (int i = 0; i < menuLength + 1; i++)
        {
            if (i == menuLength && selectItem == i)
            {
                Draw.WriteText(">", 44, 22);
            }

            if (i == selectItem && selectItem != menuLength)
            {
                Draw.WriteText($">", 4, yPosition + (i * 2));
                Console.ForegroundColor = ConsoleColor.White;
                
            }

            if (i != selectItem)
            {
                Draw.WriteText($" ", 44, 22);
                Draw.WriteText($" ", 4, yPosition + (i * 2));
            }
        }
    }

    internal static void Sell(ConsoleKeyInfo keyInfo, int menuLength, int itemClass)
    {
        int yPosition = 11;
        int selectEquipment = 0;

        while (true)
        {
            Draw.ClearBox(Date.map);
            if (itemClass == 0) Draw.WeaponList("착용","sellShop", yPosition);
            else Draw.ArmorList("착용", "sellShop", yPosition);


            selectItemCorsor(menuLength, selectEquipment, yPosition);


            (bool selectEnter, int selectMenu) = keyEvent.keyCheck(keyInfo, selectEquipment, menuLength + 1);

            selectEquipment = selectMenu;
            if (selectEnter)
            {
                if (selectMenu == menuLength) return;
                
                if (GameScene.Equipment[itemClass] > selectEquipment) --GameScene.Equipment[itemClass];
                else if (GameScene.Equipment[itemClass] == selectEquipment) GameScene.Equipment[itemClass] = -1;
                
                sellItem(selectEquipment,itemClass);

                if(--menuLength==0) return;


            }

        }
    }
    static void sellItem(int selectEquipment, int itemClass)
    {
        if (itemClass == 0)
        {
            GameScene.player.Gold += ((GameScene.weapons[selectEquipment].Price / 10) * 7);
            GameScene.weapons.RemoveAt(selectEquipment);
        }
        else if (itemClass == 1)
        {
            GameScene.player.Gold += (GameScene.armors[selectEquipment].Price / 10) * 7;
            GameScene.armors.RemoveAt(selectEquipment);
        }
        SimplePlayerInfo(GameScene.player);
    }


    internal static void Enhance(ConsoleKeyInfo keyInfo, int menuLength, int itemClass)
    {

        int yPosition = 11;
        int selectEquipment = 0;

        while (true)
        {

            if (itemClass == 0) Draw.WeaponList("MAX","Enhance", yPosition);
            else Draw.ArmorList("MAX","Enhance", yPosition);

            selectItemCorsor(menuLength, selectEquipment, yPosition);


            (bool selectEnter, int selectMenu) = keyEvent.keyCheck(keyInfo, selectEquipment, menuLength + 1);

            selectEquipment = selectMenu;
            if (selectEnter)
            {
                
                    if (itemClass == 0)
                    {
                        EnhanceWeapon(selectEquipment);
                    }
                    else if (itemClass == 1)
                    {
                        EnhanceArmor(selectEquipment);
                    }
                    SimplePlayerInfo (GameScene.player);

                if (selectMenu == menuLength) return;

            }
        }
    }

    private static void EnhanceWeapon(int selectEquipment)
    {
        if (selectEquipment != GameScene.weapons.Count)
        {
            if (GameScene.weapons[selectEquipment].Enhance < GameScene.weapons[selectEquipment].MaxEnhance)
            {
                if (GameScene.player.Gold > GameScene.weapons[selectEquipment].EnhancePrice)
                {
                    GameScene.player.Gold -= GameScene.weapons[selectEquipment].EnhancePrice;
                    if (GameScene.weapons[selectEquipment].EnhancePrice < 10)
                    {
                        GameScene.weapons[selectEquipment].EnhancePrice += 3;
                    }
                    else
                    {
                        GameScene.weapons[selectEquipment].EnhancePrice += (GameScene.weapons[selectEquipment].EnhancePrice / 10);
                    }
                    GameScene.weapons[selectEquipment].Enhance++;
                    GameScene.weapons[selectEquipment].ItemAbility += GameScene.weapons[selectEquipment].Enhance;
                }
                else
                {
                    UpdateInfo("골드가 모자랍니다.", 33, 12);
                    Thread.Sleep(1000);
                    Draw.DrawBox(Date.map);
                    Draw.DrawBox(Date.selectCampMenu);
                }

            }
            else
            {
                UpdateInfo("최대 강화 단계입니다.", 33, 12);
                Thread.Sleep(1000);
                Draw.DrawBox(Date.map);
                Draw.DrawBox(Date.selectCampMenu);
            }

        }

    }
    private static void EnhanceArmor(int selectEquipment)
    {
        if (selectEquipment != GameScene.armors.Count)
        {
            if (GameScene.armors[selectEquipment].Enhance < GameScene.armors[selectEquipment].MaxEnhance)
            {
                if (GameScene.player.Gold > GameScene.armors[selectEquipment].EnhancePrice)
                {
                    GameScene.player.Gold -= GameScene.armors[selectEquipment].EnhancePrice;
                    if (GameScene.armors[selectEquipment].EnhancePrice < 10)
                    {
                        GameScene.armors[selectEquipment].EnhancePrice += 3;
                    }
                    else
                    {
                        GameScene.armors[selectEquipment].EnhancePrice += (GameScene.armors[selectEquipment].EnhancePrice / 10);
                    }
                    GameScene.armors[selectEquipment].Enhance++;
                    GameScene.armors[selectEquipment].ItemAbility += GameScene.armors[selectEquipment].Enhance;
                }
                else
                {
                    UpdateInfo("골드가 모자랍니다.", 33, 12);
                    Thread.Sleep(1000);
                    Draw.DrawBox(Date.map);
                    Draw.DrawBox(Date.selectCampMenu);
                }

            }
            else
            {
                UpdateInfo("최대 강화 단계입니다.", 33, 12);
                Thread.Sleep(1000);
                Draw.DrawBox(Date.map);
                Draw.DrawBox(Date.selectCampMenu);
            }
        }
    }

    internal static string[] PlayerCreate()
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
            if(playerName != "" && playerName.Length <= 4)
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
            if (playerJob =="전사" || playerJob == "사냥꾼")
            {
                PlayerInfo[1] = playerJob;
                check = true;

            }
        }

        return PlayerInfo;
    }

    internal static int RandomDice()
    {
        int diceRoll = new Random().Next(1,21);
        return diceRoll;
    }




}

