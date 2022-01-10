using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{

    public float radius =3f;

    public bool isInRange;
    public bool busyWithInteraction;
    public KeyCode interactionKey;
    public UnityEvent interactAction;
    public Dialogue dialogue;


    void OnDrawGizmosSelected()
    {
        Gizmos.color =Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    // Update is called once per frame
    void Update()
    {
        if(isInRange)
        {
            if(Input.GetKeyDown(interactionKey) && !busyWithInteraction)
            {
                Debug.Log("Key gedrueckt");
                interactAction.Invoke();
            }

        }
    }

    public void bspMethode()
    {
        Debug.Log("das ist ein Beispiel");   
      
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
            busyWithInteraction = false;
            //Debug.Log("nicht mehr in range ");
        }
    }

    //Sollte als Unity Event invoked werdan damit Dialogue beginnt
    public void TriggerDialogue(){
        busyWithInteraction = true;
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
