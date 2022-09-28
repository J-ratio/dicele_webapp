using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class slot_script : MonoBehaviour , IPointerDownHandler
{
    //Gameobjects
    public GameObject dice;
    public GameObject pixel;
    public Button play_btn;
    [SerializeField]
    private Sprite play_btn_enabled_sprite;
    [SerializeField]
    private GameObject greenborder;

    //Gamevariables
    public static bool slot_tag; //is true when a spawn dice is selected before slot is selected
    public static bool dice_slot_tag; //is true when a slot dice is selected before slot is selected
    public static Sprite dice_sprite; //stores sprite of selected spawn dice
    public static Sprite slot_dice_sprite; //stores sprite of selected slot dice
    public int[] slot_pos = new int[2]; //store the slot position in the play_board as (x,y)
    public static int[,] board_dice_count = new int[5,6]; //stores the frequency of dice numbers placed on board column wise; eg: if there are 2 die5 placed on 1st column then board_dice_count[0,4] = 2;
    public static int [,] green_dice_count = new int[5,6]; //stores matched dice numbers on board column wise; eg: if the 2 die4 images are matcched in 1st column then green_dice_count[0,3] = 2;
    public static int[,] Ref_board_dice_count = {{0,0,0,1,2,2},{2,0,1,1,1,0},{1,2,1,0,0,1},{1,1,2,0,1,0},{2,0,0,0,1,2}}; //stores the dice number frequency column wise in solution ie Rsf_array
    public static Color[,] pixel_array = {{new Color(231/255f,230/255f,233/255f) , new Color(67/255f,84/255f,219/255f) , new Color(67/255f,84/255f,219/255f) , new Color(67/255f,84/255f,219/255f) , new Color(231/255f,230/255f,233/255f)},{ new Color(67/255f,84/255f,219/255f) , new Color(67/255f,84/255f,219/255f) , new Color(42/255f,38/255f,49/255f) , new Color(67/255f,84/255f,219/255f) , new Color(251/255f,209/255f,15/255f)},{ new Color(67/255f,84/255f,219/255f) , new Color(67/255f,84/255f,219/255f) , new Color(255/255f,0/255f,0/255f) , new Color(67/255f,84/255f,219/255f) , new Color(251/255f,209/255f,15/255f)},{ new Color(67/255f,84/255f,219/255f) , new Color(67/255f,84/255f,219/255f) , new Color(42/255f,38/255f,49/255f) , new Color(67/255f,84/255f,219/255f) , new Color(251/255f,209/255f,15/255f)},{ new Color(231/255f,230/255f,233/255f) , new Color(67/255f,84/255f,219/255f) , new Color(67/255f,84/255f,219/255f) , new Color(67/255f,84/255f,219/255f) , new Color(231/255f,230/255f,233/255f)}};
    public static int Total_play; //counter for no. of times play is pressed
    public static int matched; //counter for no. of dice matched on the board
    public static bool enable_play = false;




    public void OnPointerDown(PointerEventData eventData){
        if(dice.GetComponent<SpriteRenderer>().sprite == null && slot_dice_script.slot_dice_tag == true)
            {
            board_dice_count[slot_pos[0],slot_dice_script.selected_slot_dice_number] ++;        
            dice.GetComponent<SpriteRenderer>().sprite = slot_dice_sprite;
            dice_slot_tag = true;
            slot_dice_script.Yellow_selected = false;
            slot_dice_script.White_selected = false;
            dice.SetActive(true);
            dice.GetComponent<slot_dice_script>().dice_number = slot_dice_script.selected_slot_dice_number;

            if(slot_dice_script.selected_slot_dice.GetComponent<slot_dice_script>().Correct_column == slot_pos[0])
                {
                if(slot_dice_script.selected_slot_dice.GetComponent<slot_dice_script>().Correct_row == slot_pos[1])
                    {
                    dice.GetComponent<SpriteRenderer>().color = Color.green;
                    }
                else{
                    dice.GetComponent<SpriteRenderer>().color = Color.yellow;
                    }
                }
            dice.GetComponent<slot_dice_script>().Correct_row = slot_dice_script.selected_slot_dice.GetComponent<slot_dice_script>().Correct_row;
            dice.GetComponent<slot_dice_script>().Correct_column = slot_dice_script.selected_slot_dice.GetComponent<slot_dice_script>().Correct_column; 
            if(slot_dice_script.selected_slot_dice.GetComponent<slot_dice_script>().isboping){
                slot_dice_script.dice_moved++;
            }
            slot_dice_script.selected_slot_dice.GetComponent<slot_dice_script>().Stop_boping();         
            }
        else if(dice.GetComponent<SpriteRenderer>().sprite == null && dice_script.dice_tag == true)
            {
            board_dice_count[slot_pos[0],dice_script.selected_dice_number] ++;
            dice.GetComponent<SpriteRenderer>().sprite = dice_sprite;
            slot_tag = true;
            dice.SetActive(true);
            dice.GetComponent<slot_dice_script>().dice_number = dice_script.selected_dice_number;
            dice.GetComponent<slot_dice_script>().Stop_boping();
            dice_script.dice_onboard ++;
            enable_play = true;
            }

        check_enable_playBtn();
    }

    public void check_enable_playBtn(){
        if(slot_dice_script.dice_moved == (dice_script.dice_onboard) && enable_play){
            play_btn.GetComponent<Button>().enabled = true;
            play_btn.GetComponent<Image>().sprite = play_btn_enabled_sprite;
            }
    }



    void Start()
    {
        play_btn.onClick.AddListener(Onplay);
        slot_tag = false;
        dice_slot_tag = false;
        matched = 0;
        Total_play = 0;
        dice.GetComponent<slot_dice_script>().Correct_column = 6;
        dice.GetComponent<slot_dice_script>().Correct_row = 6;

        for(var i = 0; i < 5; i++){
            for(var j = 0; j<6; j++){
                green_dice_count[i,j] = 0;
            }
        }
    }

    void Onplay(){
        
        if(dice.activeSelf){
        dice.GetComponent<slot_dice_script>().Start_bopping();
        }
        enable_play = false;
        slot_dice_script.dice_moved = 1;
        slot_dice_script.Yellow_selected = false;
        slot_dice_script.White_selected = false;
        Invoke("delay",0.1f);
    }

    void delay(){
        if(dice.GetComponent<SpriteRenderer>().sprite != null && dice.GetComponent<SpriteRenderer>().color != Color.green)
        {
        if(roll_dice.Ref_Array[slot_pos[0],slot_pos[1]] == dice.GetComponent<slot_dice_script>().dice_number)
            {
            pixel.SetActive(true);
            dice.GetComponent<SpriteRenderer>().color = Color.green;
            //roll_dice.Ref_Array[slot_pos[0],slot_pos[1]] = 6;
            dice.GetComponent<slot_dice_script>().Correct_column = slot_pos[0];
            dice.GetComponent<slot_dice_script>().Correct_row = slot_pos[1];
            matched ++;
            dice.GetComponent<slot_dice_script>().Stop_boping(); 
            dice_script.dice_onboard --;  
            Invoke("pixel_delay", 1.0f);
            }
        else if(ColumnStateCheck() && (green_dice_count[slot_pos[0],dice.GetComponent<slot_dice_script>().dice_number] < Ref_board_dice_count[slot_pos[0],dice.GetComponent<slot_dice_script>().dice_number]))
            {
            dice.GetComponent<SpriteRenderer>().color = Color.yellow;
            dice.GetComponent<slot_dice_script>().Correct_column = slot_pos[0];
            }
        else
            {
            dice.GetComponent<SpriteRenderer>().color = Color.white;
            dice.GetComponent<slot_dice_script>().Correct_column = 6;
            }
        }
        Total_play ++;
    }

    bool ColumnStateCheck(){
        for(var i=0; i<5; i++){
            if(roll_dice.Ref_Array[slot_pos[0],i] == dice.GetComponent<slot_dice_script>().dice_number){
                return true;
            }
        }
        return false;
    }

    void pixel_delay(){
            
            pixel.GetComponent<SpriteRenderer>().color = pixel_array[slot_pos[0],slot_pos[1]];
            pixel.GetComponent<SpriteRenderer>().enabled = true;
    }


    void Update(){
        if(slot_dice_script.Yellow_selected){
            if(slot_dice_script.selected_slot_dice.GetComponent<slot_dice_script>().parent_slot.GetComponent<slot_script>().slot_pos[0] == slot_pos[0] && slot_dice_script.selected_slot_dice.GetComponent<slot_dice_script>().parent_slot.GetComponent<slot_script>().slot_pos[1] != slot_pos[1]){
                greenborder.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        else if(slot_dice_script.White_selected){
            if(slot_dice_script.selected_slot_dice.GetComponent<slot_dice_script>().parent_slot.GetComponent<slot_script>().slot_pos[0] != slot_pos[0]){
                greenborder.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        else{
            greenborder.GetComponent<SpriteRenderer>().enabled = false;
            }
    }
    
}
