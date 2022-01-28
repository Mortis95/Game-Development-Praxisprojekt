using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericTrigger : MonoBehaviour{
    
    [Tooltip("The Unity-Event this Trigger will invoke, when the Player touches it.")]
    public UnityEvent triggerEvent;
    [Tooltip("How often is the trigger allowed to be executed?")]
    public int howOftenToTrigger;

    void OnTriggerEnter2D(Collider2D col){
        GameObject other = col.gameObject;
        if(other != null && other.tag == "Player"){
            howOftenToTrigger--;
            triggerEvent.Invoke();
        }
        if(howOftenToTrigger <= 0){
            Destroy(gameObject);
        }
    }
}
