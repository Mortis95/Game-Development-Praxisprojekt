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
    private int inventorySpace;
    private int slotsPerRow;
    private int slotsPerColumn;
    private int selectedItemIndex;
    public Equipment equipment;
    void Start(){
        slotsPerRow = 5;    //I'd like to dynamically get these numbers, but I can't be arsed to introduce even more spaghetti. Just keep this consistent with the UI please ty
        slotsPerColumn = 4;
        inventorySpace = slotsPerRow * slotsPerColumn; 
        items = new Item[inventorySpace];
        selectedItemIndex = 0;
        inventoryUI = GetComponentInChildren<InventoryUI>();
        }

    void Update(){
        if(inventoryUI.getVisibility()){processInput();}
    }

    #region PlayerSelectItemInput
    void processInput(){
        bool changed = false;
        if(Input.GetKeyDown(KeyCode.E)){items[selectedItemIndex] = equipment.equipItem(items[selectedItemIndex]);           changed=true;}
        if(Input.GetKeyDown(KeyCode.W)){selectedItemIndex = betterModulo(selectedItemIndex - slotsPerRow, inventorySpace);  changed=true;}
        if(Input.GetKeyDown(KeyCode.S)){selectedItemIndex = betterModulo(selectedItemIndex + slotsPerRow, inventorySpace);  changed=true;}
        if(Input.GetKeyDown(KeyCode.A)){selectedItemIndex = betterModulo(selectedItemIndex - 1,           inventorySpace);  changed=true;}
        if(Input.GetKeyDown(KeyCode.D)){selectedItemIndex = betterModulo(selectedItemIndex + 1,           inventorySpace);  changed=true;}

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
    private int betterModulo(int dividend, int divisor){
        return (dividend % divisor + divisor) % divisor;   //Weird code, I know, but this makes it so that even when you input a negative number, it wraps back around to being positive. Because that's just what Modulo SHOULD do in my opinion.
    }
}