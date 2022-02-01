using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingNPC : MonoBehaviour{

    #region PublicVariables
    [Tooltip("While the Player is in this range, the NPC will try to look at him. Otherwise an Idle-Animation may play")]
    public float visionRange;
    #endregion
    
    #region PrivateVariables
    private Animator animator;
    private AnimationState currentState;
    private Transform toLookAt;
    #endregion
    
    #region Start
    void Start(){
        animator = gameObject.GetComponent<Animator>();
        currentState = AnimationState.NPCLookDown;
        toLookAt = Player.getInstance().transform;
    }
    #endregion

    #region Update

    void Update(){
        if(Vector2.Distance(toLookAt.position,transform.position) <= visionRange){
            setLookAnimation();
        } else {
            //Only change into Idle State when NPC isn't yet in any idle state
            if(!currentState.Equals(AnimationState.NPCIdle1) && !currentState.Equals(AnimationState.NPCIdle2)){
                setIdleAnimation();
            }
        }
    }

    #endregion

    #region AnimationStates
    private enum AnimationState{
        NPCIdle1,
        NPCIdle2,
        NPCLookUp,
        NPCLookDown,
        NPCLookLeft,
        NPCLookRight
    }

    private void setLookAnimation(){
        Vector2 direction = toLookAt.position - transform.position;
        direction.Normalize();

        if(Vector2.Angle(Vector2.up, direction) <= 45){
            changeAnimationState(AnimationState.NPCLookUp);
        } else if(Vector2.Angle(Vector2.down, direction) <= 45 ){
            changeAnimationState(AnimationState.NPCLookDown);
        } else if(Vector2.Angle(Vector2.left, direction) < 45 ){
            changeAnimationState(AnimationState.NPCLookLeft);
        } else if(Vector2.Angle(Vector2.right, direction) < 45 ){
            changeAnimationState(AnimationState.NPCLookRight);
        }
    }

    private void setIdleAnimation(){
        float r = Random.Range(0f,1f);
        if(r <= 0.5f){
            changeAnimationState(AnimationState.NPCIdle1);
        } else {
            changeAnimationState(AnimationState.NPCIdle2);
        }
    }

    private void changeAnimationState(AnimationState state){
        if(state.Equals(currentState)){
            return;
        }
        animator.Play(state.ToString());
        currentState = state;
    }
    
    #endregion

}
