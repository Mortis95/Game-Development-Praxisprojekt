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

    public GameObject inventoryUI;
    private List<Item> items;
    private int inventorySpace;

    void Start(){
        items = new List<Item>();
        inventorySpace = 20;            //I'd like to dynamically get this number, but I can't be arsed to introduce even more spaghetti. Just keep this consistent with the UI please ty
    }

    void Update(){
        
    }

    public bool addItem(Item item){
        if(items.Count >= inventorySpace){
            Debug.Log("Inventory Full");
            return false;
        } else {
            items.Add(item);
            if(onInventoryChangedCallback != null){onInventoryChangedCallback.Invoke();}
            return true;
        }
        
    }

    public void removeItem(Item item){
        items.Remove(item);
        if(onInventoryChangedCallback != null){onInventoryChangedCallback.Invoke();}
    } 
}
