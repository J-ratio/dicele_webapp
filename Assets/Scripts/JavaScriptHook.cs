using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class JavaScriptHook : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void storeMovesCount(int movesCount, int minutes, int seconds);

    private bool x = true;   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(slot_script.matched == 25 && x)
        {   
            x = false;
            storeMovesCount(slot_dice_script.moves, timer_script.minutes, timer_script.seconds);
        }
    }
}
