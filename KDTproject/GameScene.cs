using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Character;

namespace KDTproject
{
    internal class GameScene
    {   


        //가지고 있는 무기
        public static List<CharacterSubMenu.Weapon> weapons = new List<CharacterSubMenu.Weapon>();
        //가지고 있는 방어구
        public static List<CharacterSubMenu.Armor> armors = new List<CharacterSubMenu.Armor>();
        public static int[] Equipment =  { -1, -1 };
        internal static Player player = null;


        public static ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();

        internal static void StartScene()
        {
            
            AnimationSkip.firstLord = true;
            int selectMenu = 0;
            bool sceneEnd = false;
            string[] startLogo = Date.startLogo;
            string[] startMenu = Date.startMenu;
            string[] playerCreate = null;
            while (!sceneEnd)
            {
                Draw.ClearBox(Date.windowWidth);

                // 화면 벽그리기 순서대로 넓이, 높이, 시작 X좌표, 시작 Y좌표
                Draw.DrawBox(Date.windowWidth);

                // 로고 연출
                for (int i = 0; i < startLogo.Length; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Draw.WriteText(startLogo[i], 20, 2 + i);
                    if (AnimationSkip.firstLord)
                    {
                        Thread.Sleep(200);
                    }
                    //x키를 눌렸을시 스킵
                    keyEvent.Skip("firstLord");

                    Console.ResetColor();
                }

                //첫 로딩이 끝나면 애니메이션 진행안함
                AnimationSkip.firstLord = false;


                Console.ForegroundColor = ConsoleColor.Magenta;
                Draw.WriteText("by 태하 With Sparta", 37, 14);
                Console.ResetColor();

                Draw.WriteText("Version 2023.11", 73, 24);




                // 플레이어 선택 창
                int[] selectBtn = { 30, 4, 31, 18 };
                Draw.DrawBox(selectBtn);

                // 선택 메뉴 출력
                keyEvent.SelectMenu(startMenu, selectMenu, 42, 19, 1, false);

                // 플레이어의 선택을 Key이벤트로 보내주는 메서드
                (bool sceneCheck, int selectMenuCheck) = keyEvent.keyCheck(keyInfo, selectMenu, startMenu.Length);

                selectMenu = selectMenuCheck;
                if (sceneCheck)
                {
                    switch (selectMenu)
                    {
                        case 0:
                            playerCreate = SubMethod.PlayerCreate();
                            CampScene(selectMenu, playerCreate);
                            break;
                        case 1:
                            SubMethod.UpdateInfo("구현중...", 40, 12);
                            Thread.Sleep(1000);
                            break;
                        case 2:
                            Console.SetCursorPosition(0, 26);
                            sceneEnd = true;
                            break;
                    }
                }
            }

        }

        // 
        internal static void CampScene(int gmaeCheck, string[] playerCreate)
        {

            List<object> shop = new List<object>();
            bool[] buyCheck = null;


            bool newGame;
            int selectMenu = 0;
            bool sceneEnd = false;
            int[,] mapPosition = SubMethod.MapPosition();
            switch (gmaeCheck)
            {
                case 0:
                    player = new Character.Player(playerCreate[0], playerCreate[1]);
                    break;
                case 1:
                    break;
            }

            while (!sceneEnd)
            {
                if (player.IsDead)
                {
                    subScene.Dead(keyInfo);
                    return;
                }
                Draw.ClearBox(Date.windowWidth);

                //박스 그리기
                Draw.DrawBox(Date.windowWidth);

                // 진행 박스 그리기

                Draw.DrawBox(Date.map);

                // 맵 선택창 띄우기
                keyEvent.selectMap(mapPosition, selectMenu);

                //캐릭터 정보 출력
                SubMethod.SimplePlayerInfo(player);

                // 메뉴 선택박스 그리기

                Draw.DrawBox(Date.selectCampMenu);

                //메뉴창 띄우는 메서드
                keyEvent.SelectMenu(Date.campMenu, selectMenu, 70, 7, 3, true);

                //메뉴 선택 메서드
                (bool sceneCheck, int selectMenuCheck) = keyEvent.keyCheck(keyInfo, selectMenu, Date.campMenu.Length);

                selectMenu = selectMenuCheck;
                if (sceneCheck)
                {
                    switch (selectMenu)
                    {
                        case 0:
                            //플레이어 정보
                            Draw.ClearBox(Date.map);
                            Draw.ClearBox(Date.selectCampMenu);
                            subScene.PlayerInfo(player, keyInfo);
                            break;
                        case 1:
                            //인벤토리
                            Draw.ClearBox(Date.map);
                            Inventory.MyInventory(keyInfo, Date.map, Date.selectCampMenu, weapons, armors, "inventory");
                            break;
                        case 2:
                            //상점
                            subScene.Shop(keyInfo, Date.map, Date.selectCampMenu, shop, buyCheck);
                            break;
                        case 3:
                            //강화
                            Draw.ClearBox(Date.map);
                            Inventory.MyInventory(keyInfo, Date.map, Date.selectCampMenu, weapons, armors, "Enhance");
                            break;
                        case 4:
                            //탐험
                            if (subScene.BattleKindSelect(keyInfo))
                            {
                                selectMenu = 0;
                                mapPosition = SubMethod.MapPosition();
                                shop.Clear();
                                int buyCheckCount = SubMethod.UpdateShop(shop);
                                buyCheck = new bool[buyCheckCount];
                                player.LiveDay++;
                            }
                            
                            break;
                        case 5:
                            //게임 끝내기
                            Console.SetCursorPosition(0, 26);
                            sceneEnd = true;
                            break;
                    }
                }
            }
        }
    }
}

