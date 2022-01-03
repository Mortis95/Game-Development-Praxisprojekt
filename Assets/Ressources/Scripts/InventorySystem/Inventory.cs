using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory 
{
    private List<Item> itemList;
    public event EventHandler OnItemListChanged;
    private Action<Item> useItemAction;
    public int capacity=20;
    public int inhalt=0;

    public Inventory(Action<Item> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemList= new List<Item>();
        //Debug.Log("Inventar steht");
        //addItem(new Item{ itemtype = Item.Itemtype.Sword, amount=1});   //bsp item einfügen
        //addItem(new Item{ itemtype = Item.Itemtype.HealPotion, amount=1});   //bsp item einfügen

        //Debug.Log(itemList.Count);
    }

    public void addItem(Item newItem)
    {
        if(newItem.IsStackable())
        {
            bool isThere = false;
            foreach(Item inventoryItem in itemList)
            {
                if(inventoryItem.itemtype == newItem.itemtype)
                {
                    inventoryItem.amount += newItem.amount;
                    isThere=true;
                }
                
            }
            if(!isThere)
            {
                itemList.Add(newItem);
                inhalt++;
            }
        }
        else
        {
            itemList.Add(newItem);
            inhalt++;
        }
        OnItemListChanged?.Invoke(this,EventArgs.Empty);

    }

    

    public void RemoveItem(Item item)
    {
        if(item.IsStackable())
        {
            Item itemInInventory = null;
            foreach(Item inventoryitem in itemList)
            {
                if(inventoryitem.itemtype == item.itemtype)
                {
                    inventoryitem.amount -= item.amount;
                    itemInInventory = inventoryitem;
                }
            }
            if(itemInInventory != null && itemInInventory.amount <= 0)
            {
                itemList.Remove(itemInInventory);
                inhalt--;
            }
            

        }
        else
        {
            itemList.Remove(item);
            inhalt--;
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);

    }

    public List<Item> getItemList()
    {
        return itemList;
    }

    public void UseItem(Item item){
        useItemAction(item);

    }


}
