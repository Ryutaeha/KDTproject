using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class Date
{
    public static int[] windowWidth = { 90, 25, 1, 0 };
    public static int[] info = { 50, 12, 20, 6 };
    public static int[] map = { 60, 19, 2, 5 };
    public static int[] selectCampMenu = { 26, 19, 63, 5 };
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
    public static string[] jobClass = { "전사", "사냥꾼" };
    public static string[] campMenu = { "상태보기", "인벤토리", "상점", "강화", "탐험", "끝내기" };
    public static string[] campMap = { "", "", "★", "♨", "◈" };
    public static string[] BattleKindSelect = { "전투", "휴식", "돌아가기" };
    public static string[] BattleSceneSelect = { "평원", "폐허", "던전", "돌아가기" };
    public static string[] BattleScene = { "전투", "주사위 굴리기", "도망가기" };
    public static string[] inventory = { "무기", "방어구", "돌아가기" };
    public static string[] shop = { "구매", "판매", "돌아가기" };
    public static string[] Rest = { "황금 햇빛 여관", "검은 무쇠 여관", "늙은 들개 여관","깊은 달빛 여관", "돌아가기"};
    
    /// <summary>
    /// 이름 공격력 방어력 체력 드랍골드 설명
    /// </summary>
    public static Dictionary<int, string[]> Evil_Rins = new Dictionary<int, string[]>
    {
        { 0 , new string[] { "고블린", "10", "10","30","50", "귀여운(?) 고블린이다." ,"1"} },
        { 1 , new string[] { "놀", "15", "15", "40", "70", "댕댕이가 두발로 걸어다닌다." ,"2"} },
        { 2 , new string[] {"오우거", "20", "20", "80", "120", "오우거! 가고 싶은데로 간다.", "5" } },
    };

    public static Dictionary<int, string[]> Undead = new Dictionary<int, string[]>
    {
        { 0 , new string[] {"스켈레톤", "40", "10", "60", "150", "포션(사골) 재료인거 같다.", "4" } },
        { 1 , new string[] {"좀비", "40", "30", "70", "180", "조금은 느린거 같다.", "5" } },
        { 2 , new string[] {"고스트", "65", "20", "100", "220", "가끔보면 귀엽게 생겼다.", "7" } },
    };

    public static Dictionary<int, string[]> Devil = new Dictionary<int, string[]>
    {
        { 0 , new string[] {"임프", "100", "30", "120", "240", "소매치기 조심하세욧!", "5" } },
        { 1 , new string[] {"가고일", "75", "150", "150", "300", "그 동상에서 눈을 때면 안된다.", "6" } },
        { 2 , new string[] {"서큐버스", "200", "100", "250", "1000" , "서큐버스가 좀 이상하게 생겼다.", "10" } },
    };

    public static Dictionary<int, string[]> Run = new Dictionary<int, string[]>
    {

    };

    public static string[] Merchant =
    {
        "                       ',,.:kxO",
        "                    .,:XWWMMMMM",
        "                ,.:xdxOXWWMMMMM",
        "             .:k00OxdxOXWWMMMMM",
        "            .;dxddxOXWWMMMMMMMM",
        "           '';ooodOXWWWMMMMMMMM",
        "          .;loolcclllkNWMMMMMMM",
        "         .;cll:'.    .:kKXWMMMM",
        "        .,cl:'.        .':kXWMM",
        "        .,ll:.  .■.       'coO",
        "       .,okl.            .'..l0",
        "       ..c0k;.               ,x",
        "      .':okK0o,.    '▼'     ..",
        "    ',cx00OkOK0o,.        ..:lo",
        "   ;;lk0KKKOddkK0d,......';ccc:",
        "  ;;;;cdxkxdoolldo:,;:::::clc;:",
        " ;;;;;;;:ccccc:cl;;:::cclokOxl:",
        ";;;;c;;::;;,;:oxkkOkddkkkOKXOdo"
    };
    public static string[] MerchantTxt =
    {
        "첫날은 엉업 준비기간입니다!",
        "어서와요~! 이것좀 보고가세요",
        "☆★스파르타 코딩 클☆럽★☆",
        "가입시$$전원 강의☜☜100%증정",
        "취업까지무료$$$국가안전운영",
        "전국 최저가 매입 보상",
        "사실 매입 상점 여기 하나예요"

    };
    /// <summary>
    /// 무기 키값, 이름, 랭크, 능력치, 아이템 설명, 가격
    /// </summary>
    public static Dictionary<int, string[]> Weapon = new Dictionary<int, string[]>
    {
        { 0 ,new String[]{"검","0", "5" , "흔해 빠진 검이다" ,"50"} },
        { 1 ,new String[]{"식칼","1", "10" , "흔해 빠진 식칼이다" ,"100"} },
        { 2 ,new String[]{"장미칼","1", "15" , "아름다운 장미칼이다" ,"200"} },
        { 3 ,new String[]{"좀 긴칼","2", "25" , "이건 강해보일지도..." ,"1000"} },
        { 4 ,new String[]{"만능칼","3", "50" , "이세카이 용사의 검이다" , "5000" } }
    };
    /// <summary>
    /// 방어구 키값, 이름, 랭크, 능력치, 아이템 설명, 가격
    /// </summary>
    public static Dictionary<int, string[]> Armor = new Dictionary<int, string[]>
    {
        { 0 ,new String[]{"천때기","0", "5" , "그냥 옷이다" ,"50"} },
        { 1 ,new String[]{"가죽","1", "10" , "조금 힙할지도?" ,"100"} },
        { 2 ,new String[]{"플라스틱","1", "15" , "금이 좀 간거 같다." ,"200"} },
        { 3 ,new String[]{"강철","2", "25" , "무겁다... 많이" ,"1000"} },
        { 4 ,new String[]{"훈도시","3", "50" , "방어력과 노출은 반비례!", "5000" } }
    };
    /// <summary>
    /// 포션 키값, 증가량, 설명, 가격
    /// </summary>
    public static Dictionary<int, string[]> Potion = new Dictionary<int, string[]>
    {
        { 0 ,new String[]{"체력 포션", "20" , "평범한 체력포션" ,"50"} },
        { 1 ,new String[]{"방어력 포션", "10" , "단단해지기!" ,"75"} },
        { 2 ,new String[]{"공격력 포션", "5" , "나 좀 쌜수도?" ,"75"} },
        { 3 ,new String[]{"방어력 포션", "25" , "단★단☆묵★직" ,"750"} },
        { 4 ,new String[]{"공격력 포션", "15" , "흑염룡 사골 국물", "1000" } }
    };

}

internal class AnimationSkip
{
    public static bool firstLord;
}

internal class SaveData
{
    public List<CharacterSubMenu.Weapon> Weapons { get; set; }
    public List<CharacterSubMenu.Armor> Armors { get; set; }
    public int[] Equipment { get; set; }
    public Character.Player Player { get; set; }
}