using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScharfSchussScript : MonoBehaviour
{
    public float End;
    public float speed = 50f;
    public Rigidbody2D myRigidbody;
    private void Awake()
    {
        Player pl = Player.getInstance();
        Vector3 offset;
        switch(pl.lastFacedDirection)
        {
            case Player.Direction.Up:
            myRigidbody.velocity = new Vector2(0,1) * speed;
            transform.Rotate(0, 0, 90);
            offset = Vector3.up;
            break;

            case Player.Direction.Left:
            myRigidbody.velocity = new Vector2(-1,0) * speed;
            offset = Vector3.left;
            break;

            case Player.Direction.Right:
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
            TestEnemy enemyScript = other.GetComponent<TestEnemy>();
            enemyScript.takeDamage(DamageType.Blitz, 5);
            Destroy(gameObject);

        }
    }
    
}
