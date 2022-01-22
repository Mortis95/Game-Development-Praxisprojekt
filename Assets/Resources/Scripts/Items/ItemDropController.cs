using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropController : MonoBehaviour{
    public SpriteRenderer spr;
    public Item item;


    //Creates Item Drop at given location + a given offset.
    //The Item will spawn at a random position in a circle around the given spot, with radius r = offsetLength
    public static void createItemDropWithOffset(Transform tr, Item item, float offsetLength){
        GameObject itemDropPrefab = Resources.Load<GameObject>("Prefabs/ItemDrop");
        ItemDropController itemControl = itemDropPrefab.GetComponent<ItemDropController>();
        itemControl.item = item;
        Vector3 offset = Vector3.right * offsetLength;
        offset = Vector3Extension.RotatePointAroundPivot(offset, Vector3.zero, new Vector3(0f,0f, Random.Range(0f,360f)));
        itemDropPrefab.transform.position = tr.position + offset;
        Instantiate(itemDropPrefab);
    }

    //Creates Item Drop at given location
    public static void createItemDrop(Transform tr, Item item){
        GameObject itemDropPrefab = Resources.Load<GameObject>("Prefabs/ItemDrop");
        ItemDropController itemControl = itemDropPrefab.GetComponent<ItemDropController>();
        itemControl.item = item;
        itemDropPrefab.transform.position = tr.position;
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

    void Start(){
        StartCoroutine(floatAnimation());
    }


    IEnumerator floatAnimation(){
        float angleHelp = 0;
        float baseX = transform.position.x;
        float baseY = transform.position.y;
        while(true){
            angleHelp += 2 *  Time.fixedDeltaTime;
            angleHelp = angleHelp % 360;
            transform.position = new Vector3(baseX, baseY + Mathf.Sin(angleHelp), 0);
            yield return new WaitForFixedUpdate();
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
