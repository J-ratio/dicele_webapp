using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class archive_dropper : MonoBehaviour
{
    public GameObject current_dice;
    [SerializeField]
    public GameObject ArchiveManager;
    [SerializeField]
    private GameObject[] Stars;
    [SerializeField]
    private Sprite Star;

    public int[] slot_pos = new int[2];
    Vector3 delta2 = new Vector3 (0,0,-0.2f);
    public List<int> yellow_arr = new List<int>();
    public int green_number;
    public static int archive_swap_count;
    public static int archive_matched;
    public static int star_count = 5;
    public static int Total_sec;
    public static int Shared_Total_sec;

    [DllImport("__Internal")]
    public static extern void OnArchiveFinish(bool isSolved,int swap_count,int archive_number);


    void OnEnable(){
        archiveManager.ArchiveSlotList.Remove(this.gameObject);
        StartCoroutine("delay");
    }

    IEnumerator delay(){
        yield return new WaitForSeconds(0.4f*Time.deltaTime);
        GetComponent<BoxCollider2D>().enabled = true;
        current_dice.GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<archive_dropper>().enabled = true;
        Archive_Start();
    }

    void Archive_Start()
    {
        archive_swap_count = 21;
        archive_matched = 0;
        Total_sec = 0;
        star_count = 5;
        ArchiveManager.GetComponent<archiveManager>().swapCount.text = archive_swap_count.ToString();
        green_number = ArchiveManager.GetComponent<archiveManager>().Get_Green_number(slot_pos[0],slot_pos[1]);
        archiveManager.ArchiveSlotList.Add(this.gameObject);
        current_dice.GetComponent<SpriteRenderer>().sprite = ArchiveManager.GetComponent<archiveManager>().dice_images[ArchiveManager.GetComponent<archiveManager>().spawn_Arr[slot_pos[0],slot_pos[1]]];
        current_dice.GetComponent<archive_dragger>().dice_number = ArchiveManager.GetComponent<archiveManager>().spawn_Arr[slot_pos[0],slot_pos[1]];
        Invoke("Check_color",0.2f);     
    }

    public void Check_color(){
        
        if(current_dice.GetComponent<archive_dragger>().dice_number == green_number){
            current_dice.GetComponent<SpriteRenderer>().sprite = ArchiveManager.GetComponent<archiveManager>().GameManager.GetComponent<RandomSpawnGenerator>().GreenDiceImages[current_dice.GetComponent<archive_dragger>().dice_number];//new Color(106/255f,192/255f,81/255f);
            current_dice.GetComponent<SpriteRenderer>().color = Color.white; 
            ArchiveManager.GetComponent<archiveManager>().solutionArr[slot_pos[0],slot_pos[1]] = 6;
            GetComponent<BoxCollider2D>().enabled = false;
            current_dice.GetComponent<BoxCollider2D>().enabled = false;
            archive_matched++;
            if(archive_matched == 21){
                StopCoroutine("GameTimer");
                ShowGameTime();
                ArchiveManager.GetComponent<archiveManager>().Moves.text = (21-archive_swap_count).ToString() + "/21";
                if(archive_swap_count<6){
                    star_count = archive_swap_count;
                }
                ArchiveManager.GetComponent<archiveManager>().PlayWinAnim();
                MakeWinShare(); 
                ArchiveManager.GetComponent<archiveManager>().LastArchiveSwapCount = archive_swap_count;
                OnArchiveFinish(true,archive_swap_count,ArchiveManager.GetComponent<archiveManager>().LastArchiveOpened);

                Invoke("ShowWinScreen",4.0f);            
            }
            StartCoroutine("delay3");
        }
        else{
            StartCoroutine("delay2");
        }
        
    }

    void ShowWinScreen(){
        ArchiveManager.GetComponent<archiveManager>().WinningScreen.SetActive(true);
        ArchiveManager.GetComponent<archiveManager>().ResultScreen.SetActive(true);
        ShowStars();
    }

    void ShowStars(){
        StartCoroutine("StarAnim");
    }   

    IEnumerator StarAnim(){
        yield return new WaitForSeconds(0.7f);
        for(var i = 0; i < star_count; i++){
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(IncreaseStarVisibility(Stars[i]));
        }
        yield return new WaitForSeconds(0.1f*star_count);
        ArchiveManager.GetComponent<archiveManager>().ResultsMsg.SetActive(true);
    }

    IEnumerator IncreaseStarVisibility(GameObject star){
        for(var i = 0; i<51;i++){
            yield return new WaitForSeconds(0.001f);
            star.GetComponent<Image>().color = new Color(1,1,1,i*0.02f);
        }
    } 

    IEnumerator delay3(){
        yield return new WaitForSeconds(1f*Time.deltaTime);
        archiveManager.ArchiveSlotList.Remove(this.gameObject);
    }

    IEnumerator delay2(){
        yield return new WaitForSeconds(0.5f*Time.deltaTime);
        yellow_arr = ArchiveManager.GetComponent<archiveManager>().Get_Yellow_numbers(slot_pos[0],slot_pos[1]);
            if(yellow_arr.Contains(current_dice.GetComponent<archive_dragger>().dice_number)){
            current_dice.GetComponent<SpriteRenderer>().color = new Color(243/255f,193/255f,58/255f);;
            }
            else{
            current_dice.GetComponent<SpriteRenderer>().color = Color.white;
            }
        yield break;
    }


    public void MoveToSlot(GameObject new_slot){
        StartCoroutine(Routine1(new_slot,current_dice));
        ArchiveManager.GetComponent<archiveManager>().spawn_Arr[new_slot.GetComponent<archive_dropper>().slot_pos[0],new_slot.GetComponent<archive_dropper>().slot_pos[1]] = current_dice.GetComponent<archive_dragger>().dice_number;
        ArchiveManager.GetComponent<archiveManager>().spawn_Arr[slot_pos[0],slot_pos[1]] = new_slot.GetComponent<archive_dropper>().current_dice.GetComponent<archive_dragger>().dice_number;
        new_slot.GetComponent<archive_dropper>().current_dice = current_dice;
        current_dice.GetComponent<archive_dragger>().current_slot = new_slot;
        if(archive_swap_count == 21){
            StartCoroutine("GameTimer");
        }
        archive_swap_count--;
        ArchiveManager.GetComponent<archiveManager>().swapCount.text = archive_swap_count.ToString();
        if(archive_swap_count == 0 && archive_matched!=19){

            StopCoroutine("GameTimer");
            ShowGameTime();
            ArchiveManager.GetComponent<archiveManager>().Moves.text = (21-archive_swap_count).ToString() + "/21";

            OnArchiveFinish(false,archive_swap_count,ArchiveManager.GetComponent<archiveManager>().LastArchiveOpened);
            ArchiveManager.GetComponent<archiveManager>().GameoverScreen.SetActive(true);
            ArchiveManager.GetComponent<archiveManager>().ResultScreen.SetActive(true);
            foreach(GameObject slot in archiveManager.ArchiveSlotList){
                slot.GetComponent<archive_dropper>().current_dice.GetComponent<BoxCollider2D>().enabled = false;
                slot.GetComponent<BoxCollider2D>().enabled = false;
            }
        }        
    }


    void ShowGameTime(){
        if(Total_sec<60){
            ArchiveManager.GetComponent<archiveManager>().Timer.text = Total_sec.ToString() + "s";
        }
        else if(Total_sec<3600){
            ArchiveManager.GetComponent<archiveManager>().Timer.text = (Total_sec/60).ToString() + "m " + (Total_sec%60).ToString() + "s";
        }
        Shared_Total_sec = Total_sec;
    }

    IEnumerator GameTimer(){
        while(!(archive_swap_count==0 && archive_matched==21)){
            yield return new WaitForSeconds(1f);
            Total_sec += 1;
        }
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

    void MakeWinShare(){
        if(star_count == 0){
            ArchiveManager.GetComponent<archiveManager>().ResultMsg.text = "Phew! That was close";
            ArchiveManager.GetComponent<archiveManager>().ShareMsg = "🟩🟩🟩🟩🟩\n🟩⬜🟩⬜🟩\n🟩🟩🟩🟩🟩\n🟩⬜🟩⬜🟩\n🟩🟩🟩🟩🟩";
        }
        else if(star_count ==1){
            ArchiveManager.GetComponent<archiveManager>().ResultMsg.text = "You solved it!";
            ArchiveManager.GetComponent<archiveManager>().ShareMsg = "🟩🟩🟩🟩🟩\n🟩⬜🟩⬜🟩\n🟩🟩⭐🟩🟩\n🟩⬜🟩⬜🟩\n🟩🟩🟩🟩🟩";
        }
        else if(star_count == 2){
            ArchiveManager.GetComponent<archiveManager>().ResultMsg.text = "Good Job!";
            ArchiveManager.GetComponent<archiveManager>().ShareMsg = "🟩🟩🟩🟩🟩\n🟩⭐🟩⬜🟩\n🟩🟩🟩🟩🟩\n🟩⬜🟩⭐🟩\n🟩🟩🟩🟩🟩";
        }
        else if(star_count == 3){
            ArchiveManager.GetComponent<archiveManager>().ResultMsg.text = "Well done";
            ArchiveManager.GetComponent<archiveManager>().ShareMsg = "🟩🟩🟩🟩🟩\n🟩⭐🟩⬜🟩\n🟩🟩⭐🟩🟩\n🟩⬜🟩⭐🟩\n🟩🟩🟩🟩🟩";
        }
        else if(star_count == 4){
            ArchiveManager.GetComponent<archiveManager>().ResultMsg.text = "You are brilliant";
            ArchiveManager.GetComponent<archiveManager>().ShareMsg = "🟩🟩🟩🟩🟩\n🟩⭐🟩⭐🟩\n🟩🟩🟩🟩🟩\n🟩⭐🟩⭐🟩\n🟩🟩🟩🟩🟩";
        }
        else{
            ArchiveManager.GetComponent<archiveManager>().ResultMsg.text = "Aren't you a genius!";
            ArchiveManager.GetComponent<archiveManager>().ShareMsg = "🟩🟩🟩🟩🟩\n🟩⭐🟩⭐🟩\n🟩🟩⭐🟩🟩\n🟩⭐🟩⭐🟩\n🟩🟩🟩🟩🟩";
        }
    }

}
