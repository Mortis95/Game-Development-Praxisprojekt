using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    private Item item;
    private SpriteRenderer spriteRenderer;

    public static ItemWorld SpawnItemWorld(Vector2 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity );

        ItemWorld itemworld = transform.GetComponent<ItemWorld>();
        itemworld.SetItem(item);
        itemworld.spriteRenderer.sortingLayerName = "on floor";

        return itemworld;
    }

    public static ItemWorld DropItem(Vector2 dropPos,Item item)
    {
        ItemWorld Itemworld = SpawnItemWorld(dropPos +new Vector2(1,1) * Random.Range(1.0f, 1.5f), item);
        return Itemworld;
    } 


    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    public Item getItem()
    {
        return item;
    }
}
