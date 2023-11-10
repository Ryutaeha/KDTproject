using KDTproject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Character;
using static CharacterSubMenu;


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

}

internal class subScene
{


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

        if (Program.Equipment[0] > -1) Draw.WriteText($"공격력 : {player.AttackForce} + ({Program.weapons[Program.Equipment[0]].ItemAbility})", 4, 13);
        else Draw.WriteText($"공격력 : {player.AttackForce}", 4, 13);


        if (Program.Equipment[1] > -1) Draw.WriteText($"방어력 : {player.Defense} + ({Program.armors[Program.Equipment[1]].ItemAbility})", 29, 13);
        else Draw.WriteText($"방어력 : {player.Defense}", 29, 13);


        Draw.WriteText($"돈 : {player.Gold} 원", 4, 15);
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
                    if (Program.player.LiveDay == 1) Draw.WriteText(Date.MerchantTxt[0], 5, 12);
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
                    if (Program.player.LiveDay == 1) Draw.WriteText(Date.MerchantTxt[0], 5, 12);
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
                        Inventory.MyInventory(keyInfo, map, selectCampMenu, Program.weapons, Program.armors, "sellShop");
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
        Draw.WriteText($"'{player.Name}' Level : {player.Level}", 4, 2);
        Draw.WriteText($"HEALTH : ", 28, 2);
        UpdateHp(player.Health);
        Draw.WriteText($"/ {player.MaxHealth}", 42, 2);
        Draw.WriteText($"GOLD : ", 55, 2);
        UpdateGold(player.Gold);
        Draw.WriteText($"LIVE DAY : {player.LiveDay}", 72, 2);
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
        int randomWeapon = new Random().Next(0,4);
        string[] weaponItem = Date.Weapon[randomWeapon];
        Weapon weapon = new Weapon(weaponItem[0], weaponItem[1], weaponItem[2], weaponItem[3], weaponItem[4]);
        shop.Add(weapon);


        int randomArmor = new Random().Next(0,4);
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
        

        while (!methodEnd)
        {
            selectItemCorsor(shop.Count, selectItem, yPosition);
            

            (bool selectEnter, int selectMenu) = keyEvent.keyCheck(keyInfo, selectItem, shop.Count +1);

            selectItem = selectMenu;
            if (selectEnter)
            {
                if (selectItem != shop.Count)
                {
                    buyCheck[selectItem] = Buy(shop[selectItem], buyCheck[selectItem]);
                }
                methodEnd = true;
            }
        }
    }



    private static bool Buy(object shop, bool buyCheck)
    {
        if(shop is Weapon)
        {
            Weapon shopItem = (Weapon)shop;
            if(Program.player.Gold > shopItem.Price)
            {
                if(Program.weapons.Count < 5)
                {
                    if(buyCheck == false)
                    {
                    Program.player.Gold -= shopItem.Price;
                    Program.weapons.Add(shopItem);
                    UpdateGold(Program.player.Gold);
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
            else
            {
                UpdateInfo("골드가 모자랍니다.", 33, 12);
                Thread.Sleep(1000);
            }
        }
        if(shop is Armor)
        {
            Armor shopItem = (Armor)shop;
            if (Program.player.Gold > shopItem.Price)
            {
                if (Program.armors.Count < 5)
                {
                    if (buyCheck == false)
                    {
                        Program.player.Gold -= shopItem.Price;
                        Program.armors.Add(shopItem);
                        UpdateGold(Program.player.Gold);
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
            else
            {
                UpdateInfo("골드가 모자랍니다.", 33, 12);
                Thread.Sleep(1000);
            }
            
        }
        if(shop is Potion)
        {
            Potion shopItem = (Potion)shop;
            if (Program.player.Gold > shopItem.Price)
            {
                if (buyCheck == false)
                { 
                Program.player.Gold -= shopItem.Price;
                shopItem.Use(Program.player);
                UpdateGold(Program.player.Gold);
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
                UpdateInfo("골드가 모자랍니다.", 33, 12);
                Thread.Sleep(1000);
            }
        }
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
        bool methodEnd = false;
        int yPosition = 11;
        int selectEquipment = 0;

        while (!methodEnd)
        {
            selectItemCorsor(menuLength, selectEquipment, yPosition);


            (bool selectEnter, int selectMenu) = keyEvent.keyCheck(keyInfo, selectEquipment, menuLength + 1);

            selectEquipment = selectMenu;
            if (selectEnter)
            {
                if (Program.Equipment[itemClass] == selectEquipment)
                {
                    Program.Equipment[itemClass] = -1;
                    if (itemClass == 0)
                    {
                        sellWeapon(selectEquipment);
                    }
                    else if (itemClass == 1)
                    {
                        sellArmor(selectEquipment);
                    }
                }
                else if (selectEquipment != menuLength)
                {
                    if (itemClass == 0)
                    {
                        sellWeapon(selectEquipment);
                    }
                    else if (itemClass == 1)
                    {
                        sellArmor(selectEquipment);
                    }
                }

                    methodEnd = true;
            }
        }
    }
    static void sellWeapon(int selectEquipment)
    {
        UpdateGold(Program.player.Gold += ((Program.weapons[selectEquipment].Price / 10) * 7));
        Program.weapons.RemoveAt(selectEquipment);
    }
    static void sellArmor(int selectEquipment)
    {
        UpdateGold(Program.player.Gold += (Program.armors[selectEquipment].Price / 10) * 7);
        Program.armors.RemoveAt(selectEquipment);
    }

    internal static void Enhance(ConsoleKeyInfo keyInfo, int menuLength, int itemClass)
    {
        bool methodEnd = false;
        int yPosition = 11;
        int selectEquipment = 0;

        while (!methodEnd)
        {
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
                

                methodEnd = true;
            }
        }
    }

    private static void EnhanceWeapon(int selectEquipment)
    {
        if (selectEquipment != Program.weapons.Count)
        {
            if (Program.weapons[selectEquipment].Enhance < Program.weapons[selectEquipment].MaxEnhance)
            {
                if (Program.player.Gold > Program.weapons[selectEquipment].EnhancePrice)
                {
                    UpdateGold(Program.player.Gold -= Program.weapons[selectEquipment].EnhancePrice);
                    if (Program.weapons[selectEquipment].EnhancePrice < 10)
                    {
                        Program.weapons[selectEquipment].EnhancePrice += 3;
                    }
                    else
                    {
                        Program.weapons[selectEquipment].EnhancePrice += (Program.weapons[selectEquipment].EnhancePrice / 10);
                    }
                    Program.weapons[selectEquipment].Enhance++;
                    Program.weapons[selectEquipment].ItemAbility += Program.weapons[selectEquipment].Enhance;
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
        if (selectEquipment != Program.armors.Count)
        {
            if (Program.armors[selectEquipment].Enhance < Program.armors[selectEquipment].MaxEnhance)
            {
                if (Program.player.Gold > Program.armors[selectEquipment].EnhancePrice)
                {
                    UpdateGold(Program.player.Gold -= Program.armors[selectEquipment].EnhancePrice);
                    if (Program.armors[selectEquipment].EnhancePrice < 10)
                    {
                        Program.armors[selectEquipment].EnhancePrice += 3;
                    }
                    else
                    {
                        Program.armors[selectEquipment].EnhancePrice += (Program.armors[selectEquipment].EnhancePrice / 10);
                    }
                    Program.armors[selectEquipment].Enhance++;
                    Program.armors[selectEquipment].ItemAbility += Program.armors[selectEquipment].Enhance;
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
}
