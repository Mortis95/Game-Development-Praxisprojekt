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
    [Tooltip("The Sound this consumable will call when it is used. Leave blank for no sound.")]
    public string interactionSound;

    public override void Use(){
        base.Use();
        Player pl = Player.getInstance();
        if(healHealthPoints != 0)pl.addHealthPoints(healHealthPoints);
        if(healMagicPoints != 0)pl.addMagicPoints(healMagicPoints);
        if(healHealthPointsPercentage != 0)pl.addHealthPointsPercentage(healHealthPointsPercentage);
        if(healMagicPointsPercentage != 0)pl.addMagicPointsPercentage(healMagicPointsPercentage);
        amount -= 1;
        if (amount <= 0){
            Destroy(this);
        }
        if(!interactionSound.Equals("")){AudioManager.getInstance().PlaySound(interactionSound);}
    }
    

    public override bool Equals(object other){
        if(!base.Equals(other)){return false;}
        Consumable otherConsumable = (Consumable) other;
        if(this.healHealthPoints != otherConsumable.healHealthPoints){return false;}
        if(this.healMagicPoints  != otherConsumable.healMagicPoints ){return false;}
        if(this.healHealthPointsPercentage != otherConsumable.healHealthPointsPercentage){return false;}
        if(this.healMagicPointsPercentage  != otherConsumable.healMagicPointsPercentage ){return false;}
        return true;
    }
}
