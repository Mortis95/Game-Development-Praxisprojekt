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

    public static void SaveEquipment()
    {
        Weapon w = Equipment.getInstance().equippedWeapon;
        Armor a = Equipment.getInstance().equippedArmor;
        Consumable c = Equipment.getInstance().consumableInHand;
        Shield s = Equipment.getInstance().shieldInHand;
        Ability ab = Equipment.getInstance().equippedAbility;

        if (w != null)
        {
            ItemData itemWeapon = new ItemData(w);
            BinaryFormatter formatterW = new BinaryFormatter();
            string pathW = Application.persistentDataPath + "/WeaponItem.b";
            FileStream streamW = new FileStream(pathW, FileMode.Create);

            formatterW.Serialize(streamW, itemWeapon);
        }

        if (a != null)
        {
            ItemData itemArmor = new ItemData(a);
            BinaryFormatter formatterA = new BinaryFormatter();
            string pathA = Application.persistentDataPath + "/ArmorItem.b";
            FileStream streamA = new FileStream(pathA, FileMode.Create);

            formatterA.Serialize(streamA, itemArmor);
        }


        if (c != null)
        {
            ItemData itemConsumable = new ItemData(c);
            BinaryFormatter formatterC = new BinaryFormatter();
            string pathC = Application.persistentDataPath + "/ConsumableItem.b";
            FileStream streamC = new FileStream(pathC, FileMode.Create);

            formatterC.Serialize(streamC, itemConsumable);
        }

        if (s != null)
        {
            ItemData itemShield = new ItemData(s);
            BinaryFormatter formatterS = new BinaryFormatter();
            string pathS = Application.persistentDataPath + "/ShildItem.b";
            FileStream streamS = new FileStream(pathS, FileMode.Create);

            formatterS.Serialize(streamS, itemShield);
        }

        BinaryFormatter formatterAB = new BinaryFormatter();
        string pathAB = Application.persistentDataPath + "/AbItem.b";
        FileStream streamAB = new FileStream(pathAB, FileMode.Create);

        formatterAB.Serialize(streamAB, ab.ToString());
        
    }

    public static void LoadEquipment()
    {
        Equipment.getInstance().equippedWeapon = null;
        Equipment.getInstance().equippedArmor = null;
        Equipment.getInstance().consumableInHand = null;
        Equipment.getInstance().shieldInHand = null;
        Equipment.getInstance().equippedAbility = Ability.NoAbilityEquipped;

        Weapon w = Equipment.getInstance().equippedWeapon;
        Armor a = Equipment.getInstance().equippedArmor;
        Consumable c = Equipment.getInstance().consumableInHand;
        Shield s = Equipment.getInstance().shieldInHand;
        Ability ab = Equipment.getInstance().equippedAbility;

        if (File.Exists(Application.persistentDataPath + "/WeaponItem.b"))
        {
            BinaryFormatter formatterW = new BinaryFormatter();
            string pathW = Application.persistentDataPath + "/WeaponItem.b";
            FileStream streamW = new FileStream(pathW, FileMode.Open);
            ItemData weaponItemData = formatterW.Deserialize(streamW) as ItemData;


            Weapon weapon = new Weapon();


            weapon.amount = weaponItemData.amount;
            weapon.description = weaponItemData.description;
            weapon.isStackable = weaponItemData.isStackable;
            weapon.itemName = weaponItemData.itemName;
            var sprite = Resources.Load(weaponItemData.spritePath) as Sprite;
            weapon.itemSprite = sprite;
            weapon.itemType = (ItemType)System.Enum.Parse(typeof(ItemType), weaponItemData.itemType);

            weapon.maxStackSize = weaponItemData.maxStackSize;

            weapon.weaponType = (WeaponType)System.Enum.Parse(typeof(WeaponType), weaponItemData.weaponType);
            weapon.projectile = Resources.Load(weaponItemData.projectile) as Sprite;
            weapon.bonusAttack = weaponItemData.bonusAttack;
            weapon.bonusStrength = weaponItemData.bonusStrength;
            weapon.bonusDexterity = weaponItemData.bonusDexterity;
            weapon.bonusIntelligence = weaponItemData.bonusIntelligence;

            Equipment.getInstance().equippedWeapon = weapon;
        }

        if (File.Exists(Application.persistentDataPath + "/ArmorItem.b"))
        {
            BinaryFormatter formatterA = new BinaryFormatter();
            string pathA = Application.persistentDataPath + "/ArmorItem.b";
            FileStream streamA = new FileStream(pathA, FileMode.Open);
            ItemData armorItemData = formatterA.Deserialize(streamA) as ItemData;

            Armor armor = new Armor();

            armor.amount = armorItemData.amount;
            armor.description = armorItemData.description;
            armor.isStackable = armorItemData.isStackable;
            armor.itemName = armorItemData.itemName;

            var spriteA = Resources.Load(armorItemData.spritePath) as Sprite;
            armor.itemSprite = spriteA;

            armor.itemSprite = Resources.Load<Sprite>(armorItemData.spritePath);
            armor.itemType = (ItemType)System.Enum.Parse(typeof(ItemType), armorItemData.itemType);
            armor.maxStackSize = armorItemData.maxStackSize;

            armor.bonusDefense = armorItemData.bonusDefenseA;
            armor.bonusStrength = armorItemData.bonusStrengthA;
            armor.bonusDexterity = armorItemData.bonusDexterityA;
            armor.bonusIntelligence = armorItemData.bonusIntelligenceA;

            Equipment.getInstance().equippedArmor = armor;
        }

        if (File.Exists(Application.persistentDataPath + "/ConsumableItem.b"))
        {
            BinaryFormatter formatterC = new BinaryFormatter();
            string pathC = Application.persistentDataPath + "/ConsumableItem.b";
            FileStream streamC = new FileStream(pathC, FileMode.Open);
            ItemData consumableItemData = formatterC.Deserialize(streamC) as ItemData;

            Consumable consumable = new Consumable();

            consumable.amount = consumableItemData.amount;
            consumable.description = consumableItemData.description;
            consumable.isStackable = consumableItemData.isStackable;
            consumable.itemName = consumableItemData.itemName;

            var spriteC = Resources.Load(consumableItemData.spritePath) as Sprite;
            consumable.itemSprite = spriteC;
            consumable.itemType = (ItemType)System.Enum.Parse(typeof(ItemType), consumableItemData.itemType);
            consumable.maxStackSize = consumableItemData.maxStackSize;

            consumable.healHealthPoints = consumableItemData.healHealthPoints;
            consumable.healMagicPoints = consumableItemData.healMagicPoints;
            consumable.healHealthPointsPercentage = consumableItemData.healHealthPointsPercentage;
            consumable.healMagicPointsPercentage = consumableItemData.healMagicPointsPercentage;

            Equipment.getInstance().consumableInHand = consumable;
        }

        if (File.Exists(Application.persistentDataPath + "/ShildItem.b"))
        {
            BinaryFormatter formatterS = new BinaryFormatter();
            string pathS = Application.persistentDataPath + "/ShildItem.b";
            FileStream streamS = new FileStream(pathS, FileMode.Open);
            ItemData shieldItemData = formatterS.Deserialize(streamS) as ItemData;

            Shield shield = new Shield();

            shield.amount = shieldItemData.amount;
            shield.description = shieldItemData.description;
            shield.isStackable = shieldItemData.isStackable;
            shield.itemName = shieldItemData.itemName;

            var spriteS = Resources.Load(shieldItemData.spritePath) as Sprite;
            shield.itemSprite = spriteS;

            shield.itemSprite = Resources.Load<Sprite>(shieldItemData.spritePath);
            shield.itemType = (ItemType)System.Enum.Parse(typeof(ItemType), shieldItemData.itemType);
            shield.maxStackSize = shieldItemData.maxStackSize;

            shield.bonusDefense = shieldItemData.bonusDefenseS;
            shield.bonusStrength = shieldItemData.bonusStrengthS;
            shield.bonusDexterity = shieldItemData.bonusDexterityS;
            shield.bonusIntelligence = shieldItemData.bonusIntelligenceS;

            Equipment.getInstance().shieldInHand = shield;
        }


        BinaryFormatter formatterAB = new BinaryFormatter();
        string pathAB = Application.persistentDataPath + "/AbItem.b";
        FileStream streamAB = new FileStream(pathAB, FileMode.Open);

        
        
        
        string abItemData = formatterAB.Deserialize(streamAB) as string;


        Equipment.getInstance().equippedAbility = (Ability)System.Enum.Parse(typeof(Ability), abItemData);

        Player.getInstance().UpdateEquipment();

    }

}