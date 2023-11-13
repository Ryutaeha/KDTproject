using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDTproject
{
    internal static class KeyEvent
    {
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
                if (selectMenu < menuLength - 1)
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
}
