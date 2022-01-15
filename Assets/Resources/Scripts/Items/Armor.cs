using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Neue Armor", menuName = "Items/Armor")] 
public class Armor : Item{
    public int bonusDefense;
    public int bonusStrength;
    public int bonusDexterity;
    public int bonusIntelligence;
    public override string getStatsAsFormattedString(){
        string stats = "";
        if(bonusDefense      != 0){stats += "DEF : " + bonusDefense   + "\r\n";}
        if(bonusStrength     != 0){stats += "STR : " + bonusStrength  + "\r\n";}
        if(bonusDexterity    != 0){stats += "DEX : " + bonusDexterity + "\r\n";}
        if(bonusIntelligence != 0){stats += "INT : " + bonusIntelligence;}
        return stats;
    }

}
