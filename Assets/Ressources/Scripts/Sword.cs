using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int attackDmg;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GameObject player = GameObject.FindWithTag("Player");
            var dmg = attackDmg + player.GetComponent<Player>().atk;
            collision.gameObject.GetComponent<Enemy>().TakeDmg(dmg);
        }
    }
}
