using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KDTproject
{
    internal class Shop
    {
        internal void ShopInfo(ConsoleKeyInfo keyInfo, int[] map, int[] selectCampMenu, List<object> shop, bool[] buyCheck)
        {
            Inventory inventory = new Inventory();
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
                int randomTex = new Random().Next(0, 20);
                switch (shopSelect)
                {
                    case 0:
                        for (int i = 0; i < merchant.Length; i++)
                        {
                            Draw.WriteText(merchant[i], 30, 6 + i);
                        }
                        if (GameScene.player.LiveDay == 1) Draw.WriteText(Date.MerchantTxt[0], 5, 12);
                        else
                        {
                            if (randomTex >= 15)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    Draw.WriteText(Date.MerchantTxt[i + 2], 4, 11 + i);
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
                            if (randomTex <= 10) Draw.WriteText(Date.MerchantTxt[5], 5, 12);

                            else Draw.WriteText(Date.MerchantTxt[6], 5, 12);

                        }
                        Draw.DrawBox(merchantTxt);


                        break;

                }
                Draw.ClearBox(selectCampMenu);

                Draw.SelectMenu(shopMenu, shopSelect, 70, 7, 3, true);

                (bool selectEnter, int selectMenu) = KeyEvent.keyCheck(keyInfo, shopSelect, shopMenu.Length);

                shopSelect = selectMenu;

                if (selectEnter)
                {
                    Draw.ClearBox(Date.map);
                    switch (selectMenu)
                    {
                        case 0:
                            if (shop.Count > 0) BuyItem(keyInfo, shop, buyCheck);

                            break;
                        case 1:
                            inventory.MyInventory(keyInfo, GameScene.weapons, GameScene.armors, "sellShop");
                            break;
                        case 2:
                            methodEnd = true;
                            break;
                    }
                }
            }

        }

        internal int UpdateShop(List<object> shop)
        {
            int randomWeapon = new Random().Next(0, 5);
            string[] weaponItem = Date.Weapon[randomWeapon];
            Items.Weapon weapon = new Items.Weapon(weaponItem[0], weaponItem[1], weaponItem[2], weaponItem[3], weaponItem[4]);
            shop.Add(weapon);


            int randomArmor = new Random().Next(0, 5);
            string[] armorItem = Date.Armor[randomArmor];
            Items.Armor armor = new(armorItem[0], armorItem[1], armorItem[2], armorItem[3], armorItem[4]);
            shop.Add(armor);


            for (int i = 0; i < 3; i++)
            {
                Items.Potion potion = null;
                int randomPotion = new Random().Next(0, 101);
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
                    else if (randomPotion > 80 && randomPotion < 91)
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
                if (potion != null) shop.Add(potion);
            }

            return shop.Count;
        }
        internal Items.Potion ShopAddPotion(int dateIndex)
        {
            string[] potionItem = Date.Potion[dateIndex];
            return new Items.Potion(potionItem[0], potionItem[1], potionItem[2], potionItem[3]);
        }

        internal void BuyItem(ConsoleKeyInfo keyInfo, List<object> shop, bool[] buyCheck)
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
                Draw.selectItemCorsor(shop.Count, selectItem, yPosition);


                (bool selectEnter, int selectMenu) = KeyEvent.keyCheck(keyInfo, selectItem, shop.Count + 1);

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



        private bool Buy(object shop, bool buyCheck)
        {
            if (shop is Items.Weapon)
            {
                Items.Weapon shopItem = (Items.Weapon)shop;
                if (GameScene.player.Gold > shopItem.Price && buyCheck == false)
                {
                    if (GameScene.weapons.Count < 5)
                    {
                        if (buyCheck == false)
                        {
                            GameScene.player.Gold -= shopItem.Price;
                            GameScene.weapons.Add(shopItem);
                            buyCheck = true;
                        }
                        else
                        {
                            Draw.UpdateInfo("품절된 상품입니다.", 33, 12);
                            Thread.Sleep(1000);
                        }
                    }
                    else
                    {
                        Draw.UpdateInfo("장비칸이 꽉 찻습니다.", 33, 12);
                        Thread.Sleep(1000);
                    }
                }
                else if (buyCheck == false)
                {
                    Draw.UpdateInfo("골드가 모자랍니다.", 33, 12);
                    Thread.Sleep(1000);
                }
                else
                {
                    Draw.UpdateInfo("품절된 상품입니다.", 33, 12);
                    Thread.Sleep(1000);
                }
            }
            if (shop is Items.Armor)
            {
                Items.Armor shopItem = (Items.Armor)shop;
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
                            Draw.UpdateInfo("품절된 상품입니다.", 33, 12);
                            Thread.Sleep(1000);
                        }
                    }
                    else
                    {
                        Draw.UpdateInfo("장비칸이 꽉 찻습니다.", 33, 12);
                        Thread.Sleep(1000);
                    }
                }
                else if (buyCheck == false)
                {
                    Draw.UpdateInfo("골드가 모자랍니다.", 33, 12);
                    Thread.Sleep(1000);
                }
                else
                {
                    Draw.UpdateInfo("품절된 상품입니다.", 33, 12);
                    Thread.Sleep(1000);
                }

            }
            if (shop is Items.Potion)
            {
                Items.Potion shopItem = (Items.Potion)shop;
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
                        Draw.UpdateInfo("품절된 상품입니다.", 33, 12);
                        Thread.Sleep(1000);
                    }
                }
                else if (buyCheck == false)
                {
                    Draw.UpdateInfo("골드가 모자랍니다.", 33, 12);
                    Thread.Sleep(1000);
                }
                else
                {
                    Draw.UpdateInfo("품절된 상품입니다.", 33, 12);
                    Thread.Sleep(1000);
                }
            }
            Draw.SimplePlayerInfo(GameScene.player);
            return buyCheck;
        }

    }
}
