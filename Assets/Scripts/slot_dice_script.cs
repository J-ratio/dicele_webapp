using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class slot_dice_script : MonoBehaviour , IPointerDownHandler, IPointerUpHandler
{
    //Gameobjects
    public GameObject parent_slot; //store the slot in which this slot dice is stores

    //Gamevariables
    public static bool slot_dice_tag = false; //is true when a slot dice is selected
    public static bool Yellow_selected = false;
    public static bool White_selected = false;
    public static GameObject selected_slot_dice; //stores the selected slot dice
    public static int selected_slot_dice_number; //stores number on selected slot dice
    public static Color selected_slot_dice_color;
    public int dice_number; //stores the slot dice number
    public int Correct_column;
    public int Correct_row;
    public static int moves = 0;
    public int temp;
    private Vector3 delta = new Vector3(0.1f,0.1f,0);
    private Vector3 delta1 = new Vector3(0.01f,0.01f,0);
    public bool isboping = false;
    public static int dice_moved = 1;




    public void OnPointerDown(PointerEventData eventData){
        if(GetComponent<SpriteRenderer>().color != Color.green)
            {
            this.transform.localScale -= delta;
            if(!(slot_dice_tag))
                {
                slot_script.slot_dice_sprite = GetComponent<SpriteRenderer>().sprite;
                slot_dice_tag = true;
                selected_slot_dice = this.gameObject;
                selected_slot_dice_color = GetComponent<SpriteRenderer>().color;
                selected_slot_dice_number = this.GetComponent<slot_dice_script>().dice_number;
                if(GetComponent<SpriteRenderer>().color == Color.yellow){
                    Yellow_selected = true;
                }
                else{
                    White_selected = true;
                }
                GetComponent<SpriteRenderer>().color = Color.gray;
                if(dice_script.selected_dice !=null){
                    dice_script.selected_dice.GetComponent<SpriteRenderer>().color = Color.white;
                }
                

                }
            else if(selected_slot_dice != this.gameObject)
                {
                slot_script.board_dice_count[parent_slot.GetComponent<slot_script>().slot_pos[0],dice_number] --;
                slot_script.board_dice_count[selected_slot_dice.GetComponent<slot_dice_script>().parent_slot.GetComponent<slot_script>().slot_pos[0],selected_slot_dice_number] --;

                Yellow_selected = false;
                White_selected = false;
                if(isboping){
                    dice_moved++;
                }
                if(selected_slot_dice.GetComponent<slot_dice_script>().isboping){
                    dice_moved++;
                }                
                Stop_boping();
                selected_slot_dice.GetComponent<slot_dice_script>().Stop_boping();

                selected_slot_dice.GetComponent<SpriteRenderer>().sprite = this.GetComponent<SpriteRenderer>().sprite;
                this.GetComponent<SpriteRenderer>().sprite =  slot_script.slot_dice_sprite;
                selected_slot_dice.GetComponent<slot_dice_script>().dice_number = dice_number;
                dice_number = slot_dice_script.selected_slot_dice_number;
                temp = Correct_column;
                Correct_column = selected_slot_dice.GetComponent<slot_dice_script>().Correct_column;
                selected_slot_dice.GetComponent<slot_dice_script>().Correct_column = temp;

                if(Correct_column == parent_slot.GetComponent<slot_script>().slot_pos[0])
                    {
                    GetComponent<SpriteRenderer>().color = Color.yellow;
                    }
                else
                    {
                    GetComponent<SpriteRenderer>().color = Color.white;
                    }

                if(selected_slot_dice.GetComponent<slot_dice_script>().Correct_column == selected_slot_dice.GetComponent<slot_dice_script>().parent_slot.GetComponent<slot_script>().slot_pos[0])
                    {
                    selected_slot_dice.GetComponent<SpriteRenderer>().color = Color.yellow;
                    }
                else
                    {
                    selected_slot_dice.GetComponent<SpriteRenderer>().color = Color.white;
                    }


                slot_script.board_dice_count[parent_slot.GetComponent<slot_script>().slot_pos[0],dice_number] ++;
                slot_script.board_dice_count[selected_slot_dice.GetComponent<slot_dice_script>().parent_slot.GetComponent<slot_script>().slot_pos[0],selected_slot_dice.GetComponent<slot_dice_script>().dice_number] ++;
                slot_dice_script.selected_slot_dice = null;
                this.transform.localScale -= delta;
                slot_dice_tag = false;
                moves ++;

                parent_slot.GetComponent<slot_script>().check_enable_playBtn();  
                }
            }
        }

    public void OnPointerUp(PointerEventData eventData){
        this.transform.localScale += delta;
    }


    // Update is called once per frame
    void Update()
    { 
        if(slot_dice_tag == true && slot_script.dice_slot_tag == true)
            {
            slot_script.board_dice_count[selected_slot_dice.GetComponent<slot_dice_script>().parent_slot.GetComponent<slot_script>().slot_pos[0],selected_slot_dice_number] --;
            selected_slot_dice.GetComponent<SpriteRenderer>().color = Color.white;
            selected_slot_dice.GetComponent<SpriteRenderer>().sprite = null;
            slot_dice_tag = false;
            slot_script.dice_slot_tag = false;
            selected_slot_dice.SetActive(false);
            selected_slot_dice.GetComponent<slot_dice_script>().Correct_column = 6;
            selected_slot_dice.GetComponent<slot_dice_script>().Correct_row = 6;
            selected_slot_dice = null;
            moves ++;
            }
    }

    public void Start_bopping(){
        StartCoroutine("scaleUpDown_anim");
        isboping = true;
    }

    public void Stop_boping(){
        StopCoroutine("scaleUpDown_anim");
        this.transform.localScale = new Vector3(0.9f,0.9f,0.9f);
        isboping = false;
    }

    IEnumerator scaleUpDown_anim(){
        
        int t3 = 1000;
        while(t3>0){
            int t1 =10;
            int t2 =10;
            while(t1>0){
                scaleDown();
                yield return new WaitForSeconds(0.075f);
                t1--;
            }
            while(t2>0){
                scaleUp();
                yield return new WaitForSeconds(0.075f);
                t2--;
            }
            t3--;
        }
    }

    void scaleUp(){
            this.transform.localScale += delta1;
        }

    void scaleDown(){
            this.transform.localScale -= delta1;
        }
}
