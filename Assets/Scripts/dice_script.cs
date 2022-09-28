using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class dice_script : MonoBehaviour //, IPointerDownHandler, IPointerUpHandler
{
   
    //GameObjects
    public static GameObject selected_dice; //stores the spawn dice gameobject when selected

    //Gamevariables
    public int dice_number; 
    public static bool dice_tag; //is true when the dice is selected
    public static int selected_dice_number;// stores number of the number of selected spawn dice
    private Vector3 delta = new Vector3(0.1f,0.1f,0);
    public static int dice_onboard;


    /*public void OnPointerDown(PointerEventData eventData)
    {
    slot_script.dice_sprite = GetComponent<SpriteRenderer>().sprite; //send the selected dice sprite to slot 
    dice_tag = true;
    selected_dice = this.gameObject;
    selected_dice_number = dice_number;
    slot_dice_script.slot_dice_tag = false;
    this.transform.localScale -= delta;
    slot_dice_script.Yellow_selected =false;
    slot_dice_script.White_selected = false;
    if(slot_dice_script.selected_slot_dice != null){
        slot_dice_script.selected_slot_dice.GetComponent<SpriteRenderer>().color = slot_dice_script.selected_slot_dice_color;
    }
    GetComponent<SpriteRenderer>().color = Color.gray;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
    this.transform.localScale +=delta;
    }*/

    void Start()
    {
        dice_tag = false;
    }



    public void dothis0(){
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll (ray, Mathf.Infinity);
        foreach (var hit in hits) {
            if (hit.collider.CompareTag("spawn_dice")) {
                slot_script.dice_sprite = GetComponent<SpriteRenderer>().sprite; //send the selected dice sprite to slot 
                dice_tag = true;
                selected_dice = this.gameObject;
                selected_dice_number = dice_number;
                slot_dice_script.slot_dice_tag = false;
                
                slot_dice_script.Yellow_selected =false;
                slot_dice_script.White_selected = false;
                if(slot_dice_script.selected_slot_dice != null){
                    slot_dice_script.selected_slot_dice.GetComponent<SpriteRenderer>().color = slot_dice_script.selected_slot_dice_color;
                }
                GetComponent<SpriteRenderer>().color = Color.gray;
                this.transform.localScale -= delta;
            }
        }
    }

    public void dothis1(){
        if (this.transform.localScale.x < 1.0f) {
                this.transform.localScale +=delta;
            }
    }


    void Update()
    {
        if(dice_tag == true && slot_script.slot_tag == true){
            roll_dice.count_Array[selected_dice_number] = roll_dice.count_Array[selected_dice_number] - 1;
            Destroy(selected_dice);
            dice_tag = false;
            slot_script.slot_tag = false;
        }

        if (Input.GetMouseButtonDown(0)){
            dothis0();
        }
        if (Input.GetMouseButtonUp(0)){
            dothis1();
        }
    }
}
