using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourMelee : MonoBehaviour, EnemyBehaviour{

    //Public Variables other people can manipulate from the Inspector
    public float moveSpeed;
    public float attackDelaySeconds;
    

    //Private Variables we should get ourselves
    private Vector2 movement;
    private float lastAttack;
    private Rigidbody2D target;
    private Rigidbody2D rb;
    private EnemyManager enemyManager;
    private LayerMask layerMask;

    private void Awake(){
        movement = Vector2.zero;
        lastAttack = 0;
    }

    void Start(){
        layerMask = LayerMask.GetMask("Blocking","npcLayer");
        enemyManager = gameObject.GetComponent<EnemyManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        target = Player.getInstance().GetComponent<Rigidbody2D>();
    }

    void Update(){
        movement = target.position - rb.position;
        movement.Normalize();
    }

    void FixedUpdate(){
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void getKnockedBack(){
        return;
    }

    private float getTimeSinceLastAttack(){
        return Time.fixedTime - lastAttack;
    }

    void OnCollisionStay2D(Collision2D col){
        if(col.gameObject.tag == "Player" && getTimeSinceLastAttack() > attackDelaySeconds){
            lastAttack = Time.fixedTime;
            Player pl = col.gameObject.GetComponent<Player>();
            if(pl != null){
                pl.takeDamage(enemyManager.enemyAttack);
                pl.getKnockedBack(rb);
            }
        }
    }
}
