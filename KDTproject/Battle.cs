using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDTproject
{
    internal class Battle
    {


        /// <summary>
        /// 전투 or 휴식
        /// </summary>
        /// <param name="keyInfo"></param>
        /// <returns></returns>
        internal bool BattleKindSelect(ConsoleKeyInfo keyInfo)
        {

            int selectMenu = 0;
            while (true)
            {
                Draw.ClearBox(Date.map);
                Draw.ClearBox(Date.selectCampMenu);
                Draw.SelectMenu(Date.BattleKindSelect, selectMenu, 70, 7, 3, true);

                (bool sceneCheck, int selectMenuCheck) = KeyEvent.keyCheck(keyInfo, selectMenu, Date.BattleKindSelect.Length);

                selectMenu = selectMenuCheck;
                if (sceneCheck)
                {
                    switch (selectMenu)
                    {
                        case 0:
                            if (BattleSceneSelect(keyInfo)) return true;
                            else break;

                        case 1:
                            if (Rest(keyInfo)) return true;
                            else break;
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
        internal bool BattleSceneSelect(ConsoleKeyInfo keyInfo)
        {
            string[] battleSceneSelect = Date.BattleSceneSelect;
            int selectMenu = 0;
            bool clear;
            while (true)
            {
                Draw.ClearBox(Date.map);
                Draw.ClearBox(Date.selectCampMenu);
                Draw.SelectMenu(battleSceneSelect, selectMenu, 70, 7, 3, true);

                (bool sceneCheck, int selectMenuCheck) = KeyEvent.keyCheck(keyInfo, selectMenu, battleSceneSelect.Length);

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


        private void BattleScene(ConsoleKeyInfo keyInfo, int battleScene)
        {

            Mosters.Moster moster = null;
            string[] mosterSelect;
            int randomMonster = RandomMonster();

            switch (battleScene)
            {
                case 0:
                    mosterSelect = Date.Evil_Rins[randomMonster];
                    moster = new Mosters.Moster(mosterSelect[0], mosterSelect[1], mosterSelect[2], mosterSelect[3], mosterSelect[4], mosterSelect[5], mosterSelect[6]);
                    break;
                case 1:
                    mosterSelect = Date.Undead[0];
                    moster = new Mosters.Moster(mosterSelect[0], mosterSelect[1], mosterSelect[2], mosterSelect[3], mosterSelect[4], mosterSelect[5], mosterSelect[6]);
                    break;
                case 2:
                    mosterSelect = Date.Devil[0];
                    moster = new Mosters.Moster(mosterSelect[0], mosterSelect[1], mosterSelect[2], mosterSelect[3], mosterSelect[4], mosterSelect[5], mosterSelect[6]);
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
                int victoryProbability = Draw.BattleInfo(moster, diceNum);

                Draw.ClearBox(Date.selectCampMenu);



                Draw.SelectMenu(battleSceneSelect, selectMenu, 70, 7, 3, endSelect);

                (bool sceneCheck, int selectMenuCheck) = KeyEvent.keyCheck(keyInfo, selectMenu, battleSceneSelect.Length);

                selectMenu = selectMenuCheck;
                if (sceneCheck)
                {
                    switch (selectMenu)
                    {
                        case 0:
                            Draw.BattleEndScene(victoryProbability, moster, keyInfo);
                            return;

                        case 1:
                            if (diceRollLimit > 0)
                            {
                                diceNum = RandomDice();

                                if (--diceRollLimit == 0) selectMenu = 0;
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



        private int RandomMonster()
        {
            Random random = new Random();
            int selectMoster = random.Next(0, 20);
            if (selectMoster < 12) return 0;
            else if (selectMoster < 18) return 1;
            else return 2;
        }

        private bool Rest(ConsoleKeyInfo keyInfo)
        {
            int xPositoin = 22;
            int yPosition = 10;
            int ypositionInfo = 13;
            int selectMenu = 0;
            int RestCondition = new Random().Next(0, 111);
            while (true)
            {
                Draw.ClearBox(Date.map);
                Draw.ClearBox(Date.selectCampMenu);
                Draw.WriteText("휴식비 100 Gold", 23, 13);
                Draw.SelectMenu(Date.Rest, selectMenu, 70, 7, 3, true);
                (bool sceneCheck, int selectMenuCheck) = KeyEvent.keyCheck(keyInfo, selectMenu, Date.campMenu.Length);
                selectMenu = selectMenuCheck;
                if (sceneCheck)
                {

                    if (GameScene.player.Gold - 100 < 0) return false;
                    GameScene.player.Gold -= 100;
                    Draw.SimplePlayerInfo(GameScene.player);
                    Draw.ClearBox(Date.map);
                    switch (selectMenu)
                    {
                        case 0:
                            if (RestCondition == 110)
                            {
                                Draw.WriteText("너무 개운하다!", xPositoin, yPosition);

                                Draw.WriteText($"최대 체력 증가! +{20}", xPositoin, ypositionInfo);
                                GameScene.player.MaxHealth += 20;
                                GameScene.player.Health += 30;
                                if (GameScene.player.Health > GameScene.player.MaxHealth) GameScene.player.Health = GameScene.player.MaxHealth;
                            }
                            else if (RestCondition > 10)
                            {
                                Draw.WriteText("정신이 맑아졌다!", xPositoin, yPosition);

                                Draw.WriteText($"최대 체력 증가! +{RestCondition / 10}", xPositoin, ypositionInfo);
                                GameScene.player.MaxHealth += RestCondition / 10;
                                GameScene.player.Health += 10 + (RestCondition / 10);
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
                                GameScene.player.Defense += RestCondition / 20;
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

                                Draw.WriteText($"체력 대폭 회복! +{GameScene.player.MaxHealth - GameScene.player.Health}", xPositoin, ypositionInfo);
                                GameScene.player.Health = GameScene.player.MaxHealth;
                            }
                            else if (RestCondition > 5)
                            {
                                Draw.WriteText("정신이 맑아졌다!", xPositoin, yPosition);

                                Draw.WriteText($"체력 회복! +{RestCondition / 10}", xPositoin, ypositionInfo);
                                GameScene.player.Health += (20 + (RestCondition / 5));
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
                        Draw.SimplePlayerInfo(GameScene.player);
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

        internal void Dead(ConsoleKeyInfo keyInfo)
        {
            Draw.ClearBox(Date.map);
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
        internal int RandomDice()
        {
            int diceRoll = new Random().Next(1, 21);
            return diceRoll;
        }
    }
}
