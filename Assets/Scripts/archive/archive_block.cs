using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class archive_block : MonoBehaviour
{
    public int archive_number;
    public TextMeshProUGUI DayText;
    public TextMeshProUGUI DateText;
    public TextMeshProUGUI Unattempted;
    [SerializeField]
    GameObject[] Stars;
    [SerializeField]
    GameObject HeartBreak;
    public GameObject archiveManager;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Play_Archive);
    }

    void Play_Archive(){
        archiveManager.GetComponent<archiveManager>().StartArchiveGame(archive_number);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
