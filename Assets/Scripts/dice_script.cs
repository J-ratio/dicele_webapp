using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class dice_script : MonoBehaviour , IPointerDownHandler{
   
    //GameObjects
    public static GameObject selected_dice; //stores the spawn dice gameobject when selected

    //Gamevariables
    public int dice_number; 
    public static bool dice_tag; //is true when the dice is selected
    public static int selected_dice_number;// stores number of the number of selected spawn dice


    public void OnPointerDown(PointerEventData eventData)
    {
    slot_script.dice_sprite = GetComponent<SpriteRenderer>().sprite; //send the selected dice sprite to slot 
    dice_tag = true;
    selected_dice = this.gameObject;
    selected_dice_number = dice_number;
    slot_dice_script.slot_dice_tag = false;
    }


    void Start()
    {
        dice_tag = false;
    }

    void Update()
    {
        if(dice_tag == true && slot_script.slot_tag == true){
            roll_dice.count_Array[selected_dice_number] = roll_dice.count_Array[selected_dice_number] - 1;
            Destroy(selected_dice);
            dice_tag = false;
            slot_script.slot_tag = false;
        }
    }
}
