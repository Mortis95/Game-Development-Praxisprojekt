using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasserPfeileScript : MonoBehaviour
{
    fightable EnemyVar;
    List<EnemyMovement> enemies = new List<EnemyMovement>();
    // Start is called before the first frame update
    
    
    private float timer = 0f;
    private float waitTime = 1f;

    bool rdy = false;

    void Update()
    {
        if(timer < 0)
        {
            rdy = true; 
            timer = waitTime;         
        }
        timer -= Time.deltaTime;
    }
    
    void Start()
    {
       Destroy(gameObject, 6.0f);
       AudioManager.getInstance().PlaySound("SkillBogenWasserpfeil");
    }
   


    void OnTriggerStay2D(Collider2D other)
    {
        GameObject collisionObject = other.gameObject;
        if(collisionObject!=null)
        {
            if(collisionObject.tag == "Enemy")  
            {         
                if(rdy)
                {
                EnemyVar = collisionObject.GetComponent<fightable>(); 
                EnemyVar.TakeDMG(10);
                rdy = false;
                }                                
            }
        }
                              
        
        
    }


}
