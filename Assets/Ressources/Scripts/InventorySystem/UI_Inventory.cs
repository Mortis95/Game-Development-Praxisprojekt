using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    private Transform UIPos;
    private int rowCap =4;          //wie viele items sollen in einer reihe angezeigt werden?
    private bool isDrop=false;
    private bool isVisible=false;
    public Transform parent;
    public Player player;
    

    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
        UIPos = gameObject.transform;
    }

    public void setInventory(Inventory newInventory)
    {
        this.inventory=newInventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    public void switchVisiblity(){
        //Erstmal Variable updaten
        isVisible = !isVisible;

        //Danach schauen wo Inventar stehen soll
        if (isVisible){
            //Entweder an Parent Canvas position (Die Mitte vom Screen)
            UIPos.position = parent.position;
        } else {
            //Oder in bumfuck nowhere (offscreen)
            UIPos.position = new Vector3(20000,20000,0);
        }
    }

    void Update(){}
    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach(Transform child in itemSlotContainer)
        {
            
            if(child == itemSlotTemplate) continue ;     
            Destroy(child.gameObject);            
        }

        int x = 0;
        int y = 0;
        float cellSize = 100f;

        foreach(Item item in inventory.getItemList())
        {
            
            RectTransform itemSlotRectTransform =  Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition = new Vector2(x*cellSize, y*cellSize);
            Image image = itemSlotRectTransform.Find("Image").GetComponent<Image>();
            image.sprite = item.GetSprite();//sprite aus den assets holen
            Text uiText = itemSlotRectTransform.Find("amountText").GetComponent<Text>();    //mengenangabe refreshen
            if(item.amount>1)
            {
                uiText.text =item.amount.ToString();

            }else{
                uiText.text ="";

            }

            Button myButton = itemSlotRectTransform.Find("button").GetComponent<Button>();
             
            myButton.onClick.AddListener(() => dropOrUse(item));      
            x++;
            if(x>rowCap)    //wenn reihe voll dann die darunter befüllen
            {
                x=0;
                y--;
            }
        }
    }

    public void setisDrop()
    {
        isDrop = !isDrop;
        Debug.Log(isDrop);
    }

    private void dropOrUse(Item item)
    {
        if(isDrop)
        {
        Debug.Log("dropitem");
        Item duplicate = new Item {itemtype=item.itemtype, amount=item.amount};
        inventory.RemoveItem(item);
        ItemWorld.DropItem(player.position, duplicate);
        if(item.isequipped==true)
            {
                if(item.GetItemClass() == Item.ItemClass.Weapon)
                {
                    Debug.Log("Weapon gedroppt");
                    player.EquipedWeapon=null;
                }
                if(item.GetItemClass() == Item.ItemClass.Armor)
                {
                    Debug.Log("Armor gedroppt");
                    player.EquipedArmor=null;
                }
                if(item.GetItemClass() == Item.ItemClass.Shield)
                {
                    Debug.Log("Shield gedroppt");
                    player.EquipedShield=null;
                }
            }        
        }else
        {
        Debug.Log("use");
        inventory.UseItem(item);
        }
        
    }

}
