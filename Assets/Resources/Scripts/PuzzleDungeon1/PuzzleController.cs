using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public bool p1 = false;
    public bool p2 = false;
    public bool p3 = false;
    public bool p4 = false;

    public GameObject blockade;
    Progress script;

    public int id;        
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        script = player.GetComponent<Progress>();
    }
    void Update()
    {
        if(id == 1)
        {
            if(p1 & p2 & p3)
            {
                Debug.Log("Puzzlegeschafft");
                //SzenenÜbergang instantiieren
                
                blockade.SetActive(false);
                //Animation
                //Sound     
                script.dungeon01Puzzle01=true;           
                Destroy(this);
                

            }
        }
        if(id == 2)
        {
            if(p1 & p2 & p3 & p4)
            {
                Debug.Log("Puzzlegeschafft");
                //SzenenÜbergang instantiieren
           
                blockade.SetActive(false);
                //Animation
                //Sound
                //truhe öffnen   
                script.dungeon01Puzzle02=true;             
                Destroy(this);
            }
        }
        if(id == 3)
        {
            if(p1 & p2 & p3 & p4)
            {
                Debug.Log("Puzzlegeschafft");
               
                blockade.SetActive(false);
                //Animation
                //Sound
                //Truhe öffnen 
                script.dungeon01Puzzle03=true;               
                Destroy(this);
                //ProgressScript
                

            }
        }
        

        
    }
}
