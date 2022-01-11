using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Useless Class, wird aktuell nicht genutzt, da Gespräche aus Interactable gerufen werden können
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue(){
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
