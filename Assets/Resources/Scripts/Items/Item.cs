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
    [Range(0,100), Tooltip("This value will only be considered if Item is actually stackable.")]
    public int amount=1;        //Default amount = 1, only gets read and shown if Item isStackable

    public virtual void Use(){
       Debug.Log("Using Item: " + itemName);
    }

    public int getAmount(){
       return amount;
    }

    //Typical Equals Contract
    public override bool Equals(object other){
        if(other == null){return false;}
        Debug.Log("Other: " + other.GetType() + " This: " + this.GetType());
        if(other.GetType() != this.GetType())                   {return false;}
        Item otherItem = (Item) other;
        if(otherItem.itemType != this.itemType)                 {return false;}
        if(!otherItem.itemName.Equals(this.itemName))           {return false;}
        if(!otherItem.description.Equals(this.description))     {return false;}
        if(otherItem.isStackable != this.isStackable)           {return false;}
        if(otherItem.maxStackSize != this.maxStackSize)         {return false;}

        // Possible Idea: 
        // Downcast this and otherItem to the same ItemType and continue the Equals.
        // This *might* be interesting, since in our Inventory we only have "Item" References, not specific derived Classes References.
        // However, what is the chance that two Items of the same derived class exist, with the exact same name and description, but with different values?
        // That would only start happening once we include randomized loot in our game. Also let's be honest, it isn't the Item-Class's Job to compare two Weapons, if it was given two Items.
        // Therefore the Item Class will now consider these Items equal, as they most likely are.

        // If you want to compare two Weapons or any other derived class from this one, please implemented it in the derived class. 
        
        return true;
    }
  
   
}
