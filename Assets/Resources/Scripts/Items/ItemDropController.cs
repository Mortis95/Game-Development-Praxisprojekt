using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropController : MonoBehaviour{
    public SpriteRenderer spr;
    public Item item;

    public static void createItemDrop(Transform tr, Item item){
        GameObject itemDropPrefab = Resources.Load<GameObject>("Prefabs/ItemDrop");
        ItemDropController itemControl = itemDropPrefab.GetComponent<ItemDropController>();
        itemControl.item = item;
        Vector3 offset = Vector3.right * 5f;
        offset = Vector3Extension.RotatePointAroundPivot(offset, Vector3.zero, new Vector3(0f,0f, Random.Range(0f,360f)));
        itemDropPrefab.transform.position = tr.position + offset;
        Instantiate(itemDropPrefab);
    }

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
        if(col.gameObject != null && col.tag == "Player" && col.gameObject.name.Equals("Player")){
            Player pl = col.gameObject.GetComponent<Player>();
            Debug.Log("Player:" + pl + " Inventory:" + pl.inventory + " Item:" + item);
            bool itemWasPickedUp = pl.inventory.addItem(Instantiate(item));
            if (itemWasPickedUp){
                Destroy(gameObject);
            }
        }
    }
}
