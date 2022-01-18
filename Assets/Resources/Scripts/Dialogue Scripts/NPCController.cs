using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCController : MonoBehaviour
{
    private Vector2 goalPosition;
    private Vector2[] walkPoints;   //Noch nicht implementiert, später vielleicht.
    public Rigidbody2D rb;
    public float moveSpeed;
    public Vector2 movement;
    private void Awake(){
        rb = gameObject.GetComponent<Rigidbody2D>();
        goalPosition = transform.position;
    }


    void Update(){
        movement = goalPosition - new Vector2(transform.position.x,transform.position.y);
        if(movement.magnitude > 1){
            movement.Normalize();
        } else {
            movement = Vector2.zero;
            goalPosition = rb.position;
        }
        
    }

    void FixedUpdate(){
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedUnscaledDeltaTime);  //Unscaled, because during Cutscenes/Pauses we will set TimeScale to 0. We want our NPCs to be able to move during cutscenes.
    }

    #region NPCsCanControlAudioThroughUnityEvents
    public void playSound(string soundname){
        AudioManager.getInstance().PlaySound(soundname);
    }
    public void playMusic(string musicname){
        AudioManager.getInstance().PlayMusic(musicname);
    }
    public void stopMusic(){
        AudioManager.getInstance().StopMusic();
    }
    #endregion
    
    #region NPCsCanMoveThroughUnityEvents
    public void setHorizontalTargetPos(float x){
        goalPosition = new Vector2(x,goalPosition.y);
    }

    public void setVerticalTargetPos(float y){
        goalPosition = new Vector2(goalPosition.x, y);
    }

    public void goHorizontalUnits(float x){
        goalPosition = goalPosition + new Vector2(x,0f);
    }

    public void goVerticalUnits(float y){
        goalPosition = goalPosition + new Vector2(0f,y);

    }
    #endregion

}
