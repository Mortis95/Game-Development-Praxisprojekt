using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasserflaecheScript : MonoBehaviour
{
    EnemyMovement EnemyVar;
    List<EnemyMovement> enemies = new List<EnemyMovement>();
    // Start is called before the first frame update
    void Start()
    {
       Destroy(gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        GameObject collisionObject = other.gameObject;

        if(collisionObject.tag == "Enemy")  
        {
               EnemyVar = collisionObject.GetComponent<EnemyMovement>();
               EnemyVar.speed =1;
               enemies.Add(EnemyVar);
        }
        Debug.Log("Kollision ");
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collisionObject = collision.gameObject;

        if(collisionObject.tag =="Enemy")  
        {
               EnemyVar = collisionObject.GetComponent<EnemyMovement>();
               EnemyVar.speed =3;
        }
        Debug.Log("Kollision ende");
    }

    void OnDestroy()
    {
        foreach(EnemyMovement enemy in enemies) 
        {
            enemy.speed=3;
        }
    }

}