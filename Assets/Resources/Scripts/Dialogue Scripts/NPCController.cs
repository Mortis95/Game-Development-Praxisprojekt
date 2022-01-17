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
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }


    public void playSound(string soundname){
        AudioManager.getInstance().Play(soundname);
    }
    public void playMusic(string musicname){
        //TODO
    }

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

}
