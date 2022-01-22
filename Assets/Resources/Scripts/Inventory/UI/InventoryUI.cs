using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Animator animator;
    public TextMeshProUGUI inventoryTitle;
    public GameObject inventorySlotParent;
    private InventorySlotController[] inventorySlotsUI;
    public GameObject equipmentSlotParent;
    private InventorySlotController[] equipmentSlotsUI;
    public GameObject skillSlotsParent;
    private InventorySlotController[] skillSlotsUI;
    private Sprite[] skillSlotIcons;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI itemStatsText;
    public GameObject bottomBar;
    private TextMeshProUGUI unequipAllText;
    private TextMeshProUGUI dropItemText;
    private TextMeshProUGUI switchToSkillsText;

    private Inventory inventory;
    private bool isVisible;
    private bool itemSelectionActive;
    private void Awake(){
        inventorySlotsUI = inventorySlotParent.GetComponentsInChildren<InventorySlotController>();
        equipmentSlotsUI = equipmentSlotParent.GetComponentsInChildren<InventorySlotController>();
        skillSlotsUI     =    skillSlotsParent.GetComponentsInChildren<InventorySlotController>();
        
        TextMeshProUGUI[] infoTextsUI = bottomBar.GetComponentsInChildren<TextMeshProUGUI>();
        unequipAllText                = infoTextsUI[0];
        dropItemText                  = infoTextsUI[1];
        switchToSkillsText            = infoTextsUI[2];

        #region GetAllSkillIconsDueToBadEnumDecisionEarlier
        //This could be reworked with a SkillClass
        skillSlotIcons = new Sprite[9];
        skillSlotIcons[0] = Resources.Load<Sprite>("Sprites/SkillTreeSprites/AimedArrow-bw");
        skillSlotIcons[1] = Resources.Load<Sprite>("Sprites/SkillTreeSprites/rage");
        skillSlotIcons[2] = Resources.Load<Sprite>("Sprites/SkillTreeSprites/Chainlightning");
        skillSlotIcons[3] = Resources.Load<Sprite>("Sprites/SkillTreeSprites/IceArrow");
        skillSlotIcons[4] = Resources.Load<Sprite>("Sprites/SkillTreeSprites/SwordVortex");
        skillSlotIcons[5] = Resources.Load<Sprite>("Sprites/SkillTreeSprites/Ice-ground");
        skillSlotIcons[6] = Resources.Load<Sprite>("Sprites/SkillTreeSprites/FireArrow");
        skillSlotIcons[7] = Resources.Load<Sprite>("Sprites/SkillTreeSprites/FrozenStrike");
        skillSlotIcons[8] = Resources.Load<Sprite>("Sprites/SkillTreeSprites/Fireball");
        Debug.Log("Created Array!");
        
        #endregion
        }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        //Try to get inventory instance, if it fails wait half a second and try again.
        //The code so far has *never* failed to get inventory instance, but I am worried about Unity running 2 scripts at the same time that have timing dependency on each other, so here's a fix for a hopefully never appearing problem.
        while(inventory == null){
            inventory = Inventory.getInstance();
            yield return new WaitForSeconds(0.5f);
        }
        //Subscribe our function "UpdateUI" to InventoryChangedEvent, triggering it everytime the Event triggers. 
        inventory.onInventoryChangedCallback += UpdateUI;
        //Subscribe our function "UpdateSkillSelectionUI" to SkillTreeChanged
        SkillTree.getInstance().onSkillTreeChangedCallback += UpdateSkillsUnlockedUI;
        //First Update is free on the house
        UpdateUI();
        //Inventory should start invisible
        isVisible = false;
        itemSelectionActive = true;
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.I)){
            switchVisibility();
        }
        if(isVisible && Input.GetKeyDown(KeyCode.V)){
            switchSelectionMode();
        }
    }
    private void switchVisibility(){
        isVisible = !isVisible;
        animator.SetBool("isOpen", isVisible);
    }

    private void switchSelectionMode(){
        itemSelectionActive = !itemSelectionActive;
        if(itemSelectionActive){
            inventoryTitle.SetText("Inventar");
            unequipAllText.SetText("[X] Unequip All");
            dropItemText.SetText("[C] Drop Item");
            switchToSkillsText.SetText("[V] Switch to Skills");
            }
        else{
            inventoryTitle.SetText("Skills");
            unequipAllText.SetText("");
            dropItemText.SetText("");
            switchToSkillsText.SetText("[V] Switch to Inventory");
            }
        animator.SetBool("selectSkills",!itemSelectionActive);
    }

    #region UpdateUI
    void UpdateUI(){
        if(itemSelectionActive){UpdateItemSelectionUI();}
        else{UpdateSkillSelectionUI();}
    }

    void UpdateItemSelectionUI(){
        Item[] items = inventory.getItems();
        int selectedItemIndex = inventory.getSelectedItemIndex();
        //Update Border of Slots
        for (int i = 0; i < inventorySlotsUI.Length; i++){
            inventorySlotsUI[i].setItem(items[i]);
            inventorySlotsUI[i].beSelected(i == selectedItemIndex);    
        }
        //Update Text
        Item selectedItem = items[selectedItemIndex];
        if (selectedItem == null){
            itemNameText.SetText("");
            itemDescriptionText.SetText("");
            itemStatsText.SetText("");
        }
        else{
            itemNameText.SetText(selectedItem.itemName);
            itemDescriptionText.SetText(selectedItem.description);
            itemStatsText.SetText(selectedItem.getStatsAsFormattedString());
        }
    }

    void UpdateSkillSelectionUI(){
        int selectedSkillSlot = inventory.getSelectedSkillSlot();
        for(int i = 0; i < skillSlotsUI.Length; i++){
            skillSlotsUI[i].beSelected(selectedSkillSlot == i);
        }
    }

    void UpdateSkillsUnlockedUI(){
        Player pl = Player.getInstance();
        if(pl.ScharfschussLearned)      {skillSlotsUI[0].setSpriteAndEnable(skillSlotIcons[0]);}
        if(pl.RageLearned)              {skillSlotsUI[1].setSpriteAndEnable(skillSlotIcons[1]);}
        if(pl.KettenblitzLearned)       {skillSlotsUI[2].setSpriteAndEnable(skillSlotIcons[2]);}
        if(pl.WasserpfeilhagelLearned)  {skillSlotsUI[3].setSpriteAndEnable(skillSlotIcons[3]);}
        if(pl.ElektrowirbelLearned)     {skillSlotsUI[4].setSpriteAndEnable(skillSlotIcons[4]);}
        if(pl.WasserflaecheLearned)     {skillSlotsUI[5].setSpriteAndEnable(skillSlotIcons[5]);}
        if(pl.FeuerpfeilLearned)        {skillSlotsUI[6].setSpriteAndEnable(skillSlotIcons[6]);}
        if(pl.WasserhiebLearned)        {skillSlotsUI[7].setSpriteAndEnable(skillSlotIcons[7]);}
        if(pl.FeuerballLearned)         {skillSlotsUI[8].setSpriteAndEnable(skillSlotIcons[8]);}
    }

    #endregion

    public bool getVisibility(){
        return isVisible;
    }
    public bool getItemSelectionActive(){
        return itemSelectionActive;
    }

    public Sprite[] getAbilityIcons(){
        return skillSlotIcons;
    }
    
}
