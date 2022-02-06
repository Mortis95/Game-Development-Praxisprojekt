using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressChecker : MonoBehaviour
{
    public Typ modus;
    
        // Start is called before the first frame update
    void Start()
    {
        if(modus == Typ.check1)
        {
            if(Progress.getInstance().schluesselWeisheit == true)
            {
                Destroy(gameObject);
            }
            
        }
        if(modus == ProgressChecker.Typ.check2)
        {
            if(Progress.getInstance().schluesselMacht  == true)
            {
                GameObject tombstone = Resources.Load("Prefabs/Tombstone") as GameObject;
                GameObject test = Instantiate(tombstone);
                Destroy(gameObject);   
               
                
            }
            
        }
        if(modus == Typ.check3)
        {
            if(Progress.getInstance().schluesselFrieden  == true)
            {
                Destroy(gameObject);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum Typ
{
    check1,
    check2,
    check3    
}
}
