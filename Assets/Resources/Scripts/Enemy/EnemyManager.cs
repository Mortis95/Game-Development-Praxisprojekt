using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour{
    private int currentHealthPoints;
    public int enemyMaxHealthPoints;
    public int enemyAttack;
    public int enemyDefense;
    public int enemyExpWorth;
    public List<DamageType> enemyWeaknesses;
    public List<DamageType> enemyResistances;
    public EnemyBehaviour enemyBehaviour;


    private void Awake(){
        currentHealthPoints = enemyMaxHealthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealthPoints <= 0){
            Debug.Log("F in chat");
            Player.getInstance().addExp(enemyExpWorth);
            Destroy(gameObject);
        }
    }

    public void takeDamage(DamageType dmgType, int amount){

        float dmgMultiplicator = 1;
        bool isCrit = false;
        bool isWeak = false;
        foreach (DamageType d in enemyWeaknesses) {
            if (d.Equals(dmgType)){
                dmgMultiplicator *= 2;
                isCrit = true;
            }
        }
        foreach (DamageType d in enemyResistances) {
            if (d.Equals(dmgType)){
                dmgMultiplicator /= 2;
                isWeak = true;
            }
        }

        //Calculate for Damage Multiplicator if Weakness or Resistance
        int damage = (int) ( ((float) amount) * dmgMultiplicator);

        //If damage is normal then Defense is subtracted
        if(dmgType.Equals(DamageType.Normal)){damage -= enemyDefense;}

        //Don't allow negativ damage. Pick 0 if damage is smaller than 0. Shouldn't happen though in normal gameplay.
        damage = Mathf.Max(0, damage);

        currentHealthPoints -= damage;
        TextPopup.createEnemyDamagePopup(transform, damage, dmgType, isCrit, isWeak);
    }

    public void getKnockback(){
        enemyBehaviour.getKnockedBack();
    }

    public void addResistance(DamageType dt){
        enemyResistances.Add(dt);
    }

    public void addWeakness(DamageType dt){
        enemyWeaknesses.Add(dt);
    }
}
