using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int star_count = 5;

    void OnEnable(){
        archiveManager.ArchiveSlotList.Remove(this.gameObject);
        StartCoroutine("delay");
    }

    IEnumerator delay(){
        yield return new WaitForSeconds(0.1f*Time.deltaTime);
        GetComponent<BoxCollider2D>().enabled = true;
        current_dice.GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<archive_dropper>().enabled = true;
        Archive_Start();
    }

    void Archive_Start()
    {
        archive_swap_count = 21;
        archive_matched = 0;
        ArchiveManager.GetComponent<archiveManager>().swapCount.text = archive_swap_count.ToString();
        green_number = ArchiveManager.GetComponent<archiveManager>().Get_Green_number(slot_pos[0],slot_pos[1]);
        archiveManager.ArchiveSlotList.Add(this.gameObject);
        current_dice.GetComponent<SpriteRenderer>().sprite = ArchiveManager.GetComponent<archiveManager>().dice_images[ArchiveManager.GetComponent<archiveManager>().spawn_Arr[slot_pos[0],slot_pos[1]]];
        current_dice.GetComponent<archive_dragger>().dice_number = ArchiveManager.GetComponent<archiveManager>().spawn_Arr[slot_pos[0],slot_pos[1]];
        Check_color();       
    }

    public void Check_color(){
        
        if(current_dice.GetComponent<archive_dragger>().dice_number == green_number){
            current_dice.GetComponent<SpriteRenderer>().color = new Color(106/255f,192/255f,81/255f);
            ArchiveManager.GetComponent<archiveManager>().solutionArr[slot_pos[0],slot_pos[1]] = 6;
            GetComponent<BoxCollider2D>().enabled = false;
            current_dice.GetComponent<BoxCollider2D>().enabled = false;
            archive_matched++;
            if(archive_matched == 21){
                if(archive_swap_count<6){
                    star_count = archive_swap_count;
                }
                string finalArr1 = "[";
                for(var i=0;i<5;i++){
                    finalArr1 = finalArr1 + "[";
                        for(var j=0;j<5;j++){
                        finalArr1 = finalArr1 + ArchiveManager.GetComponent<archiveManager>().spawn_Arr[i,j].ToString();
                        if(j!=4){
                            finalArr1 = finalArr1 + ",";
                        }
                        }
                    finalArr1 = finalArr1 + "]";
                    if(i!=4){
                        finalArr1 = finalArr1 + ",";
                    }
                }
                finalArr1 = finalArr1 + "]";

                ShowStars();
                MakeWinShare();
                /*if(!RandomSpawnGenerator.isSolved){
                    OnFinish(true,finalArr1,swap_count);
                }*/
                ArchiveManager.GetComponent<archiveManager>().WinningScreen.SetActive(true);
                ArchiveManager.GetComponent<archiveManager>().ResultScreen.SetActive(true);
            }
            StartCoroutine("delay3");
        }
        else{
            StartCoroutine("delay2");
        }
        
    }

    void ShowStars(){
        for(var i=0; i<star_count; i++){
            Stars[i].GetComponent<Image>().sprite = Star;
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
        archive_swap_count--;
        ArchiveManager.GetComponent<archiveManager>().swapCount.text = archive_swap_count.ToString();
        if(archive_swap_count == 0 && archive_matched!=19){
            string final_Arr = "[";
            for(var i=0;i<5;i++){
                final_Arr = final_Arr + "[";
                for(var j=0;j<5;j++){
                    final_Arr = final_Arr + (ArchiveManager.GetComponent<archiveManager>().spawn_Arr[i,j]).ToString();
                    if(j!=4){
                        final_Arr = final_Arr + ",";
                    }
                }
                final_Arr = final_Arr + "]";
                if(i!=4){
                        final_Arr = final_Arr + ",";
                    }
            }
            final_Arr = final_Arr + "]";
            /*if(!RandomSpawnGenerator.isSolved){
                OnFinish(false,final_Arr,swap_count);
            }
            RandomSpawnGenerator.isSolved = true;*/
            ArchiveManager.GetComponent<archiveManager>().GameoverScreen.SetActive(true);
            ArchiveManager.GetComponent<archiveManager>().ResultScreen.SetActive(true);
            foreach(GameObject slot in archiveManager.ArchiveSlotList){
                slot.GetComponent<archive_dropper>().current_dice.GetComponent<BoxCollider2D>().enabled = false;
                slot.GetComponent<BoxCollider2D>().enabled = false;
            }
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
            ArchiveManager.GetComponent<archiveManager>().ShareMsg = "🟩🟩🟩🟩🟩\n🟩⬜🟩⬜🟩\n🟩🟩🟩🟩🟩\n🟩⬜🟩⬜🟩\n🟩🟩🟩🟩🟩";
        }
        else if(star_count ==1){
            ArchiveManager.GetComponent<archiveManager>().ShareMsg = "🟩🟩🟩🟩🟩\n🟩⬜🟩⬜🟩\n🟩🟩⭐🟩🟩\n🟩⬜🟩⬜🟩\n🟩🟩🟩🟩🟩";
        }
        else if(star_count == 2){
            ArchiveManager.GetComponent<archiveManager>().ShareMsg = "🟩🟩🟩🟩🟩\n🟩⭐🟩⬜🟩\n🟩🟩🟩🟩🟩\n🟩⬜🟩⭐🟩\n🟩🟩🟩🟩🟩";
        }
        else if(star_count == 3){
            ArchiveManager.GetComponent<archiveManager>().ShareMsg = "🟩🟩🟩🟩🟩\n🟩⭐🟩⬜🟩\n🟩🟩⭐🟩🟩\n🟩⬜🟩⭐🟩\n🟩🟩🟩🟩🟩";
        }
        else if(star_count == 4){
            ArchiveManager.GetComponent<archiveManager>().ShareMsg = "🟩🟩🟩🟩🟩\n🟩⭐🟩⭐🟩\n🟩🟩🟩🟩🟩\n🟩⭐🟩⭐🟩\n🟩🟩🟩🟩🟩";
        }
        else{
            ArchiveManager.GetComponent<archiveManager>().ShareMsg = "🟩🟩🟩🟩🟩\n🟩⭐🟩⭐🟩\n🟩🟩⭐🟩🟩\n🟩⭐🟩⭐🟩\n🟩🟩🟩🟩🟩";
        }
    }

}
