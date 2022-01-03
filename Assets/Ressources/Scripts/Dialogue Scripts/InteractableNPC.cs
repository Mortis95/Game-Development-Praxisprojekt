using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractableNPC : MonoBehaviour
{

    public float radius =3f;
    public string NPCName;
    public bool isInRange;
    public bool busyWithInteraction;
    public KeyCode interactionKey;
    public UnityEvent interactAction;
    public Dialogue[] dialogues;
    private Dialogue activeDialogue;
    public int activeDialogueIndex;


    void Start(){
        activeDialogue = dialogues[0];
    }
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
        Debug.Log(activeDialogue.textBoxen.Length);
        DialogueManager.getInstance().StartDialogue(activeDialogue);
    }

    public void setActiveDialogue(int index){
        if(index < dialogues.Length){
            activeDialogue = dialogues[index];
            activeDialogueIndex = index;
            Debug.Log("Active Dialogue of " + NPCName + "has been set to Element " + index);
        } else{
            Debug.Log("Can't set active Dialogue of " + NPCName + " to Element " + index + "!\nPlease Check the correct index and amount of Dialogues on this NPC.");
        }
    }
}
