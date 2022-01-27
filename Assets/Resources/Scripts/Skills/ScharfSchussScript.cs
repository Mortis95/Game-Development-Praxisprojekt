using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScharfSchussScript : MonoBehaviour
{
    public int EnemyHit = 0;
    public float End;
    public float speed = 50f;
    public Rigidbody2D myRigidbody;
    private void Awake()
    {
        Player pl = Player.getInstance();
        Vector3 offset;
        AudioManager.getInstance().PlaySound("SkillBogenScharfschuss");
        switch(pl.lastFacedDirection)
        {
            case Direction.Up:
            myRigidbody.velocity = new Vector2(0,1) * speed;
            transform.Rotate(0, 0, 90);
            offset = Vector3.up;
            break;

            case Direction.Left:
            myRigidbody.velocity = new Vector2(-1,0) * speed;
            offset = Vector3.left;
            break;

            case Direction.Right:
            myRigidbody.velocity = new Vector2(1,0) * speed;
            offset = Vector3.right;
            break;

            default:
            myRigidbody.velocity = new Vector2(0,-1) * speed;
            transform.Rotate(0, 0, -90);
            offset = Vector3.down;
            break;
        }
        Destroy(gameObject, End);
    }

    void OnTriggerEnter2D(Collider2D col){
        Debug.Log("Collision with:" + col.name);
        GameObject other = col.gameObject;
        if(other != null && other.tag == "Enemy"){
            EnemyManager enemyScript = other.GetComponent<EnemyManager>();
            enemyScript.takeDamage(DamageType.Blitz, 5);
            EnemyHit = EnemyHit + 1;
            if(EnemyHit >= 3){
                Destroy(gameObject);
            }

        }
    }
    
}
