using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingNPC : MonoBehaviour{

    //Public Variables other people can manipulate from the Inspector
    [Tooltip("The speed at which the GameObject moves. Set to 0 for stationary.")]
    public float moveSpeed;
    [Tooltip("The route in relative units that the GameObject will walk along with moveSpeed.")]
    public Vector2[] patrolRoute;
    [Tooltip("A checkpoint is considered reachd, if the distance to it is less than this margin of error. Use this in case fast NPCs have trouble registering a point in their Patrol Route."), Range(0.1f, 10f)]
    public float marginOfError;
    [Tooltip("The time the GameObject will pause walking when it reaches a point in its patrolRoute.")]
    public float patrolPauseTimeSeconds;

    //Private Variables we should get and calculate ourselves
    private Animator animator;
    private AnimationState currentState;
    private Vector2 movement;
    private Vector2 lastMovement;
    private Rigidbody2D rb;
    private int currentPatrolPoint;
    private Vector2 startPos;
    private float lastPatrolPointTime;

    #region AwakeAndStart
    private void Awake(){
        movement = Vector2.zero;
        lastMovement = Vector2.down;
        currentPatrolPoint = 0;
        lastPatrolPointTime = 0;
        if(marginOfError <= 0){
            marginOfError = 0.1f;
        }
    }

    private void Start(){
        animator = gameObject.GetComponent<Animator>();
        animator.speed = 0;
        rb = gameObject.GetComponent<Rigidbody2D>();
        startPos = rb.position;
    }
    #endregion
    
    #region UpdateAndFixedUpdate
    private void Update(){
        if(getTimeSinceLastPatrolPointReached() < patrolPauseTimeSeconds){
            movement = Vector2.zero;
            setIdleAnimation();
        } else {
            processMovement();
            setMovementAnimation();
        }
        if(patrolRoute != null && patrolRoute.Length > 0){checkPatrolPoints();}
    }

    private void FixedUpdate(){
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    #endregion
    
    #region MovementMethods
    private float getTimeSinceLastPatrolPointReached(){
        return Time.time - lastPatrolPointTime;
    }

    private void processMovement(){
        //Set movement towards next patrol point
        //But only if a patrol route actually exists.
        if(patrolRoute != null && patrolRoute.Length > 0){
            Vector2 headedTo = patrolRoute[currentPatrolPoint] + startPos;
            setMoveTowardsPoint(headedTo); 
        }
    }

    private void setMoveTowardsPoint(Vector2 targetPos){
        Vector2 direction = targetPos - rb.position;
        direction.Normalize();
        movement = direction;
        lastMovement = direction;
    }

    private void checkPatrolPoints(){
        //Check if patrol point needs to be updated
        //We consider a patrol point reached if it's closer than 0.5 world units away
        if(Vector2.Distance(rb.position, patrolRoute[currentPatrolPoint] + startPos) < marginOfError){
            currentPatrolPoint++;
            //Patrol Route wraps back to Point 0
            currentPatrolPoint = currentPatrolPoint % patrolRoute.Length;
            //Update time variable
            lastPatrolPointTime = Time.time;
        }
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
    
    #region GizmoDrawMethods
    private void OnDrawGizmosSelected(){

        //If no patrol Route exists, there is none to draw.
        if(patrolRoute == null){return;}
        if(patrolRoute.Length <= 0){return;}
        
        if(Application.isPlaying){
            drawGizmoPlaying();
        } else {
            drawGizmoDefault();
        }

    }

    private void drawGizmoDefault(){
        //Get own position to calculate relative path
        Vector2 offset = transform.position;

        //Display the Route of the GameObject (red)
        Gizmos.color = Color.red;
        for(int i = 0; i < patrolRoute.Length - 1; i++){
            Gizmos.DrawLine(patrolRoute[i] + offset, patrolRoute[i+1] + offset);
        }
        if(patrolRoute != null && patrolRoute.Length > 0){
            Gizmos.DrawLine(patrolRoute[patrolRoute.Length - 1] + offset,patrolRoute[0] + offset);
            Gizmos.color = Color.yellow;

            //Display Startpoint of Route
            Gizmos.DrawSphere(patrolRoute[0] + offset, 0.5f);
        }
    }

        private void drawGizmoPlaying(){
        //Get own position to calculate relative path
        Vector2 offset = startPos;

        //Display the Route of the GameObject (red)
        Gizmos.color = Color.red;
        for(int i = 0; i < patrolRoute.Length - 1; i++){
            Gizmos.DrawLine(patrolRoute[i] + offset, patrolRoute[i+1] + offset);
        }
        if(patrolRoute != null && patrolRoute.Length > 0){
            Gizmos.DrawLine(patrolRoute[patrolRoute.Length - 1] + offset,patrolRoute[0] + offset);
            Gizmos.color = Color.yellow;

            //Display Startpoint of Route
            Gizmos.DrawSphere(patrolRoute[0] + offset, 0.5f);
        }
    }

    #endregion

}
