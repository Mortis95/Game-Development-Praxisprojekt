using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Weapon", menuName = "Items/Weapon")] 
public class Weapon : Item
{
    public WeaponType weaponType;
    public Sprite projectile;
    
    public int bonusAttack;
    public int bonusMagicAttack;
    
      
}
