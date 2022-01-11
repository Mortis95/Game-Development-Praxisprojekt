using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class handles all the logic of the inventory
public class Equipment : MonoBehaviour
{
    #region  Singleton
    private static Equipment instance;
    private void Awake(){
        if(instance != null){
            Debug.LogWarning("Two Instances of Equipment have been found. Something went wrong!");
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public static Equipment getInstance(){return instance;}
    #endregion 

    public Inventory inventory;
    public Weapon equippedWeapon;
    public Shield shieldInHand;
    public Consumable consumableInHand;
    public Armor equippedArmor;
    public Ability equippedAbility;
    public delegate void OnEquipmentChanged();
    public OnEquipmentChanged onEquipmentChangedCallback;


    //This method will equip the given item in (hopefully) the correct slot for it.
    //Note: The code appears messy, because of how complex the problem is:
    //We have 4 Equipment Slots: Weapon, Shield/Consumable, Armor, Skill
    //We can ignore Skill for now, since it is not an item and equipped elsewhere.
    //Since the inventory only carries Item references, we need to correctly identify the given Item via the ItemType enum
    //and then correctly cast down to the correct derived class.
    //Also since the second Slot is meant for EITHER a shield, OR a Consumable, the code is a bit messy here.
    //The intention is that the player can choose to equip a shield for more defense, or a Consumable for mobile healing.
    //Since the whole Operation is basically a "Swap" with the inventory, we also need to return whatever oldItem we are replacing.
    public Item equipItem(Item item){
        if(item == null){return null;}
        switch(item.itemType){
            case ItemType.Weapon:
                Weapon oldWeapon = equippedWeapon;
                equippedWeapon = (Weapon) item;
                invokeCallback();
                return oldWeapon;
            case ItemType.Shield:
                if(shieldInHand == null){
                    Consumable oldConsumable = consumableInHand;
                    consumableInHand = null;
                    shieldInHand = (Shield) item;
                    invokeCallback();
                    return oldConsumable;
                } else {
                    Shield oldShield = shieldInHand;
                    consumableInHand = null;
                    shieldInHand = (Shield) item;
                    invokeCallback();
                    return oldShield;
                }
            case ItemType.Consumable:
                if(consumableInHand == null){
                    Shield oldShield = shieldInHand;
                    shieldInHand = null;
                    consumableInHand = (Consumable) item;
                    invokeCallback();
                    return oldShield;
                } else {
                    Consumable oldConsumable = consumableInHand;
                    shieldInHand = null;
                    consumableInHand = (Consumable) item;
                    invokeCallback();
                    return oldConsumable;
                }
            case ItemType.Armor:
                Armor oldArmor = equippedArmor;
                equippedArmor = (Armor) item;
                invokeCallback();
                return oldArmor;
        }
        //Item can't be equipped, just return it to inventory.
        return item;    
    }

    public void invokeCallback(){
        if(onEquipmentChangedCallback != null){onEquipmentChangedCallback.Invoke();}
    }
}
