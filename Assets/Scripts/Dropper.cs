using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class Dropper : MonoBehaviour
{

    public GameObject current_dice;
    [SerializeField]
    public GameObject GameManager;
    [SerializeField]
    private GameObject[] Stars;
    [SerializeField]
    private Sprite Star;

    public int[] slot_pos = new int[2];
    Vector3 delta2 = new Vector3 (0,0,-0.1f);
    public List<int> yellow_arr = new List<int>();
    public int green_number;
    public static int swap_count;
    public static int matched;
    public static int star_count = 5;
    public static int Total_sec;
    

    [DllImport("__Internal")]
    public static extern void OnFinish(bool isSolved, string finalArr,int swap_count, int play_time);
    [DllImport("__Internal")]
    public static extern void GameEnd(int result, int gameTime,int swap_count, int match_count);


    void Awake(){
        RandomSpawnGenerator.slotList.Remove(this.gameObject);
        StartCoroutine("delay");
    }

    IEnumerator delay(){
        yield return new WaitForSeconds(0.1f*Time.deltaTime);
        if(!RandomSpawnGenerator.isSolved){
        swap_count = 21;
        matched = 0;
        Total_sec = 0;
        star_count = 5;
        GetComponent<BoxCollider2D>().enabled = true;
        current_dice.GetComponent<BoxCollider2D>().enabled = true;
        }
        GameManager.GetComponent<RandomSpawnGenerator>().swapCount.text = swap_count.ToString();
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
            current_dice.GetComponent<SpriteRenderer>().sprite = GameManager.GetComponent<RandomSpawnGenerator>().GreenDiceImages[current_dice.GetComponent<Dragger>().dice_number];//new Color(106/255f,192/255f,81/255f);
            current_dice.GetComponent<SpriteRenderer>().color = Color.white;
            //current_dice.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GameManager.GetComponent<RandomSpawnGenerator>().GreenDiceImages[current_dice.GetComponent<Dragger>().dice_number];
            GameManager.GetComponent<RandomSpawnGenerator>().solutionArr[slot_pos[0],slot_pos[1]] = 6;
            GetComponent<BoxCollider2D>().enabled = false;
            current_dice.GetComponent<BoxCollider2D>().enabled = false;
            matched++;
            if(matched == 21){
                StopCoroutine("GameTimer");
                ShowGameTime();
                GameManager.GetComponent<RandomSpawnGenerator>().Moves.text = (21-swap_count).ToString() + "/21";
                if(swap_count<6){
                    star_count = swap_count;
                }
                string finalArr1 = "[";
                for(var i=0;i<5;i++){
                    finalArr1 = finalArr1 + "[";
                        for(var j=0;j<5;j++){
                        finalArr1 = finalArr1 + GameManager.GetComponent<RandomSpawnGenerator>().spawn_Arr[i,j].ToString();
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

                GameManager.GetComponent<RandomSpawnGenerator>().PlayWinAnim();
                MakeWinShare();
                if(!RandomSpawnGenerator.isSolved){
                    GameEnd(1,Total_sec,swap_count,matched);
                    OnFinish(true,finalArr1,swap_count,Total_sec);
                    archive_dropper.OnArchiveFinish(true,swap_count,GameManager.GetComponent<RandomSpawnGenerator>().Day-1);
                    Invoke("ShowWinScreen",4.0f);
                }
                else{
                    ShowWinScreen();
                }
                RandomSpawnGenerator.isSolved = true;
            }
            StartCoroutine("delay3");
        }
        else{
            StartCoroutine("delay2");
        }
        
    }

    void ShowWinScreen(){
        GameManager.GetComponent<RandomSpawnGenerator>().WinningScreen.SetActive(true);
        GameManager.GetComponent<RandomSpawnGenerator>().ResultScreen.SetActive(true);
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
        GameManager.GetComponent<RandomSpawnGenerator>().ResultsMsg.SetActive(true);

    }

    IEnumerator IncreaseStarVisibility(GameObject star){
        for(var i = 0; i<51;i++){
            yield return new WaitForSeconds(0.001f);
            star.GetComponent<Image>().color = new Color(1,1,1,i*0.02f);
        }
    }

    IEnumerator delay3(){
        yield return new WaitForSeconds(1f*Time.deltaTime);
        RandomSpawnGenerator.slotList.Remove(this.gameObject);
    }

    IEnumerator delay2(){
        yield return new WaitForSeconds(0.5f*Time.deltaTime);
        yellow_arr = GameManager.GetComponent<RandomSpawnGenerator>().Get_Yellow_numbers(slot_pos[0],slot_pos[1]);
            if(yellow_arr.Contains(current_dice.GetComponent<Dragger>().dice_number)){
            current_dice.GetComponent<SpriteRenderer>().color = new Color(243/255f,193/255f,58/255f);;
            }
            else{
            current_dice.GetComponent<SpriteRenderer>().color = Color.white;
            }
        yield break;
    }

    public void MoveToSlot(GameObject new_slot){
        StartCoroutine(Routine1(new_slot,current_dice));
        GameManager.GetComponent<RandomSpawnGenerator>().spawn_Arr[new_slot.GetComponent<Dropper>().slot_pos[0],new_slot.GetComponent<Dropper>().slot_pos[1]] = current_dice.GetComponent<Dragger>().dice_number;
        GameManager.GetComponent<RandomSpawnGenerator>().spawn_Arr[slot_pos[0],slot_pos[1]] = new_slot.GetComponent<Dropper>().current_dice.GetComponent<Dragger>().dice_number;
        new_slot.GetComponent<Dropper>().current_dice = current_dice;
        current_dice.GetComponent<Dragger>().current_slot = new_slot;
        if(swap_count == 21  && !RandomSpawnGenerator.isSolved){
            StartCoroutine("GameTimer");
            GameManager.GetComponent<RandomSpawnGenerator>().StopTime();
        }
        swap_count--;
        GameManager.GetComponent<RandomSpawnGenerator>().swapCount.text = swap_count.ToString();
        if(swap_count == 0 && matched!=19){
            StopCoroutine("GameTimer");
            ShowGameTime();
            GameManager.GetComponent<RandomSpawnGenerator>().Moves.text =  "21/21";
            string final_Arr = "[";
            for(var i=0;i<5;i++){
                final_Arr = final_Arr + "[";
                for(var j=0;j<5;j++){
                    final_Arr = final_Arr + (GameManager.GetComponent<RandomSpawnGenerator>().spawn_Arr[i,j]).ToString();
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
            if(!RandomSpawnGenerator.isSolved){
                GameEnd(0,Total_sec,swap_count,matched);
                OnFinish(false,final_Arr,swap_count,Total_sec);
                archive_dropper.OnArchiveFinish(false,swap_count,GameManager.GetComponent<RandomSpawnGenerator>().Day-1);
            }
            RandomSpawnGenerator.isSolved = true;
            GameManager.GetComponent<RandomSpawnGenerator>().GameoverScreen.SetActive(true);
            GameManager.GetComponent<RandomSpawnGenerator>().ResultScreen.SetActive(true);
            foreach(GameObject slot in RandomSpawnGenerator.slotList){
                slot.GetComponent<Dropper>().current_dice.GetComponent<BoxCollider2D>().enabled = false;
                slot.GetComponent<BoxCollider2D>().enabled = false;
            }
        }         
    }

    void ShowGameTime(){
        if(Total_sec<60){
            GameManager.GetComponent<RandomSpawnGenerator>().Timer.text = Total_sec.ToString() + "s";
        }
        else if(Total_sec<3600){
            GameManager.GetComponent<RandomSpawnGenerator>().Timer.text = (Total_sec/60).ToString() + "m " + (Total_sec%60).ToString() + "s";
        }
        
    }

    IEnumerator GameTimer(){
        while(!(swap_count==0 && matched==21)){
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
            GameManager.GetComponent<RandomSpawnGenerator>().ResultMsg.text = "Phew! That was close";
            GameManager.GetComponent<RandomSpawnGenerator>().ShareMsg = "🟩🟩🟩🟩🟩\n🟩⬜🟩⬜🟩\n🟩🟩🟩🟩🟩\n🟩⬜🟩⬜🟩\n🟩🟩🟩🟩🟩";
        }
        else if(star_count ==1){
            GameManager.GetComponent<RandomSpawnGenerator>().ResultMsg.text = "You solved it!";
            GameManager.GetComponent<RandomSpawnGenerator>().ShareMsg = "🟩🟩🟩🟩🟩\n🟩⬜🟩⬜🟩\n🟩🟩⭐🟩🟩\n🟩⬜🟩⬜🟩\n🟩🟩🟩🟩🟩";
        }
        else if(star_count == 2){
            GameManager.GetComponent<RandomSpawnGenerator>().ResultMsg.text = "Good Job!";
            GameManager.GetComponent<RandomSpawnGenerator>().ShareMsg = "🟩🟩🟩🟩🟩\n🟩⭐🟩⬜🟩\n🟩🟩🟩🟩🟩\n🟩⬜🟩⭐🟩\n🟩🟩🟩🟩🟩";
        }
        else if(star_count == 3){
            GameManager.GetComponent<RandomSpawnGenerator>().ResultMsg.text = "Well done";
            GameManager.GetComponent<RandomSpawnGenerator>().ShareMsg = "🟩🟩🟩🟩🟩\n🟩⭐🟩⬜🟩\n🟩🟩⭐🟩🟩\n🟩⬜🟩⭐🟩\n🟩🟩🟩🟩🟩";
        }
        else if(star_count == 4){
            GameManager.GetComponent<RandomSpawnGenerator>().ShareMsg = "🟩🟩🟩🟩🟩\n🟩⭐🟩⭐🟩\n🟩🟩🟩🟩🟩\n🟩⭐🟩⭐🟩\n🟩🟩🟩🟩🟩";
        }
        else{
            GameManager.GetComponent<RandomSpawnGenerator>().ResultMsg.text = "Aren't you a genius!";
            GameManager.GetComponent<RandomSpawnGenerator>().ShareMsg = "🟩🟩🟩🟩🟩\n🟩⭐🟩⭐🟩\n🟩🟩⭐🟩🟩\n🟩⭐🟩⭐🟩\n🟩🟩🟩🟩🟩";
        }
    }

}