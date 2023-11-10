

using static Character;


namespace KDTproject
{

    internal class Program
    {
        //가지고 있는 무기
        public static List<CharacterSubMenu.Weapon> weapons = new List<CharacterSubMenu.Weapon>();
        //가지고 있는 방어구
        public static List<CharacterSubMenu.Armor> armors = new List<CharacterSubMenu.Armor>();
        internal static Player player = null;
        public static int[] Equipment = {-1,-1 };
        // 화면 벽그리기 순서대로 넓이, 높이, 시작 X좌표, 시작 Y좌표
        

        static ConsoleKeyInfo keyInfo;



        // 메인 함수
        static void Main(string[] args)
        {
            
            Console.Title = "KDT PROJECT HOME";
            //StartScene();
            /*
             */
            string[] args2 = { "테스트", "전사" };
            CampScene(0, args2);
        }


        // 첫 스타트 씬
        static void StartScene()
        {
            bool firstLord = false;
            int selectMenu = 0;
            bool sceneEnd = false;
            string[] startLogo = Date.startLogo;
            string[] startMenu = Date.startMenu;
            string[] playerCreate = null;
            while (!sceneEnd) 
            {
                Console.Clear();

                // 화면 벽그리기 순서대로 넓이, 높이, 시작 X좌표, 시작 Y좌표
                Draw.DrawBox(Date.windowWidth);


                // 로고 연출
                for (int i = 0; i< startLogo.Length; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Draw.WriteText(startLogo[i], 20, 2+i);
                    if (firstLord)
                    {
                        Thread.Sleep(200);
                    }
                    Console.ResetColor();
                }

                Console.ForegroundColor = ConsoleColor.Magenta;
                Draw.WriteText("by 태하 With Sparta", 37, 14);
                Console.ResetColor();

                Console.ResetColor();
                //첫 시작시을 판별하는 bool
                firstLord = false;

                // 플레이어 선택 창
                int[] selectBtn = {30 ,4, 31, 18 };
                Draw.DrawBox(selectBtn);

                // 선택 메뉴 출력
                keyEvent.SelectMenu(startMenu, selectMenu, 42, 19, 1,false); 

                // 플레이어의 선택을 Key이벤트로 보내주는 메서드
                (bool sceneCheck, int selectMenuCheck) = keyEvent.keyCheck(keyInfo,selectMenu,startMenu.Length);
                
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
        static void CampScene(int gmaeCheck, string[] playerCreate)
        {
            List<object> shop = new List<object>();
            bool[] buyCheck = null;
            

            bool newGame;
            int selectMenu = 0;
            int selectMap = 0;
            bool sceneEnd = false;
            string[] campMenu = Date.campMenu;
            int[,] mapPosition = SubMethod.MapPosition();
            switch (gmaeCheck)
            {
                case 0:
                    player = new Player(playerCreate[0], playerCreate[1]);
                    break;
                case 1:
                    break;
            }
            
            while (!sceneEnd)
            {
                Console.Clear();
                //박스 그리기
                Draw.DrawBox(Date.windowWidth);

                // 진행 박스 그리기

                Draw.DrawBox(Date.map);

                // 맵 선택창 띄우기
                keyEvent.selectMap(mapPosition, selectMap);

                //캐릭터 정보 출력
                SubMethod.SimplePlayerInfo(player);

                // 메뉴 선택박스 그리기

                Draw.DrawBox(Date.selectCampMenu);

                //메뉴창 띄우는 메서드
                keyEvent.SelectMenu(campMenu, selectMenu, 70, 7, 3,true);

                //메뉴 선택 메서드
                (bool sceneCheck, int selectMenuCheck) = keyEvent.keyCheck(keyInfo, selectMenu, campMenu.Length);
                
                selectMenu = selectMenuCheck;
                selectMap = selectMenuCheck;
                if (sceneCheck)
                {
                    switch (selectMenu)
                    {
                        case 0:
                            Draw.ClearBox(Date.map);
                            Draw.ClearBox(Date.selectCampMenu);
                            subScene.PlayerInfo(player, keyInfo);
                            break;
                        case 1:
                            Draw.ClearBox(Date.map);
                            Inventory.MyInventory(keyInfo, Date.map, Date.selectCampMenu, weapons, armors, "inventory");
                            break;
                        case 2:
                            subScene.Shop(keyInfo, Date.map, Date.selectCampMenu, shop, buyCheck);
                            break;
                        case 3:
                            Draw.ClearBox(Date.map);
                            Inventory.MyInventory(keyInfo, Date.map, Date.selectCampMenu, weapons, armors, "Enhance");
                            break;
                        case 4:
                            mapPosition = SubMethod.MapPosition();
                            shop.Clear();
                            int buyCheckCount = SubMethod.UpdateShop(shop);
                            buyCheck = new bool[buyCheckCount];
                            player.Gold += 100;
                            SubMethod.UpdateInfo("구현중...", 40, 12);
                            Thread.Sleep(1000);
                            player.LiveDay++;
                            break;
                        case 5:
                            Console.SetCursorPosition(0, 26);
                            sceneEnd = true;
                            break;
                    }
                }
            }
        }
    }

}