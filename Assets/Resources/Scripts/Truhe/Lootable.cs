using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable : MonoBehaviour
{  
    public Item item;
    public bool isOpen;
    private SpriteRenderer spr;
    public Sprite openedSprite;
    void Start(){
        isOpen = false;
        spr = gameObject.GetComponent<SpriteRenderer>();
    }
    public void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject != null && col.tag == "Player" && !isOpen){
                GameObject OpenPrefab = Resources.Load<GameObject>("Prefabs/open");
                ItemDropController.createItemDropWithOffset(transform,item,1);
                /*Destroy(gameObject);
                Instantiate(OpenPrefab, transform.position, Quaternion.identity);*/
                isOpen = true;
                spr.sprite = openedSprite;
        }
    }    
}
