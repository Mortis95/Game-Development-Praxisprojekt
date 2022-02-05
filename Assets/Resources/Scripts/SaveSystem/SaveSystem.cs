using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Linq;

public static class SaveSystem
{
    public static void SavePlayer(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.b";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.b";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        } else
        {
            Debug.LogError("Player save file doesn't exists!");
            return null;
        }
    }

    public static void SaveMap(string mapName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/map.b";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, mapName);
        stream.Close();
    }

    public static string LoadMap()
    {
        string path = Application.persistentDataPath + "/map.b";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            string data = formatter.Deserialize(stream) as string;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("No map saved!");
            return null;
        }
    }

    public static void SavePlayerItems(Item[] items)
    {

        Item[] filteredItems = items.Where(item => item != null).ToArray();
        int itemCount = filteredItems.Length;

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/itemCount.b";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, itemCount);
        stream.Close();

        for (int i = 0; i < itemCount; i++)
        {
            BinaryFormatter itemFormatter = new BinaryFormatter();
            string itemPath = Application.persistentDataPath + "/item" + i.ToString() + ".b";
            FileStream itemStream = new FileStream(itemPath, FileMode.Create);

            itemFormatter.Serialize(itemStream, new ItemData(filteredItems[i]));
            itemStream.Close();
        }
    }

    public static List<ItemData> LoadPlayerItems()
    {
        string path = Application.persistentDataPath + "/itemCount.b";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            int data = (int)formatter.Deserialize(stream);
            stream.Close();

            int itemCount = data;
            List<ItemData> items = new List<ItemData>();
            if (itemCount != 0)
            {
                for (int i = 0; i < itemCount; i++)
                {
                    Inventory.getInstance().removeItem(i);
                }
                for (int i = 0; i < itemCount; i++)
                {
                    BinaryFormatter itemFormatter = new BinaryFormatter();
                    string itemPath = Application.persistentDataPath + "/item" + i.ToString() + ".b";
                    FileStream itemStream = new FileStream(itemPath, FileMode.Open);
                    ItemData itemData = itemFormatter.Deserialize(itemStream) as ItemData;
                    items.Add(itemData);
                }
            }
            return items;
        }
        else
        {
            Debug.LogError("Items save file doesn't exists!");
            return null;
        }
    }

    public static void SaveSkillTree()
    {
        var nodes = SkillTree.getInstance().getSkillTreeNodes();
        var data = new SkillTreeData(nodes[9].currentLevel, nodes[10].currentLevel, nodes[11].currentLevel);

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/skillData.b";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static void LoadSkillTree()
    {
        string path = Application.persistentDataPath + "/skillData.b";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            var nodes = SkillTree.getInstance().getSkillTreeNodes();
            var data = formatter.Deserialize(stream) as SkillTreeData;

            for (int i = 0; i < data.skillpointsRangeAttack; i++)
            {
                nodes[9].levelNode();
            }
            for (int i = 0; i < data.skillpointsAttack; i++)
            {
                nodes[10].levelNode();
            }
            for (int i = 0; i < data.skillpointsMagic; i++)
            {
                nodes[11].levelNode();
            }

            stream.Close();
        }
        else
        {
            Debug.LogError("No map saved!");
        }
    }

}
