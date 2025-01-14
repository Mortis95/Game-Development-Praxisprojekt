using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elektrowirbel : MonoBehaviour
{
    //Publics zum rumprobieren
    public float rotatePerStep;
    public int rotateEveryNFrames;
    public float disappearAfterSeconds;

    //Privates die sich das Script holen muss
    private int rotateFrameCount;
    private int damage;
    private Transform stuckToPlayer;
    private int playerSTR;

    private void Awake(){
        Player player = Player.getInstance();
        playerSTR = player.getStrength();
        damage = (int)((float) playerSTR * player.getSkillDamageMultiplier());                //Provisorischer Wert = 2 * STR
        rotateFrameCount = rotateEveryNFrames;
        stuckToPlayer = player.transform;
        AudioManager.getInstance().PlaySound("SkillSchwertElektrowirbel");
        Destroy(gameObject,disappearAfterSeconds);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rotateFrameCount <= 0){
            rotateFrameCount = rotateEveryNFrames;
            transform.Rotate(0.0f,0.0f,rotatePerStep);
        }
        rotateFrameCount -= 1;
        transform.position = stuckToPlayer.position;
        
    }

    void OnTriggerEnter2D(Collider2D col){
        Debug.Log("Collision with:" + col.name);
        GameObject other = col.gameObject;
        if(other != null && other.tag == "Enemy"){
            EnemyManager enemyScript = other.GetComponent<EnemyManager>();
            enemyScript.takeDamage(DamageType.Blitz, damage);
            enemyScript.getKnockback(transform.position, playerSTR);

        }
    }


}
