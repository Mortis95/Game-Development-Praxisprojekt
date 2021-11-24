using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    //Schaden
    public int damagePoint = 1;
    public float pushForce = 2.0f;

    //Upgrade
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer; //damit wir sp�ter Aussehen der Waffe �ndern k�nnen

    //swing
    private float cooldown = 0.5f; // jede halbe Sekunde kann man Schwert schwingen
    private float lastSwing;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if(coll.tag == "Fighter") //Check: kollidiert die Waffe mit nem Gegner?
        {
            if (coll.name == "Player")
                return;


            Debug.Log(coll.name);
            
        }
    }

    private void Swing()
    {
        Debug.Log("Swing");
    }
}
