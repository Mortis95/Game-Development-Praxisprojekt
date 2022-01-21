using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    private static Inventory instance;
    public static Inventory getInstance(){
        return instance;
    }
    private void Awake(){
        if(instance != null){
            Debug.LogWarning("Something went wrong, 2 Inventory instances!!!");
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    //Other Scripts can subscribe to this event and then get notified of any change. LIKE THE UI FOR INSTANCE HAHAHAHA
    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    public InventoryUI inventoryUI;
    private Item[] items;
    private int itemSlotsPerRow;
    private int itemSlotsPerColumn;
    private int inventorySpace;
    private int selectedItemIndex;
    private Ability[] skills;       //Dirty enum array, better handled as a class, but not enough time for another rework.
    private int skillSlotsPerRow;
    private int skillSlotsPerColumn;
    private int skillSlotsTotal;
    private int selectedSkillSlot;
    public Equipment equipment;
    void Start(){
        //Setup Itemslots Numbers
        itemSlotsPerRow = 5;    //I'd like to dynamically get these numbers, but I can't be arsed to introduce even more spaghetti. Just keep this consistent with the UI please ty
        itemSlotsPerColumn = 4;
        inventorySpace = itemSlotsPerRow * itemSlotsPerColumn;
        selectedItemIndex = 0;

        //Setup SkillSlot Numbers
        skillSlotsPerRow = 3;
        skillSlotsPerColumn = 3;
        skillSlotsTotal = skillSlotsPerRow * skillSlotsPerColumn;
        selectedSkillSlot = 0;

        //Setup new Inventory
        items = new Item[inventorySpace];

        //Setup new SkillSlots
        #region CreatingBigEnumArray
        skills = new Ability[] {
            Ability.Scharfschuss,     Ability.Rage,          Ability.Kettenblitz, 
            Ability.Wasserpfeilhagel, Ability.Elektrowirbel, Ability.Wasserflaeche,
            Ability.Feuerpfeil,       Ability.Wasserhieb,    Ability.Feuerball
            };
        #endregion

        inventoryUI = GetComponentInChildren<InventoryUI>();
        }

    void Update(){
        if(inventoryUI.getVisibility()){
            if(inventoryUI.getItemSelectionActive()){processItemSelectionInput();} 
            else                                    {processSkillSelectionInput();}
        }
    }

    #region PlayerSelectionInput
    void processItemSelectionInput(){
        bool changed = false;
        if(Input.GetKeyDown(KeyCode.E)){items[selectedItemIndex] = equipment.equipItem(items[selectedItemIndex]);               changed=true; AudioManager.getInstance().PlaySound("UILockIn");}
        if(Input.GetKeyDown(KeyCode.W)){selectedItemIndex = betterModulo(selectedItemIndex - itemSlotsPerRow, inventorySpace);  changed=true; AudioManager.getInstance().PlaySound("UIChangeSelection");}
        if(Input.GetKeyDown(KeyCode.S)){selectedItemIndex = betterModulo(selectedItemIndex + itemSlotsPerRow, inventorySpace);  changed=true; AudioManager.getInstance().PlaySound("UIChangeSelection");}
        if(Input.GetKeyDown(KeyCode.A)){selectedItemIndex = betterModulo(selectedItemIndex - 1,               inventorySpace);  changed=true; AudioManager.getInstance().PlaySound("UIChangeSelection");}
        if(Input.GetKeyDown(KeyCode.D)){selectedItemIndex = betterModulo(selectedItemIndex + 1,               inventorySpace);  changed=true; AudioManager.getInstance().PlaySound("UIChangeSelection");}

        if(changed && onInventoryChangedCallback != null) onInventoryChangedCallback.Invoke();
    }

    void processSkillSelectionInput(){
        bool changed = false;
        if(Input.GetKeyDown(KeyCode.E)){equipment.equipSkill(skills[selectedSkillSlot]);                                          changed=true; AudioManager.getInstance().PlaySound("UILockIn");}
        if(Input.GetKeyDown(KeyCode.W)){selectedSkillSlot = betterModulo(selectedSkillSlot - skillSlotsPerRow, skillSlotsTotal);  changed=true; AudioManager.getInstance().PlaySound("UIChangeSelection");}
        if(Input.GetKeyDown(KeyCode.S)){selectedSkillSlot = betterModulo(selectedSkillSlot + skillSlotsPerRow, skillSlotsTotal);  changed=true; AudioManager.getInstance().PlaySound("UIChangeSelection");}
        if(Input.GetKeyDown(KeyCode.A)){selectedSkillSlot = betterModulo(selectedSkillSlot - 1,                skillSlotsTotal);  changed=true; AudioManager.getInstance().PlaySound("UIChangeSelection");}
        if(Input.GetKeyDown(KeyCode.D)){selectedSkillSlot = betterModulo(selectedSkillSlot + 1,                skillSlotsTotal);  changed=true; AudioManager.getInstance().PlaySound("UIChangeSelection");}

        if(changed && onInventoryChangedCallback != null) onInventoryChangedCallback.Invoke();
    }
    #endregion

    public bool addItem(Item item){
        if(item.isStackable){
            int j = FindIndexOfFirstMatchingSlot(item);
            if(j != -1){
                items[j].amount += item.amount;
                if(onInventoryChangedCallback != null){onInventoryChangedCallback.Invoke();}
                return true;
            }
        }
        int i = FindIndexOfFirstFreeSlot();
        if(i == -1){
            Debug.Log("Inventory Full");
            return false;
        } else {
            items[i] = item;
            if(onInventoryChangedCallback != null){onInventoryChangedCallback.Invoke();}
            return true;
        }
    }

    public void removeItem(int index){
        items[index] = null;
        if(onInventoryChangedCallback != null){onInventoryChangedCallback.Invoke();}
    } 

    private int FindIndexOfFirstFreeSlot(){
        for (int i = 0; i < items.Length; i++){
            if(items[i] == null){
                return i;
            }
        }
        return -1;
    }

    private int FindIndexOfFirstMatchingSlot(Item item){
        //TODO: If we ever implement randomized loot we need to Downcast the itemtype here first and implement the needed Equals Methods.
        for (int i = 0; i < items.Length; i++){
            if(item.Equals(items[i])){
                return i;
            }
        }
        return -1;
    }

    public Item[] getItems(){
        return items;
    }
    public int getSelectedItemIndex(){
        return selectedItemIndex;
    }
    public int getSelectedSkillSlot(){
        return selectedSkillSlot;
    }
    private int betterModulo(int dividend, int divisor){
        return (dividend % divisor + divisor) % divisor;   //Weird code, I know, but this makes it so that even when you input a negative number, it wraps back around to being positive. Because that's just what Modulo SHOULD do in my opinion.
    }
}
