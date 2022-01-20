using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourMelee : MonoBehaviour, EnemyBehaviour{

    //Public Variables other people can manipulate from the Inspector
    [Tooltip("The speed at which the enemy moves when not alerted by the player. Set to 0 for stationary enemy.")]
    public float moveSpeed;
    [Tooltip("The speed at which the enemy moves when chasing the player.")]
    public float runSpeed;
    [Tooltip("The delay between enemy attacks in seconds.")]
    public float attackDelaySeconds;
    [Tooltip("The range at which the enemy can detect the player.")]
    public float detectionRange;
    [Tooltip("The angle that the enemy is able to see the player at. Set to 360 for all around vision"), Range(1f,360f)]
    public float detectionViewAngle;
    [Tooltip("The route in relative units that the enemy will walk along with moveSpeed as long as it didn't spot the player.")]
    public Vector2[] patrolRoute;
    [Tooltip("The time the enemy will pause walking when it reaches a point in its patrolRoute.")]
    public float patrolPauseTimeSeconds;
    [Tooltip("How many seconds should pass inbetween each update of movement. No delay may result in less performance if a lot of enemies exist on screen.")]
    public float updateMovementTimerSeconds;
    

    //Private Variables we should get and calculate ourselves
    private Vector2 movement;
    private Vector2 lastMovement;
    private float currentMoveSpeed;
    private float lastAttackTime;
    private Rigidbody2D target;
    private bool targetFound;
    private Rigidbody2D rb;
    private EnemyManager enemyManager;
    private LayerMask layerMask;
    private int currentPatrolPoint;
    private Vector2 startPos;
    private float lastUpdateMovementTime;
    private float lastPatrolPointTime;

    //Primitive Variables can be assigned as soon as Game Object awakes without Issue
    private void Awake(){
        targetFound = false;
        movement = Vector2.zero;
        lastAttackTime = 0;
        lastUpdateMovementTime = Random.Range(0f,updateMovementTimerSeconds);
        currentMoveSpeed = moveSpeed;
        currentPatrolPoint = 0;
        lastPatrolPointTime = 0;
    }

    //Some Unity-specific variables should only be assigned on Start() of script, to ensure other GameObjects finished loading.
    void Start(){
        layerMask = LayerMask.GetMask("Blocking","npcLayer");
        enemyManager = gameObject.GetComponent<EnemyManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        target = Player.getInstance().GetComponent<Rigidbody2D>();
        startPos = rb.position;
        
    }

    void Update(){
        if(getTimeSinceLastPatrolPointReached() < patrolPauseTimeSeconds){
            movement = Vector2.zero;
        }
        else if(getTimeSinceLastMovementUpdate() > updateMovementTimerSeconds){
            lastUpdateMovementTime = Time.time;
            if(!targetFound){processUnalertedMovement();}
            else {processAlertedMovement();}
        }
        if(patrolRoute != null && patrolRoute.Length > 0){checkPatrolPoints();}
        if(!targetFound){checkForTarget();}
    }

    void checkPatrolPoints(){
        //Check if patrol point needs to be updated
        //We consider a patrol point reached if it's closer than 0.5 world units away
        if(Vector2.Distance(rb.position, patrolRoute[currentPatrolPoint] + startPos) < 0.5f){
            currentPatrolPoint++;
            //Patrol Route wraps back to Point 0
            currentPatrolPoint = currentPatrolPoint % patrolRoute.Length;
            //Update time variable
            lastPatrolPointTime = Time.time;
        }
    }

    void checkForTarget(){
        //Check for target in range
        if(Vector2.Distance(rb.position, target.position) < detectionRange){
            Vector2 targetDirection = target.position - rb.position;
            float angle = Vector2.Angle(targetDirection, lastMovement);
            if(angle < detectionViewAngle / 2){
                RaycastHit2D hit = Physics2D.CircleCast(rb.position, 1f, target.position - rb.position, detectionRange, layerMask);
                Debug.DrawRay(rb.position, target.position - rb.position, Color.red);
                if(hit && hit.collider.tag == "Player"){
                    Debug.Log("Player found!");
                    findTarget();
                }
            }
        }
    }

    #region ProcessMovement
    void processUnalertedMovement(){
        //Set movement towards next patrol point
        //But only if a patrol route actually exists.
        if(patrolRoute != null && patrolRoute.Length > 0){
            Vector2 headedTo = patrolRoute[currentPatrolPoint] + startPos;
            movement = headedTo - rb.position;
            movement.Normalize();
            lastMovement = movement;  
        }
    }
    
    void processAlertedMovement(){
        movement = target.position - rb.position;
        movement.Normalize();
        lastMovement = movement;
        //Check direct approach
    }

    //Everything that should happen once the Player is found
    public void findTarget(){
        targetFound = true;
        currentMoveSpeed = runSpeed;
        //TODO: Maybe play some enemy sound? Gotta wait for someone to actually be Audio smart in that case... 

    }

    float getTimeSinceLastMovementUpdate(){
        return Time.time - lastUpdateMovementTime;
    }
    
    
    
    #endregion

    void FixedUpdate(){
        rb.MovePosition(rb.position + movement * currentMoveSpeed * Time.fixedDeltaTime);
    }

    public void getKnockedBack(){
        return;
    }

    private float getTimeSinceLastAttack(){
        return Time.fixedTime - lastAttackTime;
    }

    private float getTimeSinceLastPatrolPointReached(){
        return Time.time - lastPatrolPointTime;
    }

    void OnCollisionStay2D(Collision2D col){
        if(col.gameObject.tag == "Player" && getTimeSinceLastAttack() > attackDelaySeconds){
            lastAttackTime = Time.fixedTime;
            Player pl = col.gameObject.GetComponent<Player>();
            if(pl != null){
                pl.takeDamage(enemyManager.enemyAttack);
                pl.getKnockedBack(rb);
            }
        }
    }

    #region GizmoDebugStuffForInternalUseOnly
    void OnDrawGizmosSelected(){
        if(Application.isPlaying){
            drawGizmoPlaying();
        } else {
            drawGizmoDefault();
        }

    }

    void drawGizmoDefault(){
        
        drawGizmoRange();
        drawGizmoViewAngleDefault();

        //Get own position to calculate relative path
        Vector2 offset = transform.position;

        //Display the Route of the enemy (red)
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

    void drawGizmoPlaying(){

        drawGizmoRange();
        drawGizmoViewAnglePlaying();

        //Get own position to calculate relative path
        Vector2 offset = startPos;

        //Display the Route of the enemy (red)
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

    void drawGizmoRange(){
        //Display the Range of enemy (green)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    void drawGizmoViewAngleDefault(){
        Gizmos.color = Color.yellow;
        Vector3 pointUp = RotatePointAroundPivot(transform.position + Vector3.up, transform.position, new Vector3(0f, 0f, detectionViewAngle / 2));
        Vector3 pointDown = RotatePointAroundPivot(transform.position + Vector3.up, transform.position, new Vector3(0f, 0f, -detectionViewAngle / 2));
        Gizmos.DrawRay(transform.position, (pointUp - transform.position) * detectionRange);
        Gizmos.DrawRay(transform.position, (pointDown - transform.position) * detectionRange);
    }

    void drawGizmoViewAnglePlaying(){
        Gizmos.color = Color.yellow;
        Vector3 pointUp = RotatePointAroundPivot(transform.position + (Vector3) lastMovement, transform.position, new Vector3(0f, 0f, detectionViewAngle / 2));
        Vector3 pointDown = RotatePointAroundPivot(transform.position + (Vector3) lastMovement, transform.position, new Vector3(0f, 0f, -detectionViewAngle / 2));
        Gizmos.DrawRay(transform.position, (pointUp - transform.position) * detectionRange);
        Gizmos.DrawRay(transform.position, (pointDown - transform.position) * detectionRange);
    }

    #endregion

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
    return point; // return it
 }
}
