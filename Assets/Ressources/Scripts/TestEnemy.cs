using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{

    public int maxHP;
    public int currentHP;
    public DamageType[] weaknesses;
    public DamageType[] resistances;
    public GameObject damagePopupPrefab;

    private void Awake(){
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHP <= 0){
            Debug.Log("F in chat");
            Destroy(gameObject);
        }
    }

    public void takeDamage(DamageType dmgType, float amount){

        float dmgMultiplicator = 1;
        bool isCrit = false;
        bool isWeak = false;
        foreach (DamageType d in weaknesses) {
            if (d == dmgType){
                dmgMultiplicator *= 2;
                isCrit = true;
            }
        }
        foreach (DamageType d in resistances) {
            if (d == dmgType){
                dmgMultiplicator /= 2;
                isWeak = true;
            }
        }

        int damage = (int) (amount * dmgMultiplicator);
        currentHP -= damage;
        DamagePopupController.create(damagePopupPrefab, transform, damage, dmgType, isCrit, isWeak);
    }

    public void testMethod(){
        Debug.Log("Hallo!");
    }
}
