using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpinnenBossBehaviour : MonoBehaviour, EnemyBehaviour
{
    #region PublicVariables
    //Public variables people can set in the Inspector
    public float moveSpeed;
    public float meleeAttackCooldownSeconds;
    [Tooltip("The factor that EnemyAttack in EnemyManager gets multiplied with, to calculate how much damage the Player should take from a Melee Hit."), Range(0.01f, 5f)]
    public float meleeAttackDamageScale;
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

    #region SetAnimations
    private enum AnimationState
    {
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
    #endregion

    #region Start
    void Start()
    {
        enemyManager = gameObject.GetComponent<EnemyManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        movement = Vector2.zero;
        goalPosition = rb.position;
        animator = gameObject.GetComponent<Animator>();
        target = Player.getInstance().gameObject;
        lastMeleeAttackTime = Time.fixedTime;
        lastKnifeAttackTime = Time.fixedTime;
        lastAxeAttackTime = Time.fixedTime;
        meleeAttackReady = true;
        rangedKnifeAttackReady = true;
        rangedAxeAttackReady = true;
        busy = false;
        lastActionTime = Time.fixedTime;
        currentActionDelaySeconds = 0f;
    }
    #endregion

    public void getKnockedBack(Vector2 origin, float knockBackForce) { return; }

    public void findTarget() { return; }

    public void onDeath() {

    }

    void standStill() { movement = Vector2.zero; }

    void decideMovement()
    {
        if (getTimeSinceLastGoalPosition() > newGoalPositionTime)
        {
            setLastGoalPositionTime();
            Vector2 offset = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
            goalPosition = (Vector2)(target.transform.position) + offset;
        }

        if (Vector2.Distance(goalPosition, rb.position) < 0.5f)
        {
            standStill();
            setStandStillAnimation();
        }
        else
        {
            movement = goalPosition - rb.position;
            movement.Normalize();

            setMovementAnimation();
        }
    }

    #region SetTimes
    void setLastMeleeAttackTime() { lastMeleeAttackTime = Time.fixedTime; }
    void setLastKnifeAttackTime() { lastKnifeAttackTime = Time.fixedTime; }
    void setLastAxeAttackTime() { lastAxeAttackTime = Time.fixedTime; }
    void setLastActionTime() { lastActionTime = Time.fixedTime; }
    void setLastGoalPositionTime() { lastGoalPositionTime = Time.fixedTime; }
    #endregion

    #region GetTimes
    private float getTimeSinceLastMeleeAttack()
    {
        return (Time.fixedTime - lastMeleeAttackTime);
    }

    private float getTimeSinceLastKnifeAttack()
    {
        return (Time.fixedTime - lastKnifeAttackTime);
    }

    private float getTimeSinceLastAxeAttack()
    {
        return (Time.fixedTime - lastAxeAttackTime);
    }

    private float getTimeSinceLastAction()
    {
        return (Time.fixedTime - lastActionTime);
    }

    private float getTimeSinceLastGoalPosition()
    {
        return (Time.fixedTime - lastGoalPositionTime);
    }
    #endregion

    private void changeAnimationState(AnimationState newState)
    {
        if (newState.Equals(currentState)) { return; }
        if (animator == null) { Debug.LogWarning("Missing Animator at GameObject: " + gameObject); return; }
        animator.Play(newState.ToString());
        currentState = newState;
    }

    private void setMovementAnimation()
    {
        animator.speed = 1f;
        if (Vector2.Angle(Vector2.up, movement) <= 45)
        {
            changeAnimationState(AnimationState.EnemyWalkUp);
        }
        else if (Vector2.Angle(Vector2.down, movement) <= 45)
        {
            changeAnimationState(AnimationState.EnemyWalkDown);
        }
        else if (Vector2.Angle(Vector2.left, movement) < 45)
        {
            changeAnimationState(AnimationState.EnemyWalkLeft);
        }
        else if (Vector2.Angle(Vector2.right, movement) < 45)
        {
            changeAnimationState(AnimationState.EnemyWalkRight);
        }
    }

    private void setMeleeAttackAnimation()
    {
        animator.speed = 1f;
        if (Vector2.Angle(Vector2.up, movement) <= 45)
        {
            changeAnimationState(AnimationState.EnemyAttackUp);
        }
        else if (Vector2.Angle(Vector2.down, movement) <= 45)
        {
            changeAnimationState(AnimationState.EnemyAttackDown);
        }
        else if (Vector2.Angle(Vector2.left, movement) < 45)
        {
            changeAnimationState(AnimationState.EnemyAttackLeft);
        }
        else if (Vector2.Angle(Vector2.right, movement) < 45)
        {
            changeAnimationState(AnimationState.EnemyAttackRight);
        }
    }


    private void setStandStillAnimation()
    {
        animator.speed = 0f;
    }
}

