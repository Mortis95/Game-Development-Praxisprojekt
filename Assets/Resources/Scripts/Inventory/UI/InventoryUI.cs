using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryUIParent;
    public GameObject inventorySlotParent;
    public Animator animator;
    private InventorySlotController[] inventorySlotsUI;
    public GameObject equipmentSlotParent;
    private InventorySlotController[] equipmentSlotsUI;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;

    private Inventory inventory;
    private bool isVisible;
    private void Awake(){
        inventorySlotsUI = inventorySlotParent.GetComponentsInChildren<InventorySlotController>();
        equipmentSlotsUI = equipmentSlotParent.GetComponentsInChildren<InventorySlotController>();
        
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        //Try to get inventory instance, if it fails wait a second and try again.
        //The code so far has *never* failed to get inventory instance, but I am worried about Unity running 2 scripts at the same time that have timing dependency on each other, so here's a fix for a hopefully never appearing problem.
        while(inventory == null){
            inventory = Inventory.getInstance();
            yield return new WaitForSeconds(1f);
        }
        //Subscribe our function "UpdateUI" to InventoryChangedEvent, triggering it everytime the Event triggers. 
        inventory.onInventoryChangedCallback += UpdateUI;
        //First Update is free on the house
        UpdateUI();
        //Inventory should start invisible
        isVisible = false;
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.I)){
            switchVisibility();
        }
    }
    private void switchVisibility(){
        isVisible = !isVisible;
        animator.SetBool("isOpen", isVisible);
        //inventoryUIParent.SetActive(isVisible);
        /* if(isVisible){
            //Put Inventory in the middle of the screen and be visible
            transform.position = inventoryUIParent.transform.position;
        } else {
            //Put it VERY FAR offscreen, like SUPER FAR, even the BIGGEST BROADEST Monitor shouldn't be able to see that thing
            transform.position = new Vector3(5000,5000,0);
        } */
    }

    void UpdateUI(){
        Item[] items = inventory.getItems();
        int selectedItemIndex = inventory.getSelectedItemIndex();
        //Update Border of Slots
        for (int i = 0; i < inventorySlotsUI.Length; i++){
            inventorySlotsUI[i].setItem(items[i]);
            inventorySlotsUI[i].beSelected(i == selectedItemIndex);    
        }
        //Update Text
        Item selectedItem = items[selectedItemIndex];
        if (selectedItem == null){itemNameText.SetText("");itemDescriptionText.SetText("");}
        else{itemNameText.SetText(selectedItem.itemName);itemDescriptionText.SetText(selectedItem.description);}

    }

    public bool getVisibility(){
        return isVisible;
    }
}
