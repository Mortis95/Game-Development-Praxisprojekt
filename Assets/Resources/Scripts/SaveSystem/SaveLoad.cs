using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    public void LoadData()
    {

        string scene = SaveSystem.LoadMap();
        if (scene != null)
        {
            SceneManager.LoadScene(scene);


            /*List<ItemData> itemData = SaveSystem.LoadPlayerItems();

            for (int i = 0; i < itemData.Count; i++)
            {
                Item item = new Item();
                item.amount = itemData[i].amount;
                item.description = itemData[i].description;
                item.isStackable = itemData[i].isStackable;
                item.itemName = itemData[i].itemName;
                item.itemSprite = Resources.Load<Sprite>(itemData[i].spritePath);
                item.itemType = (ItemType)System.Enum.Parse(typeof(ItemType), itemData[i].itemType);
                item.maxStackSize = itemData[i].maxStackSize;
                Player.getInstance().inventory.addItem(item);

            }*/

            PlayerData playerData = SaveSystem.LoadPlayer();
            Player.getInstance().GetComponent<Transform>().position = new Vector3(playerData.position[0], playerData.position[1], playerData.position[2]);
            Player.getInstance().experiencePoints = playerData.exp;
            Player.getInstance().currentLevel = playerData.level;
            Player.getInstance().currentHealthPoints = playerData.currentHealthPoints;
            Player.getInstance().maxHealthPoints = playerData.maxHealthPoints;
            Player.getInstance().currentMagicPoints = playerData.currentMagicPoints;
            Player.getInstance().maxMagicPoints = playerData.maxMagicPoints;
        }
    }

    public void SaveData()
    {
        SaveSystem.SavePlayer(Player.getInstance());
        //SaveSystem.SavePlayerItems(Player.getInstance().inventory.getItems());
        SaveSystem.SaveMap(SceneManager.GetActiveScene().name);
    }
}
