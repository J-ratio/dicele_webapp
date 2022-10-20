using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class dice_select_script : MonoBehaviour , IPointerDownHandler, IPointerUpHandler
{

    public void OnPointerDown(PointerEventData eventData){
        if(this.enabled){
        this.transform.localScale = new Vector3(0.8f,0.8f,1f);
        }
    }

    public void OnPointerUp(PointerEventData eventData){
        if(this.enabled){
        this.transform.localScale = new Vector3(0.9f,0.9f,1f);
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
