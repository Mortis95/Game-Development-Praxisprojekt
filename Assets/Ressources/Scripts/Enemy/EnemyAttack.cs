using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private bool playerFound = false;
    private GameObject player = null;
    private Player playerScript = null;
    private Transform playerTransform = null;
    private Enemy enemy = null;
    private Transform transform = null;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>();
        playerTransform = player.GetComponent<Transform>();
        enemy = GetComponent<Enemy>();
        transform = GetComponent<Transform>();
    }

    void Update()
    {
        if (!playerFound)
            return;

        var distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer < 4)
        {
            playerScript.currentHealth -= enemy.attack;
            playerScript.pushBack(transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerFound = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerFound = false;
        }
    }
}
