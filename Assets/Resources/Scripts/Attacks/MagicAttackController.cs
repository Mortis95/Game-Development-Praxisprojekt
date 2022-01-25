using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttackController : MonoBehaviour
{

    //Images
    public SpriteRenderer spr;
    public Sprite magicProjectile;

    //Settings
    public float disappearAfterSeconds;
    public float speed;
    public Vector3 direction;        
    

    [SerializeField]
    private int damage;
    private float knockBackForce;
    

    
    private void Awake(){
        //Get Player Instance
        Player pl = Player.getInstance();

        //Set Damage and Knockback
        int attack = pl.getAttack();
        int intelligence = pl.getIntelligence();
        damage = attack + intelligence;
        knockBackForce = intelligence / 2;
        
        //Pick correct image to display and correct offset to use for position
        spr.sprite = pl.equippedWeapon.projectile;
        Vector3 offset;
        switch(pl.lastFacedDirection){
            case Direction.Up:
                transform.Rotate(0f,0f,90f);
                offset = direction = Vector3.up;
                break;

            case Direction.Left:
                transform.Rotate(0f,0f,180f);
                offset = direction = Vector3.left;
                break;

            case Direction.Right:
                //transform.Rotate(0f,0f,0f);   //No need to rotate
                offset = direction = Vector3.right;
                break;

            default:
                transform.Rotate(0f,0f,-90f);
                offset = direction = Vector3.down;
                break;
        }

        //Set Location of Object
        transform.position = pl.transform.position + offset * 2;


        //Destroy Sprite after seconds
        Destroy(gameObject, disappearAfterSeconds);
    }

    void FixedUpdate(){
        transform.position = transform.position + direction * speed * Time.fixedDeltaTime;
    }


    void OnTriggerEnter2D(Collider2D col){
        Debug.Log("Collision with:" + col.name);
        GameObject other = col.gameObject;
        if(other != null && other.tag == "Enemy"){
            EnemyManager enemyScript = other.GetComponent<EnemyManager>();
            //Need to check for enemyScript several times, in case the enemy dies in the meantime.
            if(enemyScript != null){enemyScript.takeDamage(DamageType.Normal,damage);}
            if(enemyScript != null){enemyScript.getKnockback(transform.position, knockBackForce);}
        }
        Destroy(gameObject);
    }
}
