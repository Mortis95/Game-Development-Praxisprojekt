using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A LOT of this class is copypasted from EnemyBehaviourMelee, with various Adjustments
//This is due to Unity not properly allowing classes to derive their Update or Awake() Method from their parent-classes
//Generally, having one class extend another is a frickly situation in Unity.
//And who knows, maybe we event want to completely differ EnemyBehaviourRanged from EnemyBehaviourMelee
public class EnemyBehaviourRanged : MonoBehaviour, EnemyBehaviour{

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
    [Tooltip("The range at which the enemy can shoot at the player.")]
    public float attackRange;
    [Tooltip("The speed of the shot that is shot at the player")]
    public float shotSpeed;
    [Tooltip("The Sprite the projectile will use.")]
    public Sprite projectileSprite;

    //Protected Variables we should get and calculate ourselves
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
    private float updateMovementTimerSeconds;
    private float lastPatrolPointTime;
    private float[] anglesToCheck;
    private float lastWorkingAngle;
    private GameObject rangedAttackPrefab;
    private bool receivingKnockback;
    private bool isDying;

    //Primitive Variables can be assigned as soon as Game Object awakes without Issue
    private void Awake(){
        targetFound = false;
        movement = Vector2.zero;
        lastAttackTime = 0;
        lastUpdateMovementTime = Random.Range(0f,updateMovementTimerSeconds);   //Load balancing, don't let all enemy instances be synchronized
        currentMoveSpeed = moveSpeed;
        currentPatrolPoint = 0;
        lastPatrolPointTime = 0;
        updateMovementTimerSeconds = 0.5f;
        lastWorkingAngle = 0f;
        receivingKnockback = false;
        isDying = false;
        rangedAttackPrefab = Resources.Load<GameObject>("Prefabs/EnemyRangedAttackPrefab");
    }

    //Some Unity-specific variables should only be assigned on Start() of script, to ensure other GameObjects finished loading.
    private void Start(){
        layerMask = LayerMask.GetMask("Blocking","npcLayer");
        enemyManager = gameObject.GetComponent<EnemyManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        target = Player.getInstance().GetComponent<Rigidbody2D>();
        startPos = rb.position;
        lastMovement = Vector2.up;
        anglesToCheck = new float[] {10f, -10f, 20f, -20f, 30f, -30.0f};    //Static angles, because a high ViewDetectionAngle might break the Raycast otherwise
    }

    private void Update(){
        if(isDying){return;}
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


    #region ProcessMovement
    private void processUnalertedMovement(){
        //Set movement towards next patrol point
        //But only if a patrol route actually exists.
        if(patrolRoute != null && patrolRoute.Length > 0){
            Vector2 headedTo = patrolRoute[currentPatrolPoint] + startPos;
            setMoveTowardsPoint(headedTo); 
        }
    }
    
    void processAlertedMovement(){
        if(checkDirectApproach()){
            //Is Player in range AND Attack Ready? If yes, stand still and attack.
            if(Vector2.Distance(target.position, transform.position) < attackRange && getTimeSinceLastAttack() > attackDelaySeconds){
                attackWithRangedAttack();
                lastAttackTime = Time.fixedTime;
                movement = Vector2.zero;
            //Else is Player in range (but Attack not ready)? If yes, stand still and wait for attack.
            } else if(Vector2.Distance(target.position, transform.position) < attackRange){
                movement = Vector2.zero;
            //Else Player must be out of range. Move in closer.
            } else {
                setMoveTowardsPoint(target.position);
            }
        } else {
            float workingAngle = calculateWorkingAngle(anglesToCheck, target.position);
            Vector2 targetPoint = Vector3Extension.RotatePointAroundPivot(target.position, rb.position, new Vector3(0,0,workingAngle));
            setMoveTowardsPoint(targetPoint);
        }
    }

    void attackWithRangedAttack(){
        EnemyRangedAttackController erac = rangedAttackPrefab.GetComponent<EnemyRangedAttackController>();
        erac.setParameters(projectileSprite, (target.position - rb.position).normalized, shotSpeed, 5f, enemyManager.enemyAttack);
        Instantiate(rangedAttackPrefab, transform.position, transform.rotation);
    }

    //This method returns true if the direct approach is possible, i.e. if the raycast didn't hit an obstacle or hit the player directly.
    private bool checkDirectApproach(){
        Vector2 direction = target.position - rb.position;
        RaycastHit2D hit = Physics2D.CircleCast(rb.position, 1f, direction, detectionRange, layerMask);
        return (!hit || hit.collider.tag == "Player");
    }

    private float calculateWorkingAngle(float[] angles, Vector2 pos){
        //Initialize help variables with 0
        float totalOfAnglesThatWork = 0f;
        int amountOfAnglesThatWork = 0;

        //Iterate over every angle and try a raycast.
        for(int i = 0; i < angles.Length; i++){
            float angle = angles[i];
            Vector2 direction = ((Vector2) Vector3Extension.RotatePointAroundPivot(pos, rb.position, new Vector3(0,0,angle))) - rb.position;
            RaycastHit2D hit = Physics2D.CircleCast(rb.position, 1f, direction, detectionRange, layerMask);
            
            //Wenn etwas getroffen, dass nicht der Player ist, dann kann das mit der layerMask nurnoch Kollision oder ein NPC sein.
            //Ist also blockierend und sollte ignoriert werden.
            if(hit && hit.collider.tag != "Player"){
                Debug.DrawRay(rb.position, direction, Color.red, 0.1f);
            //Anosonsten kann der Winkel miteinbezogen werden.
            } else {
                Debug.DrawRay(rb.position, direction, Color.green, 0.1f);
                totalOfAnglesThatWork += angle;
                amountOfAnglesThatWork++;
            }
        }
        
        //Wenn kein Winkel funktioniert steht man vor einer Wand. Entweder links oder rechts laufen nach bestem Ermessen
        if(amountOfAnglesThatWork == 0){
            if(lastWorkingAngle > 0){
                return 90f;
            } else {
                return -90f;
            }
        } 

        //Durchschnittswinkel von allen die funktionieren berechnen.
        float finalAngle = totalOfAnglesThatWork / amountOfAnglesThatWork;
        lastWorkingAngle = finalAngle;
        
        return finalAngle;
    }

    //Everything that should happen once the Player is found
    public void findTarget(){
        targetFound = true;
        currentMoveSpeed = runSpeed;
        //TODO: Maybe play some enemy sound? Gotta wait for someone to actually be Audio smart in that case... 

    }
    private void checkForTarget(){
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

    private void checkPatrolPoints(){
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

    private float getTimeSinceLastMovementUpdate(){
        return Time.time - lastUpdateMovementTime;
    }
    
    private void setMoveTowardsPoint(Vector2 target){
        Vector2 direction = target - rb.position;
        direction.Normalize();
        movement = direction;
        lastMovement = direction;
    }
    
    #endregion

    private void FixedUpdate(){
        if(!receivingKnockback){rb.MovePosition(rb.position + movement * currentMoveSpeed * Time.fixedDeltaTime);}
    }

    public void getKnockedBack(Vector2 origin, float knockBackForce){
        StartCoroutine(knockBackLoop(origin, knockBackForce));
    }

    IEnumerator knockBackLoop(Vector2 origin, float knockBackForce){
        receivingKnockback = true;
        float startTime = Time.time;
        float knockBackTimeSeconds = 0.25f;
        float actualKnockBackForce = Mathf.Max(knockBackForce - enemyManager.enemyDefense, 0f); //Reduce Knockback force by Enemy Defense. But don't allow negative Knockback
        Vector2 direction = rb.position - origin;
        direction.Normalize();
        while (Time.time - startTime < knockBackTimeSeconds){
            rb.MovePosition(rb.position + direction * knockBackForce * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        receivingKnockback = false;

    }

    private float getTimeSinceLastAttack(){
        return Time.fixedTime - lastAttackTime;
    }

    private float getTimeSinceLastPatrolPointReached(){
        return Time.time - lastPatrolPointTime;
    }

    private void OnCollisionStay2D(Collision2D col){
        return; //A ranged enemy has no melee attack
    }

    public void onDeath(){
        isDying = true;
        movement = Vector2.zero;       
    }

    #region GizmoDebugStuffForInternalUseOnly
    private void OnDrawGizmosSelected(){
        if(Application.isPlaying){
            drawGizmoPlaying();
        } else {
            drawGizmoDefault();
        }

    }

    private void drawGizmoDefault(){
        
        drawGizmoRange();
        drawGizmoViewAngleDefault();

        //If no patrol Route exists, there is none to draw.
        if(patrolRoute == null){return;}
        if(patrolRoute.Length <= 0){return;}

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

    private void drawGizmoPlaying(){

        drawGizmoRange();
        drawGizmoViewAnglePlaying();

        if(patrolRoute == null){return;}
        if(patrolRoute.Length <= 0){return;}

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

    private void drawGizmoRange(){
        //Display the Range of enemy (green)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        //Display the Shooting Range of enemy (magenta)
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void drawGizmoViewAngleDefault(){
        Gizmos.color = Color.yellow;
        Vector3 pointUp = Vector3Extension.RotatePointAroundPivot(transform.position + Vector3.up, transform.position, new Vector3(0f, 0f, detectionViewAngle / 2));
        Vector3 pointDown = Vector3Extension.RotatePointAroundPivot(transform.position + Vector3.up, transform.position, new Vector3(0f, 0f, -detectionViewAngle / 2));
        Gizmos.DrawRay(transform.position, (pointUp - transform.position) * detectionRange);
        Gizmos.DrawRay(transform.position, (pointDown - transform.position) * detectionRange);
    }

    private void drawGizmoViewAnglePlaying(){
        Gizmos.color = Color.yellow;
        Vector3 pointUp = Vector3Extension.RotatePointAroundPivot(transform.position + (Vector3) lastMovement, transform.position, new Vector3(0f, 0f, detectionViewAngle / 2));
        Vector3 pointDown = Vector3Extension.RotatePointAroundPivot(transform.position + (Vector3) lastMovement, transform.position, new Vector3(0f, 0f, -detectionViewAngle / 2));
        Gizmos.DrawRay(transform.position, (pointUp - transform.position) * detectionRange);
        Gizmos.DrawRay(transform.position, (pointDown - transform.position) * detectionRange);
    }

    #endregion

}
