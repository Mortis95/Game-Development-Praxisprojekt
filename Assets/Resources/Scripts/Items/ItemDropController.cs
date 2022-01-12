using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropController : MonoBehaviour{
    public SpriteRenderer spr;
    public Item item;

    private void Awake(){
        if(spr == null){
            spr = gameObject.GetComponent<SpriteRenderer>();
        }
        if(item == null){
            Debug.LogWarning("ItemDropController at GameObject: " + gameObject.name + " has NO assigned item. Please assign an Item first. You can create new Items by right-clicking in the Project window and selecting 'Create -> Items -> ...' and filling all necessary Information. This object will now self-destroy.");
            Destroy(gameObject);
        } else {
            spr.sprite = item.itemSprite;
            spr.color = Color.white;
        }
    }

    public void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject != null && col.tag == "Player"){
            Player pl = col.gameObject.GetComponent<Player>();
            bool itemWasPickedUp = pl.inventory.addItem(Instantiate(item));
            if (itemWasPickedUp){
                Destroy(gameObject);
            }
        }
    }
}
