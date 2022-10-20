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
<<<<<<< Updated upstream
    public int[,] spawn_Arr = new int[5,5];

    private int[] column_sum = new int[5];
    private int[] row_sum = new int[5];
    private bool RowLogic = true;
    public int Day;
    public string ShareMsg = "";
    public string[,] s = new string[5,5];
    public static bool isSolved = false;
    public int GamesPlayed;
    public int StarsCollected;
    public int CurrentStreak;
    public int HighestStreak;
    public int[] starFreq;

    public Sprite[] dice_images;
    public static List<GameObject> slotList = new List<GameObject>();
    public Button Help;
    [SerializeField]
    private Button Share;
=======
    public int[] freq_Arr = new int[6];
    public bool RowLogic = false;
    public int[,] spawn_Arr = new int[5,5];
    private int[] column_sum = new int[5];
    private int[] row_sum = new int[5];
    [SerializeField]
    private int green_no = 0;

    public Sprite[] dice_images;
    public static List<GameObject> slotList = new List<GameObject>();
    //public Button Play_btn;
    public Button Help;
    [SerializeField]
    private Button Share;
    //public Button Restart_btn;
>>>>>>> Stashed changes
    public Toggle row_toggle;
    public Toggle rowSum_toggle;
    [SerializeField]
    private GameObject RowSum;
    [SerializeField]
    private GameObject HowToPlay;

    [SerializeField]
    TextMeshProUGUI[] rowSumText;
    [SerializeField]
    TextMeshProUGUI[] ColumnSumText;
    [SerializeField]
    TMP_InputField greenMinBound;
    [SerializeField]
    TextMeshProUGUI Sol;
    [SerializeField]
    public TextMeshProUGUI swapCount;
    [SerializeField]
    GameObject msg;

    [DllImport("__Internal")]
<<<<<<< Updated upstream
    public static extern void shareTrigger(string msg);
    
    
    public void GetDay(string day){
        Day = int.Parse(day);
    }

    public void UpdateStats(string temp){
        string[] Temp = temp.Split(",");
        GamesPlayed = int.Parse(Temp[0]);
        StarsCollected = int.Parse(Temp[1]);
        CurrentStreak = int.Parse(Temp[2]);
        HighestStreak = int.Parse(Temp[3]);
        for(var i =0; i<5;i++){
            starFreq[i] = int.Parse(Temp[i+4]);
        }
    }

    public void ShowResultScreen(string temp1){
        string[] temp = (temp1).Split(",");
        isSolved = true;
        int Swaps = int.Parse(temp[1]);
        int[] Board_state = new int[25];
        for(var i=0;i<25;i++){
            Board_state[i] = int.Parse(temp[i+2]);
        }
        for(var j=0;j<5;j++){
            for(var k=0;k<5;k++){
                spawn_Arr[j,k] = Board_state[k + j*5];
            }
        }
        Dropper.swap_count = Swaps;
        if(int.Parse(temp[0])==0){
            ShowLose();            
        }
        else{
            ShowWin();
        }
    }

    void ShowWin(){
        
    }

    void ShowLose(){

    }
=======
    public static extern void shareTrigger();
    
>>>>>>> Stashed changes


    void Start()
    {
<<<<<<< Updated upstream
        Help.onClick.AddListener(HelpScreen);
        Share.onClick.AddListener(Share_msg);

        for(int i = 0; i<5;i++){
            for(int j = 0; j<5; j++){
                solutionArr[i,j] = GetComponent<str>().sol_arr[Day,i,j];
                if(!isSolved){
                    spawn_Arr[i,j] = GetComponent<str>().spawn_arr[Day,i,j];
                }        
            }
        }

=======
        //Play_btn.onClick.AddListener(Play);
        //Restart_btn.onClick.AddListener(Restart);

        Help.onClick.AddListener(HelpScreen);
        Share.onClick.AddListener(Share_msg);


        for (var i=0; i<5; i++){
            for(var j=0; j<5; j++){
                if((i==1 && j ==1) || (i==3 && j==1) || (i==1 && j==3) || (i==3 && j==3)){
                    solutionArr[i,j] = 6;
                }
                else{
                    solutionArr[i,j] = Random.Range(0,6);
                    freq_Arr[solutionArr[i,j]] ++;
                }
            }
        }

        //update_sol();
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
    }

    public void MakeLoseShareMsg(){
        for(var i =0; i<5;i++){
            for(var j =0; j<5;j++){
                if(solutionArr[i,j] == 6){
                s[i,j] = "🟩";
                }
                else{
                s[i,j] = "⬛";
=======

        for (var i=0; i<5; i++){
            for(var j=0; j<5; j++){
                spawn_Arr[i,j] = 7;               
            }
        }

        Green_assign(green_no);

        for (var i=0; i<5; i++){
            for(var j=0; j<5; j++){
                if(spawn_Arr[i,j] == solutionArr[i,j]){
                    freq_Arr[spawn_Arr[i,j]]--;
                }
                else if((i==1 && j ==1) || (i==3 && j==1) || (i==1 && j==3) || (i==3 && j==3)){
                    spawn_Arr[i,j] = 6;
>>>>>>> Stashed changes
                }
            }
        }

<<<<<<< Updated upstream
        string[] strings = {"","","","",""};
        s[1,1] = "⬜"; s[1,3] = "⬜"; s[3,1] = "⬜"; s[3,3] = "⬜"; 
        for(var k=0; k<5; k++){
            for(var l=0;l<5;l++){
                strings[k] = strings[k] + s[l,4-k];
            }
        }
        ShareMsg = strings[0] + "\n" + strings[1] + "\n" + strings[2] + "\n" + strings[3] + "\n" + strings[4];
    }

    void Share_msg(){
        msg.SetActive(true);
        shareTrigger(ShareMsg);
=======
        int sum = 0;
        for(var i=0; i<6; i++){
            sum = sum + freq_Arr[i];
        }
        int rand;
        for (var i=0; i<5; i++){
            for(var j=0; j<5; j++){
                if(spawn_Arr[i,j] == 7){
                    List<int> list0 = new List<int>() {0,1,2,3,4,5};

                    
                    int t = 0;
                    for(var k = 0;k<6;k++){
                        if(freq_Arr[k] == 0){
                            list0.Remove(k);
                            t++;
                        }
                    }

                    if(t != 5){
                        list0.Remove(solutionArr[i,j]);
                    }

                    rand = list0[Random.Range(0,list0.Count)];
                    spawn_Arr[i,j] = rand;
                    freq_Arr[rand]--;
                }
            }
        }
    }

    void Share_msg(){
        shareTrigger();
        msg.SetActive(true);
>>>>>>> Stashed changes
    }


    void HelpScreen(){
        if(HowToPlay.activeSelf){
            HowToPlay.SetActive(false);
        }
        else{
            HowToPlay.SetActive(true);
        }
    }

<<<<<<< Updated upstream
=======
    /*void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Play(){

        green_no = int.Parse(greenMinBound.text);
        Debug.Log(greenMinBound.text.ToString());

        if(row_toggle.isOn){
            RowLogic = true;
        }
        else{
            RowLogic = false;
        }

        if(rowSum_toggle.isOn){
            RowSum.SetActive(true);
        }
        else{
            RowSum.SetActive(false);
        }


        
    }

    public void update_sol(){

        string delimiter = ",";
        string[] strings = new string[5];
        for(var k=0; k<5; k++){
            int[] integers = new int[5];
            for(var l=0;l<5;l++){
                integers[l] = solutionArr[k,l];
            }
            strings[k] = integers.Select(i => i.ToString()).Aggregate((i, j) => i + delimiter + j);
        }

        Sol.text = "Solution Array: {" + strings[0] + "},{" + strings[1] + "},{" + strings[2] + "},{" + strings[3] + "},{" + strings[4] + "}";
    }*/

>>>>>>> Stashed changes

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

<<<<<<< Updated upstream
=======
    void Green_assign(int a){
        int[] Array_number = Random_slot_pick(a);
        int[] slot_pos = new int[2];
        for(var m=0; m<a; m++){
            slot_pos[0] = slot_pos_generator(Array_number[m])[0];
            slot_pos[1] = slot_pos_generator(Array_number[m])[1];
            spawn_Arr[slot_pos[0],slot_pos[1]] = solutionArr[slot_pos[0],slot_pos[1]];
        }
    }

    int[] Random_slot_pick(int a){
        int[] arr = new int[a];
        for(var l=0; l<a; l++){
            arr[l] = 21;
        }
        int rand;

        for(var k=0; k<a; k++){
            rand = Random.Range(0,21);
            while(System.Array.IndexOf(arr,rand)!=-1){
                rand = Random.Range(0,21);
            }
            arr[k] = rand;
        }

        return arr;    
    }    

    int[] slot_pos_generator(int a){
        int slot_pos0;
        int slot_pos1;
        int[] arr = new int[2];

        if(a<5){
            slot_pos0 = a;
            slot_pos1 = 0;
        }
        else if(a<8){
            if(a==5){slot_pos0 = 0;}
            else if(a==6){slot_pos0 = 2;}
            else{slot_pos0 = 4;}
            slot_pos1 = 1;
        }
        else if(a<13){
            slot_pos0 = a-8;
            slot_pos1 = 2;
        }
        else if(a<16){
            if(a==13){slot_pos0 = 0;}
            else if(a==14){slot_pos0 = 2;}
            else{slot_pos0 = 4;}
            slot_pos1 = 3;
        }
        else{
            slot_pos0 = a-16;
            slot_pos1 = 4;
        }

        arr[0] = slot_pos1;
        arr[1] = slot_pos0;

        return arr;

    }

>>>>>>> Stashed changes
    public static void update_color()
    {
    foreach(GameObject slot in RandomSpawnGenerator.slotList)
    {
        slot.GetComponent<Dropper>().Check_color();
    }
    }

}
