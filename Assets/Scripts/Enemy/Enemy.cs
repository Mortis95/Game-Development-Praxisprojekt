using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public int maxHealth;
    public HealthBar healthBar;
    public int exp;
    public int attack;
    public GameObject[] droppableItems;
    public List<DamageType> weaknesses;

    GameObject itemDrop;
    int currentHealth;

    private void Start()
    {
        if (droppableItems.Length != 0)
        {
            itemDrop = droppableItems[Random.Range(0, droppableItems.Length)];
        } else
        {
            itemDrop = null;
        }
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDmg(int damage, DamageType type)
    {
        if (weaknesses != null && weaknesses.Contains(type))
        {
            damage += damage * 30 / 100; 
        }
        currentHealth -= damage;
        healthBar.SetSlider(currentHealth);
        if (currentHealth <= 0)
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<Player>().addExp(exp);
            Destroy(gameObject);
        }
    }
}