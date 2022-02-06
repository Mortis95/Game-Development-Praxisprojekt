using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebAttack : MonoBehaviour
{
    public int speed = 4;
    public int removeAfterUnits = 500;



    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, Player.getInstance().transform.position, speed * Time.deltaTime);
        if (removeAfterUnits == 0)
        {
            Debug.Log("m�sste kaputt gehen");
            Destroy(gameObject);
        }
        Debug.Log(removeAfterUnits);
        removeAfterUnits -= 1;
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
