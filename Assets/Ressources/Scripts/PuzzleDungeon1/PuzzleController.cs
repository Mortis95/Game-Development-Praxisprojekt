using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    public bool p1 = false;
    public bool p2 = false;
    public bool p3 = false;
    public bool p4 = false;

    public int id=0;    
    
    void Update()
    {
        if(id == 1)
        {
            if(p1 & p2 & p3)
            {
                Debug.Log("Puzzlegeschafft");
                //SzenenÜbergang instantiieren
                //Animation
                //Sound
                Destroy(this);

            }
        }
        if(id == 2)
        {
            if(p1 & p2 & p3 & p4)
            {
                Debug.Log("Puzzlegeschafft");
                //SzenenÜbergang instantiieren
                //Animation
                //Sound
                //truhe öffnen
                Destroy(this);
            }
        }
        if(id == 3)
        {
            if(p1 & p2 & p3 & p4)
            {
                Debug.Log("Puzzlegeschafft");
                //SzenenÜbergang instantiieren
                //Animation
                //Sound
                //Truhe öffnen
                Destroy(this);

            }
        }
        

        
    }
}
