using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    internal class Date
    {
        public static string[] startLogo =
        {
            " @@@@            @          @    @         @@@@@@",
            "@@  @           @@          @    @          @   @",
            "@   @           @@          @@  @@          @   @",
            "@   @          @  @         @@  @@          @ @ ",
            "@              @  @         @@  @@          @ @ ",
            "@  @@@         @  @         @ @@ @          @@@  ",
            "@   @         @@@@@         @ @  @          @ @  ",
            "@   @         @@  @         @ @  @          @   @ ",
            "@   @         @   @         @    @          @   @ ",
            " @@@@        @@@ @@@        @@   @@        @@@@@@ "

        };

    public static string[] startMenu = { "새 게임", "이어하기", "종료" };
    }
    internal class keyEvent
    {
        /// <summary>
        /// 키 이벤트, 현재 선택되어 있는 메뉴, 메뉴 총 길이
        /// </summary>
        /// <param name="keyInfo"></param>
        /// <param name="selectMenu"></param>
        /// <param name="menuLength"></param>
        /// <returns></returns>
        public static (bool,int) keyCheck(ConsoleKeyInfo keyInfo, int selectMenu, int menuLength)
        {
        keyInfo = Console.ReadKey();

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
            if (selectMenu < menuLength - 1)
            {
               selectMenu++;
            }
        }
        if(keyInfo.Key == ConsoleKey.Enter)
        {
            return (true, selectMenu);
        }
        return (false,selectMenu);
        }
    }
    
    internal class Draw
    {
    public static void WriteText(string text, int xOffset, int yOffset)
    {
        Console.SetCursorPosition(xOffset, yOffset);
        Console.Write(text);
    }


    /// <summary>
    /// int 배열[4] 너비, 높이 , 시작 X축, 시작 Y축
    /// </summary>
    public static void DrawBox(int[] sceneArea)
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
    public static void ClearBox(int[] sceneArea)
    {
        for (int i = 1; i < sceneArea[1]; i++)
        {
            string clearText = "";
            for(int j = 1; j < sceneArea[0]; j++)
            {
                clearText += " ";
            }
            WriteText(clearText, sceneArea[2], sceneArea[3]+i);
        }
    }

        /*
    static void DrawBox(int[] widthArea, int[] heightArea)
    {
        // 상하 벽 그리기
        for (int i = 0; i < widthArea[0]-1; i++)
        {
                                      //시작 X, Y
            Console.SetCursorPosition(i + widthArea[1], widthArea[2]);
            Console.Write("-");
                                      //시작 X, Y시작점 + 높이
            Console.SetCursorPosition(i + widthArea[1], widthArea[2] + heightArea[0]);
            Console.Write("-");
        }

        // 좌우 벽 그리기
        for (int i = 0; i < heightArea[0] - 1; i++)
        {
            Console.SetCursorPosition(heightArea[1], i + heightArea[2]);
            Console.Write("|");
            Console.SetCursorPosition(heightArea[1] + widthArea[0], i + heightArea[2]);
            Console.Write("|");

        }
        Console.WriteLine();
    }
     */
}

