using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon 
{
    public int damage;
    public WeaponType weaponType;

    public enum WeaponType
   {
       Melee,
       Bow,
       Staff
   }

   public Weapon(int damage,WeaponType wt)
   {
       this.damage=damage;       
       this.weaponType=wt;
   }

      
}
