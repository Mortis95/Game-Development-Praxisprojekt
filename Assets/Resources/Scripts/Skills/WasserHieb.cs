using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasserHieb : MonoBehaviour
{

    //Images
    private SpriteRenderer spr;
    public Sprite[] animationCycle;
    private float secondsPerImage;

    //Settings
    public float disappearAfterSeconds;        
    

    [SerializeField]
    private int damage;
    private int playerStrength;
    

    
    private void Awake(){
        //Get all important components
        spr = gameObject.GetComponent<SpriteRenderer>();
        secondsPerImage = disappearAfterSeconds / animationCycle.Length;
        Debug.Log(secondsPerImage);

        //Get Player Instance
        Player pl = Player.getInstance();

        //Set Damage
        playerStrength = pl.getStrength();
        damage = (int)((float) playerStrength * pl.getSkillDamageMultiplier());
        
        //Play Sound
        AudioManager.getInstance().PlaySound("SkillSchwertWasserhieb");

        //Pick correct rotation to display and correct offset to use for position
        Vector3 offset;
        switch(pl.lastFacedDirection){
            case Direction.Up:
                offset = Vector3.up;
                transform.Rotate(0,0,180);
                break;

            case Direction.Left:
                transform.Rotate(0,0,-90);
                offset = Vector3.left;
                break;

            case Direction.Right:
                transform.Rotate(0,0,90);
                offset = Vector3.right;
                break;

            default:
                //transform.Rotate(0,0,0);      //Sprite is default at rotation down
                offset = Vector3.down;
                break;
        }

        //Set Location of Object
        transform.position = pl.transform.position + offset * 1.5f;


        //Start Animation loop
        StartCoroutine(animationLoop());

        //Destroy Sprite after seconds
        Destroy(gameObject, disappearAfterSeconds);
    }

    IEnumerator animationLoop(){
        int sprIndex = 0;
        spr.sprite = animationCycle[sprIndex];
        float timeSinceLastSprite = Time.time;
        while(true){
            if(Time.time - timeSinceLastSprite >= secondsPerImage){
                timeSinceLastSprite = Time.time;
                sprIndex++;
                if(sprIndex < animationCycle.Length){
                    spr.sprite = animationCycle[sprIndex];
                } else{
                    yield break;    //Animation finished, stop updating and exit out of loop
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }


    void OnTriggerEnter2D(Collider2D col){
        Debug.Log("Collision with:" + col.name);
        GameObject other = col.gameObject;
        if(other != null && other.tag == "Enemy"){
            EnemyManager enemyScript = other.GetComponent<EnemyManager>();
            enemyScript.takeDamage(DamageType.Wasser, damage);
            enemyScript.getKnockback(transform.position, playerStrength);

        }
    }
}
