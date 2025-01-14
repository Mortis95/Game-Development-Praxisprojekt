using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBehaviourBossGott : MonoBehaviour, EnemyBehaviour {

    #region PublicVariables
    //Public variables people can set in the Inspector
    public float moveSpeed;
    public float meleeAttackCooldownSeconds;
    [Tooltip("The factor that EnemyAttack in EnemyManager gets multiplied with, to calculate how much damage the Player should take from a Melee Hit."), Range(0.01f,5f)]
    public float meleeAttackDamageScale;
    public float teleportAttackCooldownSeconds;
    [Tooltip("The factor that EnemyAttack in EnemyManager gets multiplied with, to calculate how much damage the Player should take from a Teleport Attack Hit."), Range(0.01f,5f)]
    public float teleportAttackDamageScale;
    [Tooltip("The speed of the Teleport-Attack Dash")]
    public float teleportAttackDashSpeed;
    [Tooltip("How close should the Boss teleport behind the player?")]
    public float teleportDistance;
    public UnityEvent onDeathEvent;
    #endregion

    #region PrivateVariables
    //Private variables that help the GameObject operate
    private EnemyManager enemyManager;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 goalPosition;
    private float lastGoalPositionTime;
    private const float newGoalPositionTime = 1.5f;
    private Animator animator;
    private AnimationState currentState;
    private GameObject target;
    private float lastMeleeAttackTime;
    private float lastTeleportAttackTime;
    private bool meleeAttackReady;
    private bool teleportAttackReady;
    private bool busy;
    private float lastActionTime;
    private float currentActionDelaySeconds;
    private const float meleeAttackDelay = 1.5f;
    private const float teleportAttackDelay = 4f;
    private bool receivingKnockback;
    
    #endregion

    #region Start
    void Start(){
        enemyManager = gameObject.GetComponent<EnemyManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        movement = Vector2.zero;
        goalPosition = rb.position;
        animator = gameObject.GetComponent<Animator>();
        target = Player.getInstance().gameObject;
        lastMeleeAttackTime    = Time.fixedTime;
        lastTeleportAttackTime = Time.fixedTime;
        meleeAttackReady       = true;
        teleportAttackReady    = false;
        busy                   = false;
        lastActionTime         = Time.fixedTime;
        currentActionDelaySeconds = 0f;
        receivingKnockback = false;
    }

    #endregion
    
    #region UpdateAndCoreFunctions
    void Update(){
        if(busy){standStill();}
        else{decideAction();}
        checkCooldowns();
        checkBusy();
    }

    void decideAction(){
        decideMovement();
        decideAttack();
    }

    void decideMovement(){
        if(getTimeSinceLastGoalPosition() > newGoalPositionTime){
            setLastGoalPositionTime();
            Vector2 offset = new Vector2(Random.Range(-5, 5),Random.Range(-5,5));
            goalPosition = (Vector2) (target.transform.position) + offset;
        }

        if(Vector2.Distance(goalPosition, rb.position) < 0.5f){
            standStill();
            setStandStillAnimation();
        } else {
            movement = goalPosition - rb.position;
            movement.Normalize();

            setMovementAnimation();
        }
    }
    void decideAttack(){
        if(Vector2.Distance(rb.position,target.transform.position) < 3 && meleeAttackReady){
            performMeleeAttack();
            return;
        }

        if(teleportAttackReady){
            performTeleportAttack();
            return;
        }
    }

    void FixedUpdate(){if(!receivingKnockback){rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);}}

    void standStill(){movement = Vector2.zero;}

    #endregion

    #region Attacks
    void performMeleeAttack(){
        //Prepare necessary Info for the attack, such as direction and damage
        //Also set Animations
        Vector2 targetDirection = (Vector2) target.transform.position - rb.position;
        targetDirection.Normalize();

        Direction attackDirection = Direction.Down;
        if(Vector2.Angle(Vector2.up, targetDirection) <= 45){
            attackDirection = Direction.Up;
            changeAnimationState(AnimationState.EnemyAttackUp);
        } else if(Vector2.Angle(Vector2.down, targetDirection) <= 45 ){
            attackDirection = Direction.Down;
            changeAnimationState(AnimationState.EnemyAttackDown);
        } else if(Vector2.Angle(Vector2.left, targetDirection) < 45 ){
            attackDirection = Direction.Left;
            changeAnimationState(AnimationState.EnemyAttackLeft);
        } else if(Vector2.Angle(Vector2.right, targetDirection) < 45 ){
            attackDirection = Direction.Right;
            changeAnimationState(AnimationState.EnemyAttackRight);
        }

        int damage = (int)((float) enemyManager.enemyAttack * meleeAttackDamageScale);

        //Create the attack
        BossGottMeleeAttack.createAttack(transform, attackDirection, damage);

        //Set necessary internal states
        setLastMeleeAttackTime();
        currentActionDelaySeconds = meleeAttackDelay;
        setLastActionTime();
        meleeAttackReady = false;
    }

    void performTeleportAttack(){
        //Set necessary internal states
        setLastTeleportAttackTime();
        currentActionDelaySeconds = teleportAttackDelay;
        setLastActionTime();
        teleportAttackReady = false;

        //Perform the actual attack
        StartCoroutine(teleportAttack());

    }

     IEnumerator teleportAttack(){
        Vector2 attackPoint = target.transform.position;
        
        //Shrink down
        Vector3 baseScale = transform.localScale;
        float growthRate = 0.1f;
        bool minSizeReached = false;

        while(!minSizeReached){
            transform.localScale -= new Vector3(growthRate, 0, 0);
            if(transform.localScale.x <= 0.1f){
                minSizeReached = true;
                transform.localScale = new Vector3(0.01f, 0.01f, 1);    //Make scale very small so enemy is "invisible"
            }
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.5f);

        Vector2 targetDirection = attackPoint - rb.position;
        Vector2 targetDirectionNormalized = targetDirection.normalized;
        
        //Actual teleport
        rb.position = getTeleportPosition(rb.position, attackPoint);
        yield return new WaitForSeconds(0.5f);

        //Grow up
        transform.localScale = new Vector3(0.1f, baseScale.y, 1);
        bool maxSizeReached = false;
        while(!maxSizeReached){
            transform.localScale += new Vector3(growthRate, 0, 0);
            if(transform.localScale.x >= baseScale.x){
                maxSizeReached = true;
                transform.localScale = baseScale;
            }
            yield return new WaitForFixedUpdate();
        }

        //Set Animation
        setDashAttackAnimation();
        

        //Spawn the DashAttackHitBox
        int damage = (int) (teleportAttackDamageScale * ((float)enemyManager.enemyAttack));
        GameObject hitbox = BossGottDashAttackHitbox.createAttack(transform, damage);

        //Dash into attackpoint

        bool dashFinished = false;
        float startDashTime = Time.fixedTime;
        while(!dashFinished){
            rb.position = rb.position + (-targetDirectionNormalized) * teleportAttackDashSpeed * Time.fixedDeltaTime;
            if((Time.fixedTime - startDashTime) >= 1f){
                dashFinished = true;
            }
            yield return new WaitForFixedUpdate();
        }

        Destroy(hitbox);

        yield break;
    }

    private Vector2 getTeleportPosition(Vector2 selfPos, Vector2 targetPos){
        LayerMask lm =  LayerMask.GetMask("Blocking");
        RaycastHit2D hit = Physics2D.Raycast(targetPos, targetPos - selfPos, teleportDistance, lm);
        if(hit){
            return targetPos + (targetPos - selfPos).normalized * hit.distance;
        }
        return targetPos + (targetPos - selfPos).normalized * teleportDistance;
    } 
    
    #endregion

    #region SetTimes
    void setLastMeleeAttackTime(){lastMeleeAttackTime = Time.fixedTime;}
    void setLastTeleportAttackTime(){lastTeleportAttackTime = Time.fixedTime;}
    void setLastActionTime(){lastActionTime = Time.fixedTime;}
    void setLastGoalPositionTime(){lastGoalPositionTime = Time.fixedTime;}    
    #endregion

    #region GetTimes
    private float getTimeSinceLastMeleeAttack(){
        return (Time.fixedTime - lastMeleeAttackTime);
    }

    private float getTimeSinceLastTeleportAttack(){
        return (Time.fixedTime - lastTeleportAttackTime);
    }

    private float getTimeSinceLastAction(){
        return (Time.fixedTime - lastActionTime);
    }

    private float getTimeSinceLastGoalPosition(){
        return (Time.fixedTime - lastGoalPositionTime);
    }
    
    #endregion

    #region Cooldowns
    private void checkCooldowns(){
        if(getTimeSinceLastMeleeAttack() > meleeAttackCooldownSeconds){
            meleeAttackReady = true;
            }
        if(getTimeSinceLastTeleportAttack() > teleportAttackCooldownSeconds){
            teleportAttackReady = true;
            }
    }

    #endregion

    #region Busy
    private void checkBusy(){
        if(getTimeSinceLastAction() > currentActionDelaySeconds){
            busy = false;
        } else {
            busy = true;
        }
    }

    #endregion

    #region EnemyBehaviourInterface
    public void findTarget(){return;}
    public void onDeath(){
        busy = true;
        currentActionDelaySeconds = 1000f;
        setLastActionTime();
        onDeathEvent.Invoke();
        return;
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
    #endregion

    #region SetAnimations
    private enum AnimationState{
        EnemyWalkUp,
        EnemyWalkDown,
        EnemyWalkLeft,
        EnemyWalkRight,
        EnemyAttackUp,
        EnemyAttackDown,
        EnemyAttackLeft,
        EnemyAttackRight
    }

    private void changeAnimationState(AnimationState newState){
        if(newState.Equals(currentState)){return;}
        if(animator == null){Debug.LogWarning("Missing Animator at GameObject: " + gameObject); return;}
        animator.Play(newState.ToString());
        currentState = newState;
    }

    private void setMovementAnimation(){
        animator.speed = 1f;
        if(Vector2.Angle(Vector2.up, movement) <= 45){
            changeAnimationState(AnimationState.EnemyWalkUp);
        } else if(Vector2.Angle(Vector2.down, movement) <= 45 ){
            changeAnimationState(AnimationState.EnemyWalkDown);
        } else if(Vector2.Angle(Vector2.left, movement) < 45 ){
            changeAnimationState(AnimationState.EnemyWalkLeft);
        } else if(Vector2.Angle(Vector2.right, movement) < 45 ){
            changeAnimationState(AnimationState.EnemyWalkRight);
        }
    }

    private void setMeleeAttackAnimation(){
        animator.speed = 1f;
        if(Vector2.Angle(Vector2.up, movement) <= 45){
            changeAnimationState(AnimationState.EnemyAttackUp);
        } else if(Vector2.Angle(Vector2.down, movement) <= 45 ){
            changeAnimationState(AnimationState.EnemyAttackDown);
        } else if(Vector2.Angle(Vector2.left, movement) < 45 ){
            changeAnimationState(AnimationState.EnemyAttackLeft);
        } else if(Vector2.Angle(Vector2.right, movement) < 45 ){
            changeAnimationState(AnimationState.EnemyAttackRight);
        }    
    }

    private void setDashAttackAnimation(){
        animator.speed = 2f;
        Vector2 targetDirection = target.transform.position - transform.position;

        if(Vector2.Angle(Vector2.up, targetDirection) <= 45){
            changeAnimationState(AnimationState.EnemyAttackUp);
        } else if(Vector2.Angle(Vector2.down, targetDirection) <= 45 ){
            changeAnimationState(AnimationState.EnemyAttackDown);
        } else if(Vector2.Angle(Vector2.left, targetDirection) < 45 ){
            changeAnimationState(AnimationState.EnemyAttackLeft);
        } else if(Vector2.Angle(Vector2.right, targetDirection) < 45 ){
            changeAnimationState(AnimationState.EnemyAttackRight);
        } 
    }


    private void setStandStillAnimation(){
        animator.speed = 0f;
    }
    
    
    #endregion
}
