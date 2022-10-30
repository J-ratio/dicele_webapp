using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class archiveManager : MonoBehaviour
{
    [SerializeField]
    GameObject archive_lvl;
    [SerializeField]
    GameObject GameManager;

    public int[,] solutionArr = new int[5,5];
    public int[,] spawn_Arr = new int[5,5];
    private int[] column_sum = new int[5];
    private int[] row_sum = new int[5];
    private bool RowLogic = true;
    public string ShareMsg = "";
    public string[,] s = new string[5,5];


    public Sprite[] dice_images;
    public static List<GameObject> ArchiveSlotList = new List<GameObject>();
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
    Button Archive_close;
    [SerializeField]
    GameObject Archive_back;
    [SerializeField]
    GameObject archive_screen;
    [SerializeField]
    GameObject archive_canvas;



    int Day;
    int m2, d2;

    // Start is called before the first frame update
    void Start()
    {
        Day = GameManager.GetComponent<RandomSpawnGenerator>().Day;
        for(var i = 0; i<(Day-1) ; i++){
        var archive_lvl1 = Instantiate(archive_lvl, new Vector3(-2.5f,-0.7f - i*0.95f, transform.position.z) , Quaternion.identity, transform);
        archive_lvl1.name = "archivelvl_" + (Day-1-i).ToString();
        archive_lvl1.GetComponent<archive_block>().archive_number = Day-2-i;
        archive_lvl1.GetComponent<archive_block>().DayText.text = "#" + (Day-1-i).ToString();
        archive_lvl1.GetComponent<archive_block>().DateText.text = addDays(10,10,2022,Day - 2 -i);
        archive_lvl1.GetComponent<archive_block>().archiveManager = this.gameObject;
        }

        Archive_close.onClick.AddListener(Close_archive);
        Archive_back.GetComponent<Button>().onClick.AddListener(BackToArchive);
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

    public static void update_color()
    {
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