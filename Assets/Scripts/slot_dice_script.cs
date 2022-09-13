using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class slot_dice_script : MonoBehaviour , IPointerDownHandler
{
    //Gameobjects
    public GameObject parent_slot; //store the slot in which this slot dice is stores

    //Gamevariables
    public static bool slot_dice_tag = false; //is true when a slot dice is selected
    public static GameObject selected_slot_dice; //stores the selected slot dice
    public static int selected_slot_dice_number; //stores number on selected slot dice
    public int dice_number; //stores the slot dice number
    public int Correct_column;
    public int Correct_row;
    public static int moves = 0;
    public int temp;




    public void OnPointerDown(PointerEventData eventData){
        if(GetComponent<SpriteRenderer>().color != Color.green)
            {
            if(!(slot_dice_tag))
                {
                slot_script.slot_dice_sprite = GetComponent<SpriteRenderer>().sprite;
                slot_dice_tag = true;
                selected_slot_dice = this.gameObject;
                selected_slot_dice_number = this.GetComponent<slot_dice_script>().dice_number;
                }
            else if(selected_slot_dice != this.gameObject)
                {
                slot_script.board_dice_count[parent_slot.GetComponent<slot_script>().slot_pos[0],dice_number] --;
                slot_script.board_dice_count[selected_slot_dice.GetComponent<slot_dice_script>().parent_slot.GetComponent<slot_script>().slot_pos[0],selected_slot_dice_number] --;

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
            
                slot_dice_tag = false;
                moves ++;
                }
            }
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
            moves ++;
            }
    }
}
