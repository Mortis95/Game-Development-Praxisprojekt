using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="New Item", menuName = "Item")] 
public class Item : ScriptableObject{
   public ItemType itemType;
   public Sprite itemSprite;
   public string itemName;
   public string description;
   public bool isStackable;
   public int maxStackSize;
   private int amount;

   public virtual void Use(){
       Debug.Log("Using Item: " + itemName);
   }

   public int getAmount(){
       return amount;
   }
   
}
