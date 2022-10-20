using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;


public class link_text : MonoBehaviour
{

    private bool tag0 = false;
    [DllImport("__Internal")]
    public static extern void Gform_Redirect();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseOver(){
        transform.localScale = new Vector3(1.1f,1.1f,1);
    }

    void OnMouseExit(){
        transform.localScale = new Vector3(1f,1f,1);
    }

    void OnMouseDown(){
        if(!tag0){
            GetComponent<TextMeshProUGUI>().color = new Color(0,0,0);
            Gform_Redirect();
            tag0 = true;
        }
        
    }

    void OnMouseUp(){
        if(tag0){
            GetComponent<TextMeshProUGUI>().color = new Color(0,23/255f,1);
            tag0 = false;
        }
    }
<<<<<<< Updated upstream
}
=======
}
>>>>>>> Stashed changes
