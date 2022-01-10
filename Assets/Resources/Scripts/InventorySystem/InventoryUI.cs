using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventorySlotParent;
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
        while(inventory == null){
            inventory = Inventory.getInstance();
            yield return new WaitForSeconds(3f);
        }
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.I)){
            switchVisibility();
        }
    }
    private void switchVisibility(){
        isVisible = !isVisible;
        gameObject.SetActive(isVisible);
    }
}
