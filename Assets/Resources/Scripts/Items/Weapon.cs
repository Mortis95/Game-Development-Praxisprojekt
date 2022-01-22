using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Weapon", menuName = "Items/Weapon")] 
public class Weapon : Item
{
    public WeaponType weaponType;
    public Sprite projectile;
    
    public int bonusAttack;
    public int bonusStrength;
    public int bonusDexterity;
    public int bonusIntelligence;
    public override string getStatsAsFormattedString(){
        string stats = "";
        if(bonusAttack       != 0){stats += "ATK : " + bonusAttack    + "\r\n";}
        if(bonusStrength     != 0){stats += "STR : " + bonusStrength  + "\r\n";}
        if(bonusDexterity    != 0){stats += "DEX : " + bonusDexterity + "\r\n";}
        if(bonusIntelligence != 0){stats += "INT : " + bonusIntelligence;}
        return stats;
    }
    
      
}
