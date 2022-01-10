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
        Debug.Log(stackSizeText);

        //Make sure to have default look on Awake
        slotBackground.color = Color.white;
        slotDisplay.enabled = false;
        stackSizeText.SetText("");

    }
    public void setItem(Item item){
        slotDisplay.sprite = item.itemSprite;
        slotDisplay.enabled = true;
        if(item.isStackable){
            stackSizeText.SetText(item.getAmount().ToString());
        } else {
            stackSizeText.SetText("");
        }

    }

    public void clearItem(){
        slotDisplay.sprite = null;
        stackSizeText.SetText("");
    }

    public void beSelected(){
        slotBackground.color = Color.red;
    }

    public void beUnselected(){
        slotBackground.color = Color.white;
    }

}
