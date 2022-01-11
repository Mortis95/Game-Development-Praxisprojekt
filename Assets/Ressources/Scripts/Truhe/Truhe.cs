using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truhe : Collectable
{
    public Sprite emptyChest;
    public int muenzenAmount = 100;
    
    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            Debug.Log("Du hast " + muenzenAmount + " Münzen bekommen.");
        }
    }
}