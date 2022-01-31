using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBehaviourBerserker : MonoBehaviour, EnemyBehaviour {

    #region PublicVariables
    //Public variables people can set in the Inspector
    public float moveSpeed;
    public float meleeAttackCooldownSeconds;
    [Tooltip("The factor that EnemyAttack in EnemyManager gets multiplied with, to calculate how much damage the Player should take from a Melee Hit."), Range(0.01f,5f)]
    public float meleeAttackDamageScale;
    public float rangedKnifeAttackCooldownSeconds;
    [Tooltip("The factor that EnemyAttack in EnemyManager gets multiplied with, to calculate how much damage the Player should take from a Ranged Knife Hit."), Range(0.01f,5f)]
    public float rangedKnifeAttackDamageScale;
    public float rangedAxeAttackCooldownSeconds;
    [Tooltip("The factor that EnemyAttack in EnemyManager gets multiplied with, to calculate how much damage the Player should take from a Ranged Axe Hit."), Range(0.01f,5f)]
    public float rangedAxeAttackDamageScale;
    public float rangedAxeAttackTimeToLiveSeconds;
    [Tooltip("The Event that plays when this enemy dies. Please assign this in the inspector.")]
    public UnityEvent onDeathEvent;
    #endregion

    #region PrivateVariables
    //Private variables that help the GameObject operate
    private EnemyManager enemyManager;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 goalPosition;
    private float lastGoalPositionTime;
    private const float newGoalPositionTime = 2f;
    private Animator animator;
    private AnimationState currentState;
    private GameObject target;
    private float lastMeleeAttackTime;
    private float lastKnifeAttackTime;
    private float lastAxeAttackTime;
    private bool meleeAttackReady;
    private bool rangedKnifeAttackReady;
    private bool rangedAxeAttackReady;
    private bool busy;
    private float lastActionTime;
    private float currentActionDelaySeconds;
    private const float meleeAttackDelay = 1f;
    private const float rangedKnifeAttackDelay = 0.5f;
    private const float rangedAxeAttackDelay = 2f;
    
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
        lastKnifeAttackTime    = Time.fixedTime;
        lastAxeAttackTime      = Time.fixedTime;
        meleeAttackReady       = true;
        rangedKnifeAttackReady = true;
        rangedAxeAttackReady   = true;
        busy                   = false;
        lastActionTime         = Time.fixedTime;
        currentActionDelaySeconds = 0f;
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
        if(Vector2.Distance(rb.position,target.transform.position) < 4 && meleeAttackReady){
            performMeleeAttack();
            return;
        }

        if(rangedKnifeAttackReady){
            performRangedKnifeAttack();
            return;
        }

        if(rangedAxeAttackReady){
            performRangedAxeAttack();
            return;
        }
    }

    void FixedUpdate(){rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);}

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
        BerserkerMeleeAttack.createAttack(transform, attackDirection, damage);

        //Set necessary internal states
        setLastMeleeAttackTime();
        currentActionDelaySeconds = meleeAttackDelay;
        setLastActionTime();
        meleeAttackReady = false;
    }

    void performRangedKnifeAttack(){
        //Prepare necessary info
        Vector2 targetDirection = (Vector2) target.transform.position - rb.position;
        targetDirection.Normalize();
        int damage = (int)((float) enemyManager.enemyAttack * rangedKnifeAttackDamageScale);

        //Set the animation
        changeAnimationState(AnimationState.EnemyRangedAttack);
        
        //Create the attack
        BerserkerRangedKnifeAttack.createAttack(transform, targetDirection, damage);
    
        //Set necessary internal states
        setLastKnifeAttackTime();
        currentActionDelaySeconds = rangedKnifeAttackDelay;
        setLastActionTime();
        rangedKnifeAttackReady = false;
    }

    void performRangedAxeAttack(){
        //Prepare necessary info
        Vector2 targetDirection = (Vector2) target.transform.position - rb.position;
        targetDirection.Normalize();
        int damage = (int)((float) enemyManager.enemyAttack * rangedAxeAttackDamageScale);

        //Set the animation
        changeAnimationState(AnimationState.EnemyRangedAttack);
        
        //Create the attack
        BerserkerRangedAxeAttack.createAttack(transform, targetDirection, damage, rangedAxeAttackTimeToLiveSeconds);
    
        //Set necessary internal states
        setLastAxeAttackTime();
        currentActionDelaySeconds = rangedAxeAttackDelay;
        setLastActionTime();
        rangedAxeAttackReady = false;
    }
    
    #endregion

    #region SetTimes
    void setLastMeleeAttackTime(){lastMeleeAttackTime = Time.fixedTime;}
    void setLastKnifeAttackTime(){lastKnifeAttackTime = Time.fixedTime;}
    void setLastAxeAttackTime(){lastAxeAttackTime = Time.fixedTime;}
    void setLastActionTime(){lastActionTime = Time.fixedTime;}
    void setLastGoalPositionTime(){lastGoalPositionTime = Time.fixedTime;}    
    #endregion

    #region GetTimes
    private float getTimeSinceLastMeleeAttack(){
        return (Time.fixedTime - lastMeleeAttackTime);
    }

    private float getTimeSinceLastKnifeAttack(){
        return (Time.fixedTime - lastKnifeAttackTime);
    }

    private float getTimeSinceLastAxeAttack(){
        return (Time.fixedTime - lastAxeAttackTime);
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
        if(getTimeSinceLastKnifeAttack() > rangedKnifeAttackCooldownSeconds){
            rangedKnifeAttackReady = true;
            }
        if(getTimeSinceLastAxeAttack() > rangedAxeAttackCooldownSeconds){
            rangedAxeAttackReady = true;
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
        currentActionDelaySeconds = 1000f; //For at least 100 seconds 
        setLastActionTime();
        changeAnimationState(AnimationState.EnemyDeath);
        onDeathEvent.Invoke();
        return;
    }
    public void getKnockedBack(Vector2 origin, float knockBackForce){return;}
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
        EnemyAttackRight,
        EnemyRangedAttack,
        EnemyDeath
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

    private void setRangedAttackAnimation(){
        animator.speed = 1f;
        changeAnimationState(AnimationState.EnemyRangedAttack);
    }

    private void setStandStillAnimation(){
        animator.speed = 0f;
    }
    
    
    #endregion
}
