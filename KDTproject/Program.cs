

using static System.Net.Mime.MediaTypeNames;

namespace KDTproject
{

    internal class Program
    {
        // 화면 벽그리기 순서대로 넓이, 높이, 시작 X좌표, 시작 Y좌표
        static int[] windowWidth = { 90, 25, 1, 0 };
        static int[] info = { 50, 12, 20, 6 };
        static ConsoleKeyInfo keyInfo;



        // 메인 함수
        static void Main(string[] args)
        {    
            Console.Title = "KDT PROJECT HOME";
            bool gameStart = StartScene();
            if (!gameStart)
            {
                return;
            }
        }


        // 첫 스타트 씬
        static bool StartScene()
        {
            int selectMenu = 0;
            bool firstLord = true;
            bool sceneEnd = false;
            string[] startLogo = Date.startLogo;
            string[] startMenu = Date.startMenu;

            while(!sceneEnd) 
            {
                Console.Clear();

                // 화면 벽그리기 순서대로 넓이, 높이, 시작 X좌표, 시작 Y좌표
                Draw.DrawBox(windowWidth);
             
                for(int i = 0; i< startLogo.Length; i++)
                {
                    Draw.WriteText(startLogo[i], 20, 2+i);
                    if (firstLord)
                    {
                        Thread.Sleep(200);
                    }
                }
                firstLord = false;
                Draw.WriteText("by 태하 With Sparta", 37, 14);

                int[] selectBtn = {30 ,4, 31, 18 };
                Draw.DrawBox(selectBtn);
                for(int i = 0; i<startMenu.Length; i++)
                {
                    if(i == selectMenu)
                    {
                        Draw.WriteText(">", 40, 19 + i);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    Draw.WriteText(startMenu[i], 42, 19+i);
                    Console.ResetColor();
                }
                

                (bool sceneCheck, int selectMenuCheck) = keyEvent.keyCheck(keyInfo,selectMenu,startMenu.Length);
                sceneEnd = sceneCheck;
                selectMenu = selectMenuCheck;

                if(selectMenu == 0 && sceneEnd)
                {    
                    Draw.DrawBox(info);
                    Draw.ClearBox(info);
                    Draw.WriteText("구현중...", 40, 12);
                    sceneEnd = false;
                    
                    Thread.Sleep(1000);
                    
                }
                if(selectMenu == 1 && sceneEnd)
                {
                    Draw.DrawBox(info);
                    Draw.ClearBox(info);
                    Draw.WriteText("구현중...", 40, 12);
                    sceneEnd = false;

                    Thread.Sleep(1000);
                }
                if (selectMenu == 2 && sceneEnd)
                {
                    Console.SetCursorPosition(0,26);
                    sceneEnd = true;
                }
            }
            if(selectMenu == 2)
            {
                return false;
            }
            return true;
        }

        // 
        static void MainScene()
        {
            //이어하기시 읽은 데이터를 바탕으로 캐릭터를 생성
            //새로하기시 데이터 새로 생성
            //if()
        }
        

    }
}