using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDTproject
{
    internal class Inventory
    {
        public void MyInventory(ConsoleKeyInfo keyInfo, List<Items.Weapon> weapons, List<Items.Armor> armors, string View)
        {

            bool methodEnd = false;
            int inventorySelect = 0;
            string[] inventoryMenu = Date.inventory;
            string enhance = View == "Enhance" ? "MAX" : "착용";
            while (!methodEnd)
            {
                Draw.ClearBox(Date.map);
                int yPosition = 11;
                switch (inventorySelect)
                {
                    case 0:
                        if (weapons.Count == 0)
                        {
                            Draw.WriteText("보유중인 무기가 없습니다.", 20, 15);
                        }
                        else
                        {

                            Draw.WeaponList(enhance, View, yPosition);



                        }
                        break;

                    case 1:
                        if (armors.Count == 0)
                        {
                            Draw.WriteText("보유중인 방어구가 없습니다.", 20, 15);
                        }
                        else
                        {

                            Draw.ArmorList(enhance, View, yPosition);

                        }
                        break;


                }

                Draw.ClearBox(Date.selectCampMenu);

                Draw.SelectMenu(inventoryMenu, inventorySelect, 70, 7, 3, true);

                (bool selectEnter, int selectMenu) = KeyEvent.keyCheck(keyInfo, inventorySelect, inventoryMenu.Length);

                inventorySelect = selectMenu;

                if (selectEnter)
                {
                    switch (selectMenu)
                    {
                        case 0:
                            if (View == "inventory" && weapons.Count > 0) Equipment(keyInfo, weapons.Count, inventorySelect);
                            if (View == "sellShop" && weapons.Count > 0) Sell(keyInfo, weapons.Count, inventorySelect);
                            if (View == "Enhance" && weapons.Count > 0) Enhance(keyInfo, weapons.Count, inventorySelect);
                            break;
                        case 1:
                            if (View == "inventory" && armors.Count > 0) Equipment(keyInfo, armors.Count, inventorySelect);
                            if (View == "sellShop" && armors.Count > 0) Sell(keyInfo, armors.Count, inventorySelect);
                            if (View == "Enhance" && armors.Count > 0) Enhance(keyInfo, armors.Count, inventorySelect);
                            break;
                        case 2:
                            methodEnd = true;
                            break;
                    }
                }



            }
        }

        public void Equipment(ConsoleKeyInfo keyInfo, int menuLength, int itemClass)
        {
            bool methodEnd = false;
            int yPosition = 11;
            int selectEquipment = 0;

            while (!methodEnd)
            {
                Draw.selectItemCorsor(menuLength, selectEquipment, yPosition);


                (bool selectEnter, int selectMenu) = KeyEvent.keyCheck(keyInfo, selectEquipment, menuLength + 1);

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

        internal void Sell(ConsoleKeyInfo keyInfo, int menuLength, int itemClass)
        {
            int yPosition = 11;
            int selectEquipment = 0;

            while (true)
            {
                Draw.ClearBox(Date.map);
                if (itemClass == 0) Draw.WeaponList("착용", "sellShop", yPosition);
                else Draw.ArmorList("착용", "sellShop", yPosition);


                Draw.selectItemCorsor(menuLength, selectEquipment, yPosition);


                (bool selectEnter, int selectMenu) = KeyEvent.keyCheck(keyInfo, selectEquipment, menuLength + 1);

                selectEquipment = selectMenu;
                if (selectEnter)
                {
                    if (selectMenu == menuLength) return;

                    if (GameScene.Equipment[itemClass] > selectEquipment) --GameScene.Equipment[itemClass];
                    else if (GameScene.Equipment[itemClass] == selectEquipment) GameScene.Equipment[itemClass] = -1;

                    sellItem(selectEquipment, itemClass);

                    if (--menuLength == 0) return;


                }

            }
        }
        void sellItem(int selectEquipment, int itemClass)
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
            Draw.SimplePlayerInfo(GameScene.player);
        }
        internal void Enhance(ConsoleKeyInfo keyInfo, int menuLength, int itemClass)
        {

            int yPosition = 11;
            int selectEquipment = 0;

            while (true)
            {

                if (itemClass == 0) Draw.WeaponList("MAX", "Enhance", yPosition);
                else Draw.ArmorList("MAX", "Enhance", yPosition);

                Draw.selectItemCorsor(menuLength, selectEquipment, yPosition);


                (bool selectEnter, int selectMenu) = KeyEvent.keyCheck(keyInfo, selectEquipment, menuLength + 1);

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
                    Draw.SimplePlayerInfo(GameScene.player);

                    if (selectMenu == menuLength) return;

                }
            }
        }

        private void EnhanceWeapon(int selectEquipment)
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
                        Draw.UpdateInfo("골드가 모자랍니다.", 33, 12);
                        Thread.Sleep(1000);
                        Draw.DrawBox(Date.map);
                        Draw.DrawBox(Date.selectCampMenu);
                    }

                }
                else
                {
                    Draw.UpdateInfo("최대 강화 단계입니다.", 33, 12);
                    Thread.Sleep(1000);
                    Draw.DrawBox(Date.map);
                    Draw.DrawBox(Date.selectCampMenu);
                }

            }

        }
        private void EnhanceArmor(int selectEquipment)
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
                        Draw.UpdateInfo("골드가 모자랍니다.", 33, 12);
                        Thread.Sleep(1000);
                        Draw.DrawBox(Date.map);
                        Draw.DrawBox(Date.selectCampMenu);
                    }

                }
                else
                {
                    Draw.UpdateInfo("최대 강화 단계입니다.", 33, 12);
                    Thread.Sleep(1000);
                    Draw.DrawBox(Date.map);
                    Draw.DrawBox(Date.selectCampMenu);
                }
            }
        }
    }
}
