using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeuerBallScript : MonoBehaviour
{
    public float End;
    public float speed = 12f;
    public float Damage;
    public float SplashRange = 10;

    public Rigidbody2D myRigidbody;
    private void Awake()
    {
        Player pl = Player.getInstance();
        Vector3 offset;
        switch(pl.lastFacedDirection)
        {
            case Direction.Up:
            myRigidbody.velocity = new Vector2(0,1) * speed;
            transform.Rotate(0, 0, 90);
            offset = Vector3.up;
            break;

            case Direction.Left:
            myRigidbody.velocity = new Vector2(-1,0) * speed;
            transform.Rotate(0, 0, 180);
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
    

    void OnTriggerEnter2D(Collider2D col)
    {   
        var TestEnemy = col.gameObject;
        if(TestEnemy.tag == "Enemy")
        {
            var hitColliders = Physics2D.OverlapCircleAll(transform.position, SplashRange);
            foreach(var hitCollider in hitColliders)
            {
                var enemy = hitCollider.GetComponent<EnemyManager>();
                if(enemy)
                {
                    var closestPoint = hitCollider.ClosestPoint(transform.position);
                    var distance = Vector3.Distance(closestPoint, transform.position);

                    var damagePercent = Mathf.InverseLerp(SplashRange, 0, distance);
                    enemy.takeDamage(DamageType.Feuer, (int)(damagePercent * Damage)); 
                    Destroy(gameObject);
                }
        }   }       
    }
}
