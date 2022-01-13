using UnityEngine;

[CreateAssetMenu(fileName ="New Shield", menuName = "Items/Shield")] 
public class Shield : Item{
    public int bonusDefense;
    public int bonusStrength;
    [Tooltip("Negative Numbers will result in a Dexterity Penalty for the player.")]
    public int bonusDexterity;
    public int bonusIntelligence;
}
