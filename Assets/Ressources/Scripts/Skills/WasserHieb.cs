using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasserHieb : MonoBehaviour
{

    //Images
    public SpriteRenderer spr;
    public Sprite WasserHiebUpwards;
    public Sprite WasserHiebLeft;
    public Sprite WasserHiebRight;
    public Sprite WasserHiebDownwards;

    //Settings
    public float disappearAfterSeconds;        
    

    [SerializeField]
    private int damage;
    

    
    private void Awake(){
        //Get Player Instance
        Player pl = Player.getInstance();

        //Set Damage
        damage = pl.str * 2;          //Damage = 2 * STR //Can be changed to whatever is your liking
        
        //Pick correct image to display and correct offset to use for position
        Vector3 offset;
        switch(pl.lastFacedDirection){
            case Player.Direction.Up:
            spr.sprite = WasserHiebUpwards;
            offset = Vector3.up;
            break;

            case Player.Direction.Left:
            spr.sprite = WasserHiebLeft;
            offset = Vector3.left;
            break;

            case Player.Direction.Right:
            spr.sprite = WasserHiebRight;
            offset = Vector3.right;
            break;

            default:
            spr.sprite = WasserHiebDownwards;
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
            TestEnemy enemyScript = other.GetComponent<TestEnemy>();
            enemyScript.takeDamage(DamageType.Wasser, damage);

        }
    }
}
