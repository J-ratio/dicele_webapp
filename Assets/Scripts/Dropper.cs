using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{

    public GameObject current_dice;
    [SerializeField]
    public GameObject GameManager;
    public int[] slot_pos = new int[2];
    Vector3 delta2 = new Vector3 (0,0,-0.1f);
    public List<int> yellow_arr = new List<int>();
    private int green_number;
    public static int swap_count = 0;
    

    void Awake(){
        RandomSpawnGenerator.slotList.Remove(this.gameObject);
        GameManager.GetComponent<RandomSpawnGenerator>().Play_btn.onClick.AddListener(Play);
    }

    void Play(){
        StartCoroutine("delay");
    }

    IEnumerator delay(){
        yield return new WaitForSeconds(2f*Time.deltaTime);
        gameObject.GetComponent<Dropper>().enabled = true;
    }

    void Start(){
        green_number = GameManager.GetComponent<RandomSpawnGenerator>().Get_Green_number(slot_pos[0],slot_pos[1]);
        RandomSpawnGenerator.slotList.Add(this.gameObject);
        current_dice.GetComponent<SpriteRenderer>().sprite = GameManager.GetComponent<RandomSpawnGenerator>().dice_images[GameManager.GetComponent<RandomSpawnGenerator>().spawn_Arr[slot_pos[0],slot_pos[1]]];
        current_dice.GetComponent<Dragger>().dice_number = GameManager.GetComponent<RandomSpawnGenerator>().spawn_Arr[slot_pos[0],slot_pos[1]];
        Check_color();

    }

    public void Check_color(){
        
        if(current_dice.GetComponent<Dragger>().dice_number == green_number){
            current_dice.GetComponent<SpriteRenderer>().color = Color.green;
            GameManager.GetComponent<RandomSpawnGenerator>().solutionArr[slot_pos[0],slot_pos[1]] = 6;
            GetComponent<BoxCollider2D>().enabled = false;
            current_dice.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine("delay3");
        }
        else{
            StartCoroutine("delay2");
        }
        
    }

    

    IEnumerator delay3(){
        yield return new WaitForSeconds(2f*Time.deltaTime);
        RandomSpawnGenerator.slotList.Remove(this.gameObject);
    }

    IEnumerator delay2(){
        yield return new WaitForSeconds(0.5f*Time.deltaTime);
        yellow_arr = GameManager.GetComponent<RandomSpawnGenerator>().Get_Yellow_numbers(slot_pos[0],slot_pos[1]);
            if(yellow_arr.Contains(current_dice.GetComponent<Dragger>().dice_number)){
            current_dice.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            else{
            current_dice.GetComponent<SpriteRenderer>().color = Color.white;
            }
        yield break;
    }

    public void MoveToSlot(GameObject new_slot){
        StartCoroutine(Routine1(new_slot,current_dice));
        new_slot.GetComponent<Dropper>().current_dice = current_dice;
        current_dice.GetComponent<Dragger>().current_slot = new_slot;
        swap_count++;
        GameManager.GetComponent<RandomSpawnGenerator>().swapCount.text = "Swaps: " + swap_count.ToString();      
    }

    public IEnumerator Routine1(GameObject new_slot,GameObject current_dice)
    {
        while(Vector3.Distance(current_dice.transform.position,new_slot.transform.position)>0.01)
        {
            current_dice.transform.position = Vector3.MoveTowards(current_dice.transform.position,new_slot.gameObject.transform.position,50*Time.deltaTime);
            yield return new WaitForSeconds(0.01f*Time.deltaTime);
        }
        current_dice.transform.position += delta2;
        yield break;
         
    }

}