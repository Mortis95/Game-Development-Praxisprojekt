using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCCutsceneController : MonoBehaviour
{
    private Vector2 goalPosition;
    public Rigidbody2D rb;
    public float moveSpeed;
    public Vector2 movement;
    private Animator animator;
    private AnimationState currentState;
    private void Awake(){
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        animator.speed = 0;
        currentState = AnimationState.NPCWalkDown;
        goalPosition = transform.position;
    }

    void Update(){
        movement = goalPosition - (Vector2)transform.position;
        movement.Normalize();

        if(Vector2.Distance(transform.position, goalPosition) < 1f){
            movement = Vector2.zero;
            goalPosition = transform.position;
            setIdleAnimation();
        } else {
            setMovementAnimation();
        }
        
        transform.position = (Vector2) transform.position + movement * moveSpeed * Time.unscaledDeltaTime;  //Unscaled, because during Cutscenes/Pauses we will set TimeScale to 0. We want our NPCs to be able to move during cutscenes.
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
        Debug.Log("Meine alte GoalPos:" + goalPosition);
        Debug.Log("Ich addiere den Vektor:" + new Vector2(x,0f));
        goalPosition = goalPosition + new Vector2(x,0f);
        Debug.Log("Meine neue GoalPos:" + goalPosition);
    }

    public void goVerticalUnits(float y){
        goalPosition = goalPosition + new Vector2(0f,y);

    }
    #endregion

    #region SetAnimations
    private enum AnimationState{
        NPCWalkUp,
        NPCWalkDown,
        NPCWalkLeft,
        NPCWalkRight
    }
    void setMovementAnimation(){
        animator.speed = 1;
        if(Vector2.Angle(Vector2.up, movement) <= 45){
            changeAnimationState(AnimationState.NPCWalkUp);
        } else if(Vector2.Angle(Vector2.down, movement) <= 45 ){
            changeAnimationState(AnimationState.NPCWalkDown);
        } else if(Vector2.Angle(Vector2.left, movement) < 45 ){
            changeAnimationState(AnimationState.NPCWalkLeft);
        } else if(Vector2.Angle(Vector2.right, movement) < 45 ){
            changeAnimationState(AnimationState.NPCWalkRight);
        }
    }

    void setIdleAnimation(){
        animator.speed = 0;
    }

    private void changeAnimationState(AnimationState state){
        //If current animation is already playing, do not start it again.
        //Otherwise an animation would start every frame and never properly play out.
        if(state.Equals(currentState)){
            return;
        }
        animator.Play(state.ToString());
        currentState = state;
    }
    
    #endregion
    //Calling this method will disable this script.
    public void disableCutsceneController(){
        gameObject.GetComponent<NPCCutsceneController>().enabled = false;
    }

}
