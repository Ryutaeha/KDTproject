using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                            player = new Player(playerCreate[0], playerCreate[1]);
                            CampScene();
                            break;
                        case 1:
                            SaveData saveData = JsonLoad();
                            if (saveData != null)
                            {
                                weapons = saveData.Weapons;
                                armors = saveData.Armors;
                                Equipment = saveData.Equipment;
                                player = new Player(saveData.Player);
                                CampScene();
                            }
                            else
                            {
                                SubMethod.UpdateInfo("정보 없음", 40, 12);
                                Thread.Sleep(1000);
                            }
                            
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
        internal static void CampScene()
        {

            List<object> shop = new List<object>();
            bool[] buyCheck = null;


            bool newGame;
            int selectMenu = 0;
            bool sceneEnd = false;
            int[,] mapPosition = SubMethod.MapPosition();
            

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
                            JsonSave();
                            
                            //게임 끝내기
                            Console.SetCursorPosition(0, 26);
                            sceneEnd = true;
                            break;
                    }
                }
            }
        }

        private static void JsonSave()
        {
            SaveData saveData = new SaveData
            {
                Weapons = weapons,
                Armors = armors,
                Equipment = Equipment,
                Player = player
            };
            string saveDatas = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            // 키 생성 어따보관하지? DB 연동하기엔 C#은 어캐하는지 몰라유
            byte[] key = Encoding.UTF8.GetBytes("0123456789ABCDEF");

            // 초기화 벡터 생성 (암호화된 데이터를 시작하는 지점을 지정)
            byte[] iv = Encoding.UTF8.GetBytes("ABCDEFGHABCDEFGH");

            // 문자열을 바이트 배열로 변환
            byte[] originalBytes = Encoding.UTF8.GetBytes(saveDatas);

            // 암호화
            byte[] encryptedBytes = Encrypt(originalBytes, key, iv);

            string directoryPath = @"C:\saveTest";

            // 폴더가 없으면 생성
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // 파일에 쓰기
            File.WriteAllBytes(@"C:\saveTest\path2.json", encryptedBytes);
        }

        private static byte[] Encrypt(byte[] originalBytes, byte[] key, byte[] iv)
        {
            
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                //대칭 키 암호화 생성
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                //대칭 암호화키를 이용하면 메모리 암호화
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(originalBytes, 0, originalBytes.Length);
                        csEncrypt.FlushFinalBlock();
                    }
                    return msEncrypt.ToArray();
                }
            }
        }

        private static SaveData JsonLoad()
        {
            string filePath = @"C:\saveTest\path2.json";
            SaveData saveData = null;
            // 파일이 존재하는지 확인
            if (File.Exists(filePath))
            {
                byte[] key = Encoding.UTF8.GetBytes("0123456789ABCDEF");

                // 초기화 벡터 생성 (암호화 시 사용된 초기화 벡터와 동일해야 함)
                byte[] iv = Encoding.UTF8.GetBytes("ABCDEFGHABCDEFGH");

                // 파일 내용을 바이트 배열로 읽기
                byte[] encryptedBytes = File.ReadAllBytes(filePath);

                // 복호화
                byte[] decryptedBytes = Decrypt(encryptedBytes, key, iv);

                // 복호화된 데이터를 문자열로 변환
                string decryptedData = Encoding.UTF8.GetString(decryptedBytes);

                saveData = JsonConvert.DeserializeObject<SaveData>(decryptedData);


            }
            return saveData;
            
        }

        private static byte[] Decrypt(byte[] encryptedBytes, byte[] key, byte[] iv)
        {
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                //대칭 키 암호화 생성
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                //대칭 암호화키를 참조하여 암호화된 메모리 복호화
                using (MemoryStream msDecrypt = new MemoryStream())
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                    {
                        csDecrypt.Write(encryptedBytes, 0, encryptedBytes.Length);
                        csDecrypt.FlushFinalBlock();
                    }
                    return msDecrypt.ToArray();
                }
            }
        }
    }
}

