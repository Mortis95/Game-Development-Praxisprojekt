using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackController : MonoBehaviour
{

    //Images
    public SpriteRenderer spr;
    public Sprite MeleeAttackUp;
    public Sprite MeleeAttackLeft;
    public Sprite MeleeAttackRight;
    public Sprite MeleeAttackDown;

    //Settings
    public float disappearAfterSeconds;        
    

    [SerializeField]
    private int damage;
    private float knockBackForce;
    

    
    private void Awake(){
        //AudioManager.getInstance().PlaySound("GameOver");   //For Debug Purposes
        //Get Player Instance
        Player pl = Player.getInstance();

        //Set Damage and Knockback
        int str = pl.getStrength();
        int atk = pl.getAttack();
        damage = str + atk;          //Damage = STR //Can be changed to whatever is your liking
        knockBackForce = str;

        //Pick correct image to display and correct offset to use for position
        Vector3 offset;
        switch(pl.lastFacedDirection){
            case Direction.Up:
                spr.sprite = MeleeAttackUp;
                offset = Vector3.up;
                break;

            case Direction.Left:
                spr.sprite = MeleeAttackLeft;
                offset = Vector3.left;
                break;

            case Direction.Right:
                spr.sprite = MeleeAttackRight;
                offset = Vector3.right;
                break;

            default:
                spr.sprite = MeleeAttackDown;
                offset = Vector3.down;
                break;
        }

        //Set Location of Object
        transform.position = pl.transform.position + offset * 1.5f;


        //Destroy Sprite after seconds
        Destroy(gameObject, disappearAfterSeconds);
    }


    void OnTriggerEnter2D(Collider2D col){
        Debug.Log("Collision with:" + col.name);
        GameObject other = col.gameObject;
        if(other != null && other.tag == "Enemy"){
            EnemyManager enemyScript = other.GetComponent<EnemyManager>();
            //Need to check for enemyScript several times, in case the enemy dies in the meantime.
            if(enemyScript != null){
                if(!RageBuffController.rageBuffActive){
                    enemyScript.takeDamage(DamageType.Normal, damage);
                } else {
                    enemyScript.takeDamage(DamageType.Feuer, damage);
                    }
                }
            if(enemyScript != null){enemyScript.getKnockback(transform.position, knockBackForce);}

        }
    }
}
