using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropTable : MonoBehaviour{
    public ItemDrop[] dropTable;
    public List<Item> getDrops(){
        //Create new List of Drops
        List<Item> drops = new List<Item>();

        //Roll each drop in droptable
        foreach (ItemDrop drop in dropTable){
            float chance = Random.Range(0.01f, 99.99f);
            Debug.Log("Rolling for " + drop.toDrop.name + " Chance: " + chance + " ItemDropRate: " + drop.percentageChanceToDrop + " -> Will it drop: " + (chance <= drop.percentageChanceToDrop));
            if(chance <= drop.percentageChanceToDrop){
                drops.Add(drop.toDrop);
            }
        }

        //Return the List
        return drops;
    }
}


[System.Serializable]
public class ItemDrop{
    [Tooltip("The Item to drop.")]
    public Item toDrop;
    [Tooltip("The percentage chance of this Item to drop. Set to 100 for a guaranteed drop."), Range(0.01f,100f)]
    public float percentageChanceToDrop;
}
