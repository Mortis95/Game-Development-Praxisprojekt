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
    

    
    private void Awake(){
        AudioManager.getInstance().PlaySound("SchwertSchlag");
        //Get Player Instance
        Player pl = Player.getInstance();

        //Set Damage
        damage = pl.getStrength() + pl.getAttack();          //Damage = STR //Can be changed to whatever is your liking
        
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
        transform.position = pl.transform.position + offset * 2;


        //Destroy Sprite after seconds
        Destroy(gameObject, disappearAfterSeconds);
    }


    void OnTriggerEnter2D(Collider2D col){
        Debug.Log("Collision with:" + col.name);
        GameObject other = col.gameObject;
        if(other != null && other.tag == "Enemy"){
            EnemyManager enemyScript = other.GetComponent<EnemyManager>();
            enemyScript.takeNormalDamage(damage);

        }
    }
}
