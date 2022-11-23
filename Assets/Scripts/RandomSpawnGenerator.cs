using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;


public class RandomSpawnGenerator : MonoBehaviour
{
    public int[,] solutionArr = new int[5,5];
    public int[,] spawn_Arr = new int[5,5];

    private int[] column_sum = new int[5];
    private int[] row_sum = new int[5];
    private bool RowLogic = true;
    public int Day;
    public string ShareMsg = "";
    public string[,] s = new string[5,5];
    public static bool isSolved = false;
    int GamesPlayed;
    int StarsCollected;
    int CurrentStreak;
    int HighestStreak;
    int[] starFreq = new int[6];
    public List<int> ArchiveList;
    public List<int> ArchiveSwapList;

    public Sprite[] dice_images;
    public Sprite[] GreenDiceImages;
    public static List<GameObject> slotList = new List<GameObject>();
    [SerializeField]
    Button Share;
    [SerializeField]
    Button Share_stats;

    [SerializeField]
    TextMeshProUGUI[] rowSumText;
    [SerializeField]
    TextMeshProUGUI[] ColumnSumText;
    public TextMeshProUGUI swapCount;
    [SerializeField]
    TextMeshProUGUI DayHeader;
    [SerializeField]
    TextMeshProUGUI[] gamesPlayed;
    [SerializeField]
    TextMeshProUGUI[] starsCollected;
    [SerializeField]
    TextMeshProUGUI[] currentStreak;
    [SerializeField]
    TextMeshProUGUI[] highestStreak;
    [SerializeField]
    TextMeshProUGUI[] StarStats;
    [SerializeField]
    Slider[] StarFreq;
    [SerializeField]
    GameObject msg;
    [SerializeField]
    GameObject msg_stats;
    public GameObject WinningScreen;
    public GameObject GameoverScreen;
    public GameObject ResultScreen;
    [SerializeField]
    Button CloseWin;
    [SerializeField]
    Button CloseLose;
    [SerializeField]
    public GameObject HowToPlay;
    [SerializeField]
    public GameObject Stats;
    [SerializeField]
    private GameObject Stats_NextDay;
    [SerializeField]
    public GameObject Menu;
    [SerializeField]
    private GameObject Archive;
    [SerializeField]
    private Button Help;
    [SerializeField]
    private Button Stat;
    [SerializeField]
    private Button MenuBtn;
    [SerializeField]
    private Button ArchiveBtn;
    [SerializeField]
    private Button HelpClose;
    [SerializeField]
    private Button StatClose;
    [SerializeField]
    private Button MenuClose;
    [SerializeField]
    private Button MenuStat;
    [SerializeField]
    private Button MenuHelp;
    [SerializeField]
    public GameObject ArchiveManager;
    public TextMeshProUGUI Timer;
    public TextMeshProUGUI Moves;
    [SerializeField]
    GameObject RedDot_Menu;
    [SerializeField]
    GameObject RedDot_Archive;
    public GameObject ResultsMsg; 
    [SerializeField]
    ParticleSystem ConfettiVFX;

    




    [DllImport("__Internal")]
    public static extern void shareTrigger(string msg);
    [DllImport("__Internal")]
    public static extern void storeShare(string msg);
    
    
    public void GetDay(string day){
        Day = int.Parse(day);
        DayHeader.text = "#" + day;
    }

    public void UpdateStats(string temp){
        string[] Temp = temp.Split(",");
        gamesPlayed[0].text = Temp[0]; gamesPlayed[1].text = Temp[0];
        starsCollected[0].text = Temp[1]; starsCollected[1].text = Temp[1];
        currentStreak[0].text = Temp[2]; currentStreak[1].text = Temp[2];
        highestStreak[0].text = Temp[3]; highestStreak[1].text = Temp[3];
        GamesPlayed = int.Parse(Temp[0]);
        StarsCollected = int.Parse(Temp[1]);
        CurrentStreak = int.Parse(Temp[2]);
        HighestStreak = int.Parse(Temp[3]);
        for(var i =0; i<6;i++){
            starFreq[i] = int.Parse(Temp[i+4]);
            StarStats[i].text = Temp[i+4];
            StarFreq[i].value = (starFreq[i]*1.0f/GamesPlayed);
        }
        

    }

    public void StoreArchiveStats(string temp){
        string[] Temp = temp.Split("|");
        string[] arList = Temp[0].Split(",");
        string[] arSwapList = Temp[1].Split(",");

        for(var i=0;i<arList.Length;i++){
            ArchiveList.Add(int.Parse(arList[i]));
            ArchiveSwapList.Add(int.Parse(arSwapList[i]));
        }
        if(ArchiveList.SkipLast(1).Contains(0)){
            RedDot_Archive.SetActive(true);
            RedDot_Menu.SetActive(true);
        }
    }

    public void UpdateArchiveStats(string temp){
        string[] Temp = temp.Split("|");
        Debug.Log(Temp[0] + "    " +Temp[1]);
        ArchiveManager.GetComponent<archiveManager>().UpdateArchiveStats(Temp[0],Temp[1]);
    }

    public void ShowResultScreen(string temp1){
        string[] temp = (temp1).Split(",");
        isSolved = true;
        int Swaps = int.Parse(temp[1]);
        int Time = int.Parse(temp[2]);
        int[] Board_state = new int[25];
        for(var i=0;i<25;i++){
            Board_state[i] = int.Parse(temp[i+3]);
        }
        for(var j=0;j<5;j++){
            for(var k=0;k<5;k++){
                spawn_Arr[j,k] = Board_state[k + j*5];
            }
        }
        Dropper.swap_count = Swaps;
        Moves.text = (21-Swaps).ToString() + "/21";
        Dropper.Total_sec = Time;
        if(Time<60){
            Timer.text = Time.ToString() + "s";
        }
        else if(Time<3600){
            Timer.text = (Time/60).ToString() + "m " + (Time%60).ToString() + "s";
        }
        
        if(int.Parse(temp[0])==0){
            ShowLose();            
        }
        else{
            ShowWin();
        }
    }

    void ShowWin(){
        WinningScreen.SetActive(true);
        ResultScreen.SetActive(true);
    }

    void ShowLose(){
        GameoverScreen.SetActive(true);
        ResultScreen.SetActive(true);
    }

    void ShowHelp(){
        HowToPlay.SetActive(true);
    }


    void WinClose(){
        WinningScreen.SetActive(false);
        ResultScreen.SetActive(false);
    }

    void LoseClose(){
        GameoverScreen.SetActive(false);
        ResultScreen.SetActive(false);
    }

    void HelpScreen(){
        if(HowToPlay.activeSelf){
                HowToPlay.SetActive(false);
        }
        else{
                HowToPlay.SetActive(true);
                Stats.SetActive(false);
                Menu.SetActive(false);
        }
    }

    void MenuScreen(){
        if(Menu.activeSelf){
                Menu.SetActive(false);
        }
        else{
                Menu.SetActive(true);
                Stats.SetActive(false);
                HowToPlay.SetActive(false);
        }
    }

    void StatScreen(){
        if(Stats.activeSelf){
            Stats.SetActive(false);
        }
        else{
            HowToPlay.SetActive(false);
            Menu.SetActive(false);
            Stats.SetActive(true);
            if(isSolved){
                Stats_NextDay.SetActive(true);
            }
        }
    }

    void ShowArchive(){
        Archive.SetActive(true);
        Menu.SetActive(false);
        RedDot_Archive.SetActive(false);
        RedDot_Menu.SetActive(false);
    }

    void CloseHelp(){
        HowToPlay.SetActive(false);
    }

    void CloseStat(){
        Stats.SetActive(false);
    }

    void CloseMenu(){
        Menu.SetActive(false);
    }

    public void PlayWinAnim(){
        ConfettiVFX.Play();
    }


    void Start()
    {   
        Share.onClick.AddListener(Share_msg1);
        Share_stats.onClick.AddListener(Share_msg2);
        CloseWin.onClick.AddListener(WinClose);
        CloseLose.onClick.AddListener(LoseClose);
        Help.onClick.AddListener(HelpScreen);
        Stat.onClick.AddListener(StatScreen);
        HelpClose.onClick.AddListener(CloseHelp);
        StatClose.onClick.AddListener(CloseStat);
        MenuBtn.onClick.AddListener(MenuScreen);
        MenuClose.onClick.AddListener(CloseMenu);
        MenuStat.onClick.AddListener(StatScreen);
        MenuHelp.onClick.AddListener(HelpScreen);
        ArchiveBtn.onClick.AddListener(ShowArchive);

        for(int i = 0; i<5;i++){
            for(int j = 0; j<5; j++){
                solutionArr[i,j] = GetComponent<str>().sol_arr[Day-1,i,j];
                if(!isSolved){
                    spawn_Arr[i,j] = GetComponent<str>().spawn_arr[Day-1,i,j];
                }        
            }
        }


        for(var i=0; i<5; i++){
            if(i == 1 || i == 3){
                ColumnSumText[i].text = (solutionArr[i,0] + solutionArr[i,2] + solutionArr[i,4] + 3).ToString();
            }
            else{
                ColumnSumText[i].text = (solutionArr[i,0] + solutionArr[i,1] + solutionArr[i,2] + solutionArr[i,3] + solutionArr[i,4] + 5).ToString();
            }
            
        }

        for(var i=0; i<5; i++){
            if(i == 1 || i == 3){
                rowSumText[i].text = (solutionArr[0,4-i] + solutionArr[2,4-i] + solutionArr[4,4-i] + 3).ToString();
            }
            else{
                rowSumText[i].text = (solutionArr[0,4-i] + solutionArr[1,4-i] + solutionArr[2,4-i] + solutionArr[3,4-i] + solutionArr[4,4-i] + 5).ToString();
            }
            
        }
    }

    public void MakeLoseShareMsg(){
        for(var i =0; i<5;i++){
            for(var j =0; j<5;j++){
                if(solutionArr[i,j] == 6){
                s[i,j] = "🟩";
                }
                else{
                s[i,j] = "⬛";
                }
            }
        }

        string[] strings = {"","","","",""};
        s[1,1] = "⬜"; s[1,3] = "⬜"; s[3,1] = "⬜"; s[3,3] = "⬜"; 
        for(var k=0; k<5; k++){
            for(var l=0;l<5;l++){
                strings[k] = strings[k] + s[l,4-k];
            }
        }
        ShareMsg = strings[0] + "\n" + strings[1] + "\n" + strings[2] + "\n" + strings[3] + "\n" + strings[4];
        storeShare(ShareMsg);
    }

    void Share_msg1(){
        msg.SetActive(true);
        shareTrigger(ShareMsg);
    }

    void Share_msg2(){
        msg_stats.SetActive(true);
        shareTrigger(ShareMsg);
    }

    public List<int> Get_Yellow_numbers(int slot_pos0, int slot_pos1){
        List<int> column_arr = new List<int>();
        List<int> row_arr = new List<int>();

        for(var i = 0; i < 5; i++){
            if(i!=slot_pos1){
                if(solutionArr[slot_pos0,i]!=6){
                    column_arr.Add(solutionArr[slot_pos0,i]);
                }
            }
        }

        if(RowLogic){
            for(var i = 0; i < 5; i++){
            if(i!=slot_pos0){
                if(solutionArr[i,slot_pos1]!=6){
                    column_arr.Add(solutionArr[i,slot_pos1]);
                }
            }
        }
        column_arr.AddRange(row_arr);
        }

        List<int> column_arr_new = column_arr.Distinct().ToList();

        return column_arr_new;

    }

    public int Get_Green_number(int slot_pos0, int slot_pos1){
        return solutionArr[slot_pos0,slot_pos1];
    }

    public static void update_color()
    {
    foreach(GameObject slot in RandomSpawnGenerator.slotList)
    {
        slot.GetComponent<Dropper>().Check_color();
    }
    }

}
