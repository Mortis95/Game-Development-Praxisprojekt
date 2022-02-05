using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class ItemData
{
    public string itemType;
    public string itemName;
    public string description;
    public bool isStackable;
    public int maxStackSize;
    public int amount;
    public string spritePath;

    // if item type is Weapong
    public string weaponType;
    public string projectile;

    public int bonusAttack;
    public int bonusStrength;
    public int bonusDexterity;
    public int bonusIntelligence;

    // if item type is consumable
    public int healHealthPoints;
    public int healMagicPoints;
    public float healHealthPointsPercentage;
    public float healMagicPointsPercentage;

    // if item type is armor
    public int bonusDefenseA;
    public int bonusStrengthA;
    public int bonusDexterityA;
    public int bonusIntelligenceA;

    // if item type is shield
    public int bonusDefenseS;
    public int bonusStrengthS;
    public int bonusDexterityS;
    public int bonusIntelligenceS;


    public ItemData(Item item)
        {
        itemType = item.itemType.ToString();
        itemName = item.itemName;
        description = item.description;
        isStackable = item.isStackable;
        maxStackSize = item.maxStackSize;
        amount = item.amount;
        spritePath = AssetDatabase.GetAssetPath(item.itemSprite);
        switch (item.itemType)
        {
            case ItemType.Weapon:
                weaponType = ((Weapon)item).weaponType.ToString();
                projectile = AssetDatabase.GetAssetPath(((Weapon)item).projectile);
                bonusAttack = ((Weapon)item).bonusAttack;
                bonusStrength = ((Weapon)item).bonusStrength;
                bonusDexterity = ((Weapon)item).bonusDexterity;
                bonusIntelligence = ((Weapon)item).bonusIntelligence;
                break;
            case ItemType.Consumable:
                healHealthPoints = ((Consumable)item).healHealthPoints;
                healMagicPoints = ((Consumable)item).healMagicPoints;
                healHealthPointsPercentage = ((Consumable)item).healHealthPointsPercentage;
                healMagicPointsPercentage = ((Consumable)item).healMagicPointsPercentage;
                break;
            case ItemType.Armor:
                bonusDefenseA = ((Armor)item).bonusDefense;
                bonusStrengthA = ((Armor)item).bonusStrength;
                bonusDexterityA = ((Armor)item).bonusDexterity;
                bonusIntelligenceA = ((Armor)item).bonusIntelligence;
                break;
            case ItemType.Shield:
                bonusDefenseS = ((Shield)item).bonusDefense;
                bonusStrengthS = ((Shield)item).bonusStrength;
                bonusDexterityS = ((Shield)item).bonusDexterity;
                bonusIntelligenceS = ((Shield)item).bonusIntelligence;
                break;
            default:
                break;

        }
    }
}
