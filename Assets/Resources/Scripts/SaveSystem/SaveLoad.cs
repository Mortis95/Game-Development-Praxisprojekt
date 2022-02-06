using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SaveLoad : MonoBehaviour
{
    public void LoadData()
    {

        

        string scene = SaveSystem.LoadMap();
        if (scene != null)
        {
            SceneManager.LoadScene(scene);


            List<ItemData> itemData = SaveSystem.LoadPlayerItems();

            for (int i = 0; i < itemData.Count; i++)
            {

                switch (itemData[i].itemType)
                {
                    case "Weapon":
                        Weapon weapon = new Weapon();


                        weapon.amount = itemData[i].amount;
                        weapon.description = itemData[i].description;
                        weapon.isStackable = itemData[i].isStackable;
                        weapon.itemName = itemData[i].itemName;
                        var sprite2d = AssetDatabase.LoadAssetAtPath<Texture2D>(itemData[i].spritePath);
                        Sprite sprite = Sprite.Create(sprite2d, new Rect(0,0, sprite2d.width, sprite2d.height), new Vector2(0.5f,0.5f));
                        weapon.itemSprite = sprite;
                        weapon.itemType = (ItemType)System.Enum.Parse(typeof(ItemType), itemData[i].itemType);
                        
                        weapon.maxStackSize = itemData[i].maxStackSize;

                        weapon.weaponType = (WeaponType)System.Enum.Parse(typeof(WeaponType), itemData[i].weaponType);
                        weapon.projectile = Resources.Load<Sprite>(itemData[i].projectile); 
                        weapon.bonusAttack = itemData[i].bonusAttack;
                        weapon.bonusStrength = itemData[i].bonusStrength;
                        weapon.bonusDexterity = itemData[i].bonusDexterity;
                        weapon.bonusIntelligence = itemData[i].bonusIntelligence;
                        Player.getInstance().inventory.addItem(weapon);
                        break;
                    case "Consumable":
                        Consumable consumable = new Consumable();

                        consumable.amount = itemData[i].amount;
                        consumable.description = itemData[i].description;
                        consumable.isStackable = itemData[i].isStackable;
                        consumable.itemName = itemData[i].itemName;

                        var sprite2dC = AssetDatabase.LoadAssetAtPath<Texture2D>(itemData[i].spritePath);
                        Sprite spriteC = Sprite.Create(sprite2dC, new Rect(0, 0, sprite2dC.width, sprite2dC.height), new Vector2(0.5f, 0.5f));
                        consumable.itemSprite = spriteC;
                        consumable.itemType = (ItemType)System.Enum.Parse(typeof(ItemType), itemData[i].itemType);
                        consumable.maxStackSize = itemData[i].maxStackSize;

                        consumable.healHealthPoints = itemData[i].healHealthPoints;
                        consumable.healMagicPoints = itemData[i].healMagicPoints;
                        consumable.healHealthPointsPercentage = itemData[i].healHealthPointsPercentage;
                        consumable.healMagicPointsPercentage = itemData[i].healMagicPointsPercentage;
                        Player.getInstance().inventory.addItem(consumable);
                        break;
                    case "Armor":
                        Armor armor = new Armor();

                        armor.amount = itemData[i].amount;
                        armor.description = itemData[i].description;
                        armor.isStackable = itemData[i].isStackable;
                        armor.itemName = itemData[i].itemName;

                        var sprite2dA = AssetDatabase.LoadAssetAtPath<Texture2D>(itemData[i].spritePath);
                        Sprite spriteA = Sprite.Create(sprite2dA, new Rect(0, 0, sprite2dA.width, sprite2dA.height), new Vector2(0.5f, 0.5f));
                        armor.itemSprite = spriteA;

                        armor.itemSprite = Resources.Load<Sprite>(itemData[i].spritePath); 
                        armor.itemType = (ItemType)System.Enum.Parse(typeof(ItemType), itemData[i].itemType);
                        armor.maxStackSize = itemData[i].maxStackSize;

                        armor.bonusDefense = itemData[i].bonusDefenseA;
                        armor.bonusStrength = itemData[i].bonusStrengthA;
                        armor.bonusDexterity = itemData[i].bonusDexterityA;
                        armor.bonusIntelligence = itemData[i].bonusIntelligenceA;
                        Player.getInstance().inventory.addItem(armor);
                        break;
                    case "Shield":
                        Shield shield = new Shield();

                        shield.amount = itemData[i].amount;
                        shield.description = itemData[i].description;
                        shield.isStackable = itemData[i].isStackable;
                        shield.itemName = itemData[i].itemName;

                        var sprite2dS = AssetDatabase.LoadAssetAtPath<Texture2D>(itemData[i].spritePath);
                        Sprite spriteS = Sprite.Create(sprite2dS, new Rect(0, 0, sprite2dS.width, sprite2dS.height), new Vector2(0.5f, 0.5f));
                        shield.itemSprite = spriteS;

                        shield.itemSprite = Resources.Load<Sprite>(itemData[i].spritePath);
                        shield.itemType = (ItemType)System.Enum.Parse(typeof(ItemType), itemData[i].itemType);
                        shield.maxStackSize = itemData[i].maxStackSize;

                        shield.bonusDefense = itemData[i].bonusDefenseS;
                        shield.bonusStrength = itemData[i].bonusStrengthS;
                        shield.bonusDexterity = itemData[i].bonusDexterityS;
                        shield.bonusIntelligence = itemData[i].bonusIntelligenceS;
                        Player.getInstance().inventory.addItem(shield);
                        break;
                    default:
                        Item item = new Item();

                        item.amount = itemData[i].amount;
                        item.description = itemData[i].description;
                        item.isStackable = itemData[i].isStackable;
                        item.itemName = itemData[i].itemName;

                        item.itemSprite = Resources.Load<Sprite>(itemData[i].spritePath);
                        item.itemType = (ItemType)System.Enum.Parse(typeof(ItemType), itemData[i].itemType);
                        item.maxStackSize = itemData[i].maxStackSize;

                        var sprite2dI = AssetDatabase.LoadAssetAtPath<Texture2D>(itemData[i].spritePath);
                        Sprite spriteSI = Sprite.Create(sprite2dI, new Rect(0, 0, sprite2dI.width, sprite2dI.height), new Vector2(0.5f, 0.5f));
                        item.itemSprite = spriteSI;

                        Player.getInstance().inventory.addItem(item);
                        break;

                }
            }

            PlayerData playerData = SaveSystem.LoadPlayer();
            Player.getInstance().GetComponent<Transform>().position = new Vector3(playerData.position[0], playerData.position[1], playerData.position[2]);
            Player.getInstance().experiencePoints = playerData.exp;
            Player.getInstance().currentLevel = playerData.level;
            Player.getInstance().currentHealthPoints = playerData.currentHealthPoints;
            Player.getInstance().maxHealthPoints = playerData.maxHealthPoints;
            Player.getInstance().currentMagicPoints = playerData.currentMagicPoints;
            Player.getInstance().maxMagicPoints = playerData.maxMagicPoints;

            SaveSystem.LoadSkillTree();

            var nodes = SkillTree.getInstance().getSkillTreeNodes();

            if (playerData.ScharfschussLearned)
            {
                nodes[0].levelNode();
            }
            if (playerData.RageLearned)
            {
                nodes[1].levelNode();
            }
            if (playerData.KettenblitzLearned)
            {
                nodes[2].levelNode();
            }
            if (playerData.WasserpfeilhagelLearned)
            {
                nodes[3].levelNode();
            }
            if (playerData.ElektrowirbelLearned)
            {
                nodes[4].levelNode();
            }
            if (playerData.WasserflaecheLearned)
            {
                nodes[5].levelNode();
            }
            if (playerData.FeuerpfeilLearned)
            {
                nodes[6].levelNode();
            }
            if (playerData.WasserhiebLearned)
            {
                nodes[7].levelNode();
            }
            if (playerData.FeuerballLearned)
            {
                nodes[8].levelNode();
            }

            SaveSystem.LoadEquipment();

        }
    }

    public void SaveData()
    {
        SaveSystem.SavePlayer(Player.getInstance());
        SaveSystem.SavePlayerItems(Player.getInstance().inventory.getItems());
        SaveSystem.SaveMap(SceneManager.GetActiveScene().name);
        SaveSystem.SaveEquipment();
        SaveSystem.SaveSkillTree();
    }
}
