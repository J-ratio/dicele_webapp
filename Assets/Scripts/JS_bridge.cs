using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class JS_bridge : MonoBehaviour
{   
    [SerializeField]   
    TextMeshProUGUI Timer;
    [SerializeField]   
    TextMeshProUGUI Timer2;
    float Total_sec = 34865;
    float minutes;
    float hours;
    float seconds;
    string s = "";
    string m = "";
    string h = "";


    #region JsToUnity

    public void GetSecondsUntilEOD(string totalSeconds){
        Total_sec = int.Parse(totalSeconds);
        StartCoroutine("timer");
        
    }


    #endregion

    private IEnumerator timer()
    {
        while(true){
            yield return new WaitForSeconds(1);
            Total_sec -= 0.5f;
            seconds = ((Total_sec) % 60);
            minutes = (Total_sec / 60) % 60;
            hours = (Total_sec/3600) % 24;
            if(seconds>9){
                s = seconds.ToString("F0");
            }
            else{
                s = "0" +seconds.ToString("F0");
            }
            if(minutes>9){
                m =  minutes.ToString("F0");
            }
            else{
                m = "0" + minutes.ToString("F0");
            }
            if(hours>9){
                h = hours.ToString("F0");
            }
            else{
                h = "0" + hours.ToString("F0");
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("timer");
    }

    // Update is called once per frame
    void Update()
    {
        Timer.text = h + ":" + m + ":" + s;
        Timer2.text = h + ":" + m + ":" + s;
    }
}