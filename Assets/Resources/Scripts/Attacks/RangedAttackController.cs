using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackController : MonoBehaviour
{

    //Images
    public SpriteRenderer spr;
    public Sprite arrow;

    //Settings
    public float disappearAfterSeconds;
    public float speed;
    public Vector3 direction;        
    

    [SerializeField]
    private int damage;
    

    
    private void Awake(){
        //Get Player Instance
        Player pl = Player.getInstance();

        //Set Damage
        damage = pl.dexterity;          //Damage = DEX //Can be changed to whatever is your liking
        
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
            TestEnemy enemyScript = other.GetComponent<TestEnemy>();
            enemyScript.takeDamage(DamageType.Normal, damage);
        }
        Destroy(gameObject);
    }
}