using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class green_state_update : MonoBehaviour
{
    public Button play_btn;
    private GameObject die;
    
    // Start is called before the first frame update
    void Start()
    {
        play_btn.onClick.AddListener(play);
    }

    void play(){
        die = this.GetComponent<slot_script>().dice;
        if( die.GetComponent<SpriteRenderer>().sprite != null && die.GetComponent<SpriteRenderer>().color != Color.green)
            {   
            if(roll_dice.Ref_Array[this.GetComponent<slot_script>().slot_pos[0],this.GetComponent<slot_script>().slot_pos[1]] == die.GetComponent<slot_dice_script>().dice_number)
                {
                slot_script.green_dice_count[this.GetComponent<slot_script>().slot_pos[0],die.GetComponent<slot_dice_script>().dice_number] ++;
                }
            }
        }

}
