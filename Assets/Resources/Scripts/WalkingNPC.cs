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
    private Vector2 movement;
    private Rigidbody2D rb;
    private int currentPatrolPoint;
    private Vector2 startPos;
    private float lastPatrolPointTime;

    #region AwakeAndStart
    private void Awake(){
        movement = Vector2.zero;
        currentPatrolPoint = 0;
        lastPatrolPointTime = 0;
    }

    private void Start(){
        rb = gameObject.GetComponent<Rigidbody2D>();
        startPos = rb.position;
    }
    #endregion
    
    #region UpdateAndFixedUpdate
    private void Update(){
        if(getTimeSinceLastPatrolPointReached() < patrolPauseTimeSeconds){
            movement = Vector2.zero;
        } else {
            processMovement();
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
