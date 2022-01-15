using UnityEngine;

[CreateAssetMenu(fileName ="New Shield", menuName = "Items/Shield")] 
public class Shield : Item{
    public int bonusDefense;
    public int bonusStrength;
    [Tooltip("Negative Numbers will result in a Dexterity Penalty for the player.")]
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
