using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebAttack : MonoBehaviour
{
    public int speed = 5;
    private void Start()
    {
        Vector3 toPlayer = (Player.getInstance().GetComponent<Transform>().transform.position).normalized;
        Vector2 toV2 = new Vector2(toPlayer.x, toPlayer.y);
        GetComponent<Rigidbody2D>().velocity = toPlayer * speed;
    }

    public int dmg = 15;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Enemy")
        {
            switch (collision.gameObject.tag)
            {
                case "Player":
                    Player.getInstance().takeDamage(dmg);
                    Debug.Log(Player.getInstance().currentHealthPoints);
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
