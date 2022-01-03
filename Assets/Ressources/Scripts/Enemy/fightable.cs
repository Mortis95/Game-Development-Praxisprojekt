using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fightable : MonoBehaviour
{

    //public Animator animator;
    public int maxHealth = 100;
    int currentHealth;
    //LevelSystem Komponente
    public int expValue=100; //wie viel xp bringt das töten?
    [SerializeField] Player player;
    
    void Start()
    {
        currentHealth=maxHealth;
    }

    public void TakeDMG(int dmg)
    {
        currentHealth -= dmg;
        //animator.SetTrigger("Hurt");

        if(currentHealth <=0){
            die();
        }
    }

    void die()
    {
        //animator.SetBool("isDead", true);
        Debug.Log("Enemy died");

        GetComponent<Collider2D>().enabled= false;
        this.enabled = false;

        player.addExp(expValue);

        Destroy(this);
        foreach (Transform child in transform) 
        {
            Destroy(child.gameObject);
        }

    }
}

//nötige attack methode im Player

/*  void Attack()
    {
        //Animation
        animator.SetTrigger("Attack");
        //Attack enemies
        Collider2D [] hitEnemies= Physics2D.OverlapCircleAll(attackpoint.position, attackrange, enemyLayers);

        //dmg to enemies
        foreach(Collider2D enemy in hitEnemies){
            Debug.Log("Hit"+enemy.name);
            enemy.GetComponent<Enemy>().TakeDMG(Schaden);
        }
    } */
    
