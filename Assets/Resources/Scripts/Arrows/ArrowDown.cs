using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDown : MonoBehaviour
{
   public float speed=20f;
    public Rigidbody2D rb;

    public enum Direction
   {
       Left,
       Right,
       Up,
       Down

   } 
    Direction direction;

    void Start()
    {
       // rb.velocity = transform.right * speed;
       rb.velocity = new Vector2(0,-1) * speed;
       transform.Rotate(0, 0, -90);

    }

    

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
            fightable enemy = hitInfo.GetComponent<fightable>();
            if(enemy != null)
            {
                enemy.TakeDMG(10);
                Debug.Log("Getroffen");
            Destroy(gameObject);
            }
            
        }
        
    }
