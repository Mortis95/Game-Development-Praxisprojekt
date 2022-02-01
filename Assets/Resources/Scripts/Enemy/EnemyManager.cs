using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour{

    #region PublicVariables
    //Public Variables that people can set through the Editor
    private int currentHealthPoints;
    public int enemyMaxHealthPoints;
    public int enemyAttack;
    public int enemyDefense;
    public int enemyExpWorth;
    public List<DamageType> enemyWeaknesses;
    public List<DamageType> enemyResistances;
    [Tooltip("The sound this enemy will play when it dies. Leave Blank for a generic Mob Explosion Sound.")]
    public string deathSound;
    #endregion

    #region PrivateVariables
    //Private Variables we should get ourselves
    private EnemyBehaviour enemyBehaviour;
    private DropTable enemyDropTable;
    private bool isAlive;
    #endregion


    private void Awake(){
        currentHealthPoints = enemyMaxHealthPoints;
        isAlive = true;
    }

    void Start(){
        if(enemyBehaviour == null){enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();}
        if(enemyDropTable == null){enemyDropTable = gameObject.GetComponent<DropTable>();}

    }

    // Update is called once per frame
    void Update(){
        if(currentHealthPoints <= 0 && isAlive){
            onDeath();
        }
    }

    void onDeath(){
        //Update isAlive so this method only plays once
        isAlive = false;

        //Give Player EXP
        Player.getInstance().addExp(enemyExpWorth);

        //Let EnemyBehaviour know we just died (F in chat)
        if(enemyBehaviour != null){enemyBehaviour.onDeath();}

        //Destroy important GameObject-Components, so that the Enemy will stop dealing damage.
        Collider2D col = gameObject.GetComponent<Collider2D>();
        if(col != null){Destroy(col);}

        //Generate ItemDrops if there are any
        if(enemyDropTable != null){
            List<Item> drops = enemyDropTable.getDrops();
            foreach (Item drop in drops){
                ItemDropController.createItemDropWithOffset(transform, drop, 1f);
            }
        }

        //Play Death Sound
        if(!deathSound.Equals("")){AudioManager.getInstance().PlaySound(deathSound);}
        else{AudioManager.getInstance().PlaySound("MobExplosion");}

        //Destroy GameObject after small delay
        Destroy(gameObject, 3f);
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

        //Subtract Defense from the damage 
        damage -= enemyDefense;

        //Don't allow negativ damage. Pick 0 if damage is smaller than 0. Shouldn't happen though in normal gameplay.
        damage = Mathf.Max(0, damage);

        currentHealthPoints -= damage;
        TextPopup.createEnemyDamagePopup(transform, damage, dmgType, isCrit, isWeak);
        if(enemyBehaviour != null){enemyBehaviour.findTarget();}
    }

    public void getKnockback(Vector2 origin, float knockBackForce){
        if(enemyBehaviour != null){enemyBehaviour.getKnockedBack(origin, knockBackForce);}
    }

    public void addResistance(DamageType dt){
        enemyResistances.Add(dt);
    }

    public void addWeakness(DamageType dt){
        enemyWeaknesses.Add(dt);
    }
}
