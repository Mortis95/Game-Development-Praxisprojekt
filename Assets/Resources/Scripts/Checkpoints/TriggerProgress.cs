using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerProgress : MonoBehaviour
{
    public KeyCode interactionKey;    
    public int id;
    public bool isInRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isInRange)
        {
            if(Input.GetKeyDown(interactionKey) )
            {
                if(id == 1)
                {
                Progress.getInstance().schluesselWeisheit=true;
                }
                if(id == 2)
                {
                    Progress.getInstance().schluesselMacht=true;
                }
                if(id == 3)
                {
                    Progress.getInstance().schluesselMacht=true;
                }else{
                    Debug.Log("Da ist deine id in den fritten");
                }        

            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isInRange= true;
            //Debug.Log("in range");

        }
    }

     private void OnTriggerExit2D(Collider2D collision)
     {
        if(collision.gameObject.CompareTag("Player"))
        {
            isInRange= false;           
        }
    }
}
