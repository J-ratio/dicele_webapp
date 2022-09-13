using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class button_script : MonoBehaviour , IPointerDownHandler, IPointerUpHandler
{   
    
    public void OnPointerDown(PointerEventData eventData){
        if(GetComponent<Button>().enabled){
        GetComponent<RectTransform>().sizeDelta = new Vector2(193, 119);
        }
    }

    public void OnPointerUp(PointerEventData eventData){
        if(GetComponent<Button>().enabled){
        GetComponent<RectTransform>().sizeDelta = new Vector2(207, 127);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
