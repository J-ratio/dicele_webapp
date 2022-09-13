using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;



public class roll_dice : MonoBehaviour
{

    //Gameobjects
    public GameObject diceOg;
    GameObject diceClone;//instantiated spawn dice
    [SerializeField]
    private Button roll_btn;
    [SerializeField]
    private Button play_btn;
    [SerializeField]
    private Sprite[] dice_images; //die1 to die6 images
    [SerializeField]
    private Sprite[] enabled_disabled_button_images;


    //Gamevariables
    private int random_number; //number assigned to spawn dice
    public static int[] count_Array = new int[6]; //stores frequency of each dice number left to be placed in the board; eg: if there are 4 die1 remaining to be placed on board then count_Array[0] = 4;
    private static int spawn_dice_count = 1; //no. of dice to be spawned per roll
    private int[] random_array = new int[spawn_dice_count]; //stores dice numbers of spwaned dice
    public static int[ , ] Ref_Array = {{4,5,3,4,5},{2,3,0,0,4},{5,0,1,2,1},{1,0,2,2,4},{4,5,5,0,0}}; //Correct positions of dice on board
    private GameObject[] dice_Array = new GameObject[spawn_dice_count];//stores instantiated dice objects
    // public static bool isMoved = false;
    public TextMeshProUGUI play_time;
    public TextMeshProUGUI matched;
    /*public TextMeshProUGUI turns;    
    public TextMeshProUGUI moves; */







    void Start()
    {   
        //disable play button at start
        play_btn.GetComponent<Button>().enabled = false;
        play_btn.GetComponent<Image>().sprite = enabled_disabled_button_images[0];

        //add listeners
        roll_btn.onClick.AddListener(Roll_dice);
        play_btn.onClick.AddListener(Play);

        //initialise count_Array at start
        count_Array[0] =  6; count_Array[1] =  3; count_Array[2] =  4; count_Array[3] =  2; count_Array[4] =  5; count_Array[5] =  5;  //{6,3,4,2,5,5}

        //set moves count at start as 0
        slot_dice_script.moves = 0;
    }

    void Update(){

        play_time.text = timer_script.minutes.ToString() + ":" + timer_script.seconds.ToString() + " sec";
        matched.text = "Matched: " + slot_script.matched.ToString() + "/25";
        /*turns.text = "Turns: " + ((slot_script.Total_play)/25).ToString();        
        moves.text = "Moves: " + slot_dice_script.moves.ToString(); */        
    }


    void Roll_dice(){

        if(count_Array[0]>0 || count_Array[1]>0 || count_Array[2]>0 || count_Array[3]>0 || count_Array[4]>0 || count_Array[5]>0) //check if the board has any remaining dice slots
        {
            for(var i = 0; i<spawn_dice_count; i++){
                diceClone = Instantiate(diceOg,new Vector3(0,-1.7f,0),Quaternion.identity); //instantiate a spawn die
                random_number = Random.Range(0,dice_images.Length); //assign the die a random number 

                while(count_Array[random_number]<= 0) {random_number = Random.Range(0,dice_images.Length);} //check if the assigned number is present in remaining slots of the board, if not then assign and check again
                    
                diceClone.GetComponent<SpriteRenderer>().sprite = dice_images[random_number]; //assign the sprite image for the dice number
                diceClone.GetComponent<dice_script>().dice_number = random_number; //store the randomly assigned dice number

                random_array[i] = random_number;
                dice_Array[i] = diceClone;
                }
        }
        
        roll_btn.GetComponent<Button>().enabled = false; //disable roll 
        roll_btn.GetComponent<Image>().sprite = enabled_disabled_button_images[0];
    }

    void Play(){

        roll_btn.GetComponent<Button>().enabled = true; //enable roll
        roll_btn.GetComponent<Image>().sprite = enabled_disabled_button_images[2];
        if(count_Array[0]>0 || count_Array[1]>0 || count_Array[2]>0 || count_Array[3]>0 || count_Array[4]>0 || count_Array[5]>0)
        {
            play_btn.GetComponent<Button>().enabled = false; //disable play unless all dice are placed on board
            play_btn.GetComponent<Image>().sprite = enabled_disabled_button_images[0];
        }
        else{
            roll_btn.GetComponent<Button>().enabled = false;
            roll_btn.GetComponent<Image>().sprite = enabled_disabled_button_images[0];
        }
        
        
        for(var l = 0; l<spawn_dice_count; l++){
            Destroy(dice_Array[l]);
            }
        }
}
