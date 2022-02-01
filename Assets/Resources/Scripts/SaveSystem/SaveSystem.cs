using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

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
        int itemCount = items.Length;

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/itemCount.b";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, itemCount);
        stream.Close();

        for (int i = 0; i < itemCount; i++)
        {
            BinaryFormatter itemFormatter = new BinaryFormatter();
            string itemPath = Application.persistentDataPath + "/item" + i.ToString() + ".b";
            FileStream itemStream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, new ItemData(items[i]));
            stream.Close();
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
        SkillTree skillTree = SkillTree.getInstance();
        skillTree.getSkillTreeNodes();
    }
}
