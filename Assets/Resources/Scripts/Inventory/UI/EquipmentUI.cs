using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour{

    public GameObject equipmentSlotsParent;
    public InventorySlotController[] equipmentSlots;
    public Equipment equipment;
    public InventoryUI invUI;

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
        Ability equippedAbility = equipment.equippedAbility;
        if(!equippedAbility.Equals(Ability.NoAbilityEquipped)){
            Sprite abilitySprite = invUI.getAbilityIcons()[Convert.ToInt32(equippedAbility)];   //Convert enum back to index, which should match up with the correct sprite in the Sprite-Array
            equipmentSlots[3].setSpriteAndEnable(abilitySprite);
        }
    }    
}
