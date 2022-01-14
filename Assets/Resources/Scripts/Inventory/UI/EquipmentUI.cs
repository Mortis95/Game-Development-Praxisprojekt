using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour{

    public GameObject equipmentSlotsParent;
    public InventorySlotController[] equipmentSlots;
    public Equipment equipment;

    private void Awake(){
        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<InventorySlotController>();
        equipment.onEquipmentChangedCallback += UpdateEquipmentUI;
    }

    public void UpdateEquipmentUI(){
        equipmentSlots[0].setItem(equipment.equippedWeapon);
        if(equipment.shieldInHand != null){
            equipmentSlots[1].setItem(equipment.shieldInHand);
        } else {
            equipmentSlots[1].setItem(equipment.consumableInHand);
        }
        equipmentSlots[2].setItem(equipment.equippedArmor);
    }    
}
