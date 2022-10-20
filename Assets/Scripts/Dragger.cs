using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dragger : MonoBehaviour
{   [SerializeField]
    new bool tag = false;
    Vector3 drag_offset;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject HowToPlay;
    [SerializeField]
    private Button Help;

    public int dice_number;

    Vector3 delta = new Vector3 (0.1f,0.1f,0);
    Vector3 delta2 = new Vector3 (0,0,-0.1f);

    public GameObject current_slot;



    void Awake(){
        cam = Camera.main;
        Help.onClick.AddListener(HelpScreen);
    }

    void HelpScreen(){
        if(GetComponent<BoxCollider2D>().enabled){
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else{
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    void OnMouseDown(){
        if(!tag){
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity);
        foreach (var hit in hits) {
            if (hit.collider.name == gameObject.name) {
                tag = true;
                transform.localScale +=delta;
                GetComponent<SpriteRenderer>().sortingOrder ++;
                break;
            }
        }
        drag_offset = transform.position - GetMousePos();
        }
        
    }

    void OnMouseDrag(){
        if(tag){
        transform.position = Vector3.MoveTowards(transform.position,GetMousePos() + drag_offset,speed*Time.deltaTime);
        }
    }

    void OnMouseUp(){        
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity);
        foreach (var hit in hits) {
            if (hit.collider.CompareTag("slot") && tag && hit.collider.gameObject != current_slot) {
                StartCoroutine(Routine2(hit.collider.gameObject));
                transform.localScale -=delta;
                GetComponent<SpriteRenderer>().sortingOrder --;
                hit.collider.gameObject.GetComponent<Dropper>().MoveToSlot(current_slot);
                current_slot = hit.collider.gameObject;
                current_slot.GetComponent<Dropper>().current_dice = this.gameObject;
                tag = false;
                RandomSpawnGenerator.update_color();
                if(Dropper.swap_count == 0 && Dropper.matched!=19){
                    current_slot.GetComponent<Dropper>().GameManager.GetComponent<RandomSpawnGenerator>().MakeLoseShareMsg();                }
                break;
            }   
        }
        if(tag){
            StartCoroutine(Routine1());
            transform.localScale -=delta;
            GetComponent<SpriteRenderer>().sortingOrder --;
            tag = false;
        }
    }


    public IEnumerator Routine2(GameObject new_slot)
    {
        while(Vector3.Distance(transform.position,new_slot.transform.position)>0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position,new_slot.gameObject.transform.position,10*Time.deltaTime);
            yield return new WaitForSeconds(0.01f*Time.deltaTime);
        }
        transform.position += delta2;
        yield break;
         
    }

    public IEnumerator Routine1()
    {
        while(Vector3.Distance(transform.position,current_slot.transform.position)>0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position,current_slot.transform.position,30*Time.deltaTime);
            yield return new WaitForSeconds(0.01f*Time.deltaTime);
        }
        transform.position += delta2;
        yield break;
         
    }

    Vector3 GetMousePos(){
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
    

}