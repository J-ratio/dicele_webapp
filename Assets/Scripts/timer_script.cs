using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class timer_script : MonoBehaviour
{
    //Gameobjects
    public Button roll_btn;

    //Gamevariables
    public static int playtime;
    public static int seconds;
    public static int minutes;
    private bool time_tag = true;

    void Start()
    {   
        roll_btn.onClick.AddListener(Dothis);

        playtime = 0;
        seconds = 0;
        minutes = 0;
    }

    void Dothis(){
        if(time_tag){
        StartCoroutine("Timer");
        time_tag = false;
        }
    }

    private IEnumerator Timer()
    {
        while(slot_script.matched < 25)
        {
            yield return new WaitForSeconds(1);
            playtime += 1;
            seconds = (playtime % 60);
            minutes = (playtime / 60) % 60;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
