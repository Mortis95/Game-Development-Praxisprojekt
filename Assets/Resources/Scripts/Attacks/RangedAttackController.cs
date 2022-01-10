using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackController : MonoBehaviour
{

    //Images
    public SpriteRenderer spr;
    public Sprite RangedAttackUp;
    public Sprite RangedAttackLeft;
    public Sprite RangedAttackRight;
    public Sprite RangedAttackDown;

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
        Vector3 offset;
        switch(pl.lastFacedDirection){
            case Direction.Up:
                spr.sprite = RangedAttackUp;
                offset = direction = Vector3.up;
                break;

            case Direction.Left:
                spr.sprite = RangedAttackLeft;
                offset = direction = Vector3.left;
                break;

            case Direction.Right:
                spr.sprite = RangedAttackRight;
                offset = direction = Vector3.right;
                break;

            default:
                spr.sprite = RangedAttackDown;
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
