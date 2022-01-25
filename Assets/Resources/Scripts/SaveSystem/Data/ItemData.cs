using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData : MonoBehaviour
{
    public string itemType;
    public string itemName;
    public string description;
    public bool isStackable;
    public int maxStackSize;
    public int amount;
    public string spritePath;


    public ItemData(Item item)
    {
        itemType = item.itemType.ToString();
        itemName = item.itemName;
        description = item.description;
        isStackable = item.isStackable;
        maxStackSize = item.maxStackSize;
        amount = item.amount;
        spritePath = item.itemSprite.name;
    }
}
