using UnityEngine;

[CreateAssetMenu(fileName ="New Shield", menuName = "Items/Shield")] 
public class Shield : Item{
    public int bonusDefense;
    [Tooltip("Negative Numbers will result in a Dexterity Penalty for the player.")]
    public int bonusDexterity;
}
