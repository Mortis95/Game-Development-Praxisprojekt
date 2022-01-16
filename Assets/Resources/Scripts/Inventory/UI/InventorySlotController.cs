using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotController : MonoBehaviour{
    public Image slotBackground;
    public Image slotDisplay;
    private TextMeshProUGUI stackSizeText;

    private void Awake(){
        //Get Text Object (for some reason couldn't be assigned in Unity Inspect)
        stackSizeText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        //Debug.Log(stackSizeText);

        //Make sure to have default look on Awake
        slotBackground.color = Color.white;
        slotDisplay.enabled = false;
        stackSizeText.SetText("");

    }
    public void setItem(Item item){
        if(item == null){
            clearItem();
            return;
        }
        slotDisplay.sprite = item.itemSprite;
        slotDisplay.enabled = true;
        if(item.isStackable){
            stackSizeText.SetText(item.getAmount().ToString());
        } else {
            stackSizeText.SetText("");
        }

    }

    public void setSpriteAndEnable(Sprite spr){
        this.slotDisplay.sprite = spr;
        this.slotDisplay.enabled = true;
    }

    public void clearItem(){
        slotDisplay.sprite = null;
        slotDisplay.enabled = false;
        stackSizeText.SetText("");
    }

    public void beSelected(bool selected){
        if(selected){slotBackground.color = Color.red;}
        else{slotBackground.color = Color.white;}
        
    }

    public void setDisplayEnabled(bool enabled){
        slotDisplay.enabled = enabled;
    }
}
