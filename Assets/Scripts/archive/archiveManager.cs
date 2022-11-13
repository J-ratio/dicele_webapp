using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Runtime.InteropServices;

public class archiveManager : MonoBehaviour
{
    [SerializeField]
    GameObject archive_lvl;
    [SerializeField]
    public GameObject GameManager;

    public int[,] solutionArr = new int[5,5];
    public int[,] spawn_Arr = new int[5,5];
    private int[] column_sum = new int[5];
    private int[] row_sum = new int[5];
    private bool RowLogic = true;
    public string ShareMsg = "";
    public string[,] s = new string[5,5];
    public int LastArchiveOpened;
    public int LastArchiveSwapCount;
    public List<int> ArchiveList = new List<int>();
    public List<int> ArchiveSwapList = new List<int>();

    public Sprite[] dice_images;
    [SerializeField]
    Sprite star_image;
    public static List<GameObject> ArchiveSlotList = new List<GameObject>();
    List <GameObject> ArchiveBlock = new List<GameObject>();
    [SerializeField]
    TextMeshProUGUI[] rowSumText;
    [SerializeField]
    TextMeshProUGUI[] ColumnSumText;
    public TextMeshProUGUI swapCount;
    [SerializeField]
    TextMeshProUGUI DayHeader;
    public GameObject WinningScreen;
    public GameObject GameoverScreen;
    public GameObject ResultScreen;
    [SerializeField]
    Button Share;
    [SerializeField]
    GameObject Shared;

    [SerializeField]
    Button Archive_close;
    [SerializeField]
    GameObject Archive_back;
    [SerializeField]
    GameObject archive_screen;
    [SerializeField]
    GameObject archive_canvas;

    [SerializeField]
    Button gameoverclose;
    [SerializeField]
    Button winClose;

    [SerializeField]
    TextMeshProUGUI ArchivePlayed;
    [SerializeField]
    TextMeshProUGUI ArchiveCompleted;
    [SerializeField]
    TextMeshProUGUI Archive5starsCount;

    public TextMeshProUGUI Timer;
    public TextMeshProUGUI Moves;
    




    int Day;
    int m2, d2;

    [DllImport("__Internal")]
    public static extern void ArchiveShareTrigger(string msg,int ArchiveNumber, int swap_count);

    public void UpdateArchiveStats(string temp1,string temp2){
        string[] arList = temp1.Split(",");
        string[] arSwapList = temp2.Split(",");
        ArchiveList.Clear();
        ArchiveSwapList.Clear();
        for(var i=0;i<arList.Length;i++){
            Debug.Log(arList[i]  + "    " +arSwapList[i]);
            ArchiveList.Add(int.Parse(arList[i]));
            ArchiveSwapList.Add(int.Parse(arSwapList[i]));
        }
        foreach(GameObject block in ArchiveBlock){
            if(ArchiveList[block.GetComponent<archive_block>().archive_number]==1){
                block.GetComponent<archive_block>().HeartBreak.SetActive(true);
                block.GetComponent<archive_block>().Unattempted.SetActive(false);
            }
            else if(ArchiveList[block.GetComponent<archive_block>().archive_number]==2){
                for(var j = 0;j<5;j++){
                    block.GetComponent<archive_block>().Stars[j].SetActive(true);
                    if(j<(ArchiveSwapList[block.GetComponent<archive_block>().archive_number]>5?5:ArchiveSwapList[block.GetComponent<archive_block>().archive_number])){
                    block.GetComponent<archive_block>().Stars[j].GetComponent<Image>().sprite = star_image;
                    }
                }
                block.GetComponent<archive_block>().Unattempted.SetActive(false);
            }
        }

        int Ones = ArchiveList.ToArray().Count(x => x==1);
        int Twos = ArchiveList.ToArray().Count(x => x==2);
        int Fives = ArchiveSwapList.ToArray().Count(x => x == 0) + ArchiveSwapList.ToArray().Count(x => x == 1) + ArchiveSwapList.ToArray().Count(x => x == 2) + ArchiveSwapList.ToArray().Count(x => x == 3) + ArchiveSwapList.ToArray().Count(x => x == 4);

        ArchivePlayed.text = (Ones + Twos).ToString() + "/" + (Day-1).ToString() + " (" + ((Ones + Twos)*100/(Day-1)).ToString() + "%)";
        ArchiveCompleted.text = (Twos).ToString() + "/" + (Day-1).ToString() + " (" + (Twos*100/(Day-1)).ToString() + "%)";
        Archive5starsCount.text = (Day - 1 - Fives).ToString() + "/" + (Day-1).ToString() + " (" + ((Day - 1 - Fives)*100/(Day -1)).ToString() + "%)";
    }


    void Start()
    {   
        for(var l =0;l<GameManager.GetComponent<RandomSpawnGenerator>().ArchiveList.Count;l++){
            ArchiveList.Add(GameManager.GetComponent<RandomSpawnGenerator>().ArchiveList[l]);
            ArchiveSwapList.Add(GameManager.GetComponent<RandomSpawnGenerator>().ArchiveSwapList[l]);
        }

        Day = GameManager.GetComponent<RandomSpawnGenerator>().Day;

        int Ones = ArchiveList.ToArray().Count(x => x==1);
        int Twos = ArchiveList.ToArray().Count(x => x==2);
        int Fives = ArchiveSwapList.ToArray().Count(x => x == 0) + ArchiveSwapList.ToArray().Count(x => x == 1) + ArchiveSwapList.ToArray().Count(x => x == 2) + ArchiveSwapList.ToArray().Count(x => x == 3) + ArchiveSwapList.ToArray().Count(x => x == 4);

        ArchivePlayed.text = (Ones + Twos).ToString() + "/" + (Day-1).ToString() + " (" + ((Ones + Twos)/(Day -1)).ToString() + "%)";
        ArchiveCompleted.text = (Twos).ToString() + "/" + (Day-1).ToString() + " (" + (Twos/(Day -1)).ToString() + "%)";
        Archive5starsCount.text = (Day - 1 - Fives).ToString() + "/" + (Day-1).ToString() + " (" + ((Day - 1 - Fives)/(Day -1)).ToString() + "%)";


        for(var i = 0; i<(Day-1) ; i++){
        var archive_lvl1 = Instantiate(archive_lvl, new Vector3(-2.5f,-0.7f - i*0.95f, transform.position.z) , Quaternion.identity, transform);
        archive_lvl1.name = "archivelvl_" + (Day-1-i).ToString();
        archive_lvl1.GetComponent<archive_block>().archive_number = Day-2-i;
        archive_lvl1.GetComponent<archive_block>().DayText.text = "#" + (Day-1-i).ToString();
        archive_lvl1.GetComponent<archive_block>().DateText.text = addDays(10,10,2022,Day - 2 -i);
        if(ArchiveList[Day-2-i]==1){
            archive_lvl1.GetComponent<archive_block>().HeartBreak.SetActive(true);
            archive_lvl1.GetComponent<archive_block>().Unattempted.SetActive(false);
        }
        else if(ArchiveList[Day-2-i]==2){
            for(var j = 0;j<5;j++){
                archive_lvl1.GetComponent<archive_block>().Stars[j].SetActive(true);
                if(j<(ArchiveSwapList[Day-2-i]>5?5:ArchiveSwapList[Day-2-i])){
                archive_lvl1.GetComponent<archive_block>().Stars[j].GetComponent<Image>().sprite = star_image;
                }
        }
            archive_lvl1.GetComponent<archive_block>().Unattempted.SetActive(false);

        }
        archive_lvl1.GetComponent<archive_block>().archiveManager = this.gameObject;
        ArchiveBlock.Add(archive_lvl1);
        }

        Archive_close.onClick.AddListener(Close_archive);
        Archive_back.GetComponent<Button>().onClick.AddListener(BackToArchive);
        Share.onClick.AddListener(ShareArchiveMsg);
        gameoverclose.onClick.AddListener(closelosescreen);
        winClose.onClick.AddListener(closewinscreen);
    }

    void closelosescreen(){
        GameoverScreen.SetActive(false);
        ResultScreen.SetActive(false);
    }

    void closewinscreen(){
        WinningScreen.SetActive(false);
        ResultScreen.SetActive(false);
    }

    void ShareArchiveMsg(){
        ArchiveShareTrigger(ShareMsg,LastArchiveOpened,LastArchiveSwapCount);
        Shared.SetActive(true);
    }

    void Close_archive(){
        archive_screen.SetActive(false);
    }

    void BackToArchive(){
        archive_screen.SetActive(true);
        archive_canvas.SetActive(false);
        Archive_back.SetActive(false);
    }

bool isLeap(int y){
        if (y % 100 != 0 && y % 4 == 0 || y % 400 == 0)
            return true;
    
        return false;
    }

    int offsetDays(int d, int m, int y)
    {
        int offset = d;
    
        if(m - 1 == 11)
            offset += 335;
        if(m - 1 == 10)
            offset += 304;
        if(m - 1 == 9)
            offset += 273;
        if(m - 1 == 8)
            offset += 243;
        if(m - 1 == 7)
            offset += 212;
        if(m - 1 == 6)
            offset += 181;
        if(m - 1 == 5)
            offset += 151;
        if(m - 1 == 4)
            offset += 120;
        if(m - 1 == 3)
            offset += 90;
        if(m - 1 == 2)
            offset += 59;
        if(m - 1 == 1)
            offset += 31;
    
        if (isLeap(y) && m > 2)
            offset += 1;
    
        return offset;
    }

    void revoffsetDays(int offset, int y){
        int []month = { 0, 31, 28, 31, 30, 31, 30,31, 31, 30, 31, 30, 31 };
    
        if (isLeap(y))
            month[2] = 29;
        int i;
        for (i = 1; i <= 12; i++)
        {
            if (offset <= month[i])
                break;
            offset = offset - month[i];
        }
    
        d2 = offset;
        m2 = i;
    }


    string addDays(int d1, int m1, int y1, int x)
    {
        int offset1 = offsetDays(d1, m1, y1);
        int remDays = isLeap(y1) ? (366 - offset1) : (365 - offset1);
    
        // y2 is going to store result year and
        // offset2 is going to store offset days
        // in result year.
        int y2, offset2 = 0;
        if (x <= remDays)
        {
            y2 = y1;
            offset2 = offset1+x;
        }
    
        else
        {
            // x may store thousands of days.
            // We find correct year and offset
            // in the year.
            x -= remDays;
            y2 = y1 + 1;
            int y2days = isLeap(y2) ? 366 : 365;
            while (x >= y2days)
            {
                x -= y2days;
                y2++;
                y2days = isLeap(y2)?366:365;
            }
            offset2 = x;
        }
        revoffsetDays(offset2, y2);
        return d2 + "/" + m2 + "/" + y2;
}




    public void StartArchiveGame(int archive_number){
        LastArchiveOpened = archive_number;
        DayHeader.text = "#" + (archive_number+1);
        for(int i = 0; i<5;i++){
            for(int j = 0; j<5; j++){
                solutionArr[i,j] = GetComponent<str>().sol_arr[archive_number,i,j];
                spawn_Arr[i,j] = GetComponent<str>().spawn_arr[archive_number,i,j];    
            }
        archive_screen.SetActive(false);
        archive_canvas.SetActive(true);
        Archive_back.SetActive(true);
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

    public static void update_color(){

        foreach(GameObject slot in archiveManager.ArchiveSlotList)
        {
            slot.GetComponent<archive_dropper>().Check_color();
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
    }


}
