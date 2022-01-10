using UnityEngine;

[CreateAssetMenu(fileName ="Neues Consumable", menuName = "Items/Consumable")] 
public class Consumable : Item{

    [Range(-100,100), Tooltip("Negative values result in damaging the Player when consumed!")]
    public int healHealthPoints;
    [Range(-100,100), Tooltip("Negative values result in damaging the Player when consumed!")]
    public int healMagicPoints;
    [Range(-1f,1f), Tooltip("Negative values result in damaging the Player when consumed!")]
    public float healHealthPointsPercentage;
    [Range(-1f,1f), Tooltip("Negative values result in damaging the Player when consumed!")]
    public float healMagicPointsPercentage;

    public override void Use()
    {
        base.Use();
        Player pl = Player.getInstance();
        pl.addHealthPoints(healHealthPoints);
        pl.addMagicPoints(healMagicPoints);
        pl.addHealthPointsPercentage(healHealthPointsPercentage);
        pl.addMagicPointsPercentage(healMagicPointsPercentage);
    }
}
