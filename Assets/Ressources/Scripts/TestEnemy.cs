using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{

    public int maxHP;
    public int currentHP;
    public DamageType[] weaknesses;
    public DamageType[] resistances;

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

    public void takeDamage(DamageType dmgType, int amount){

        float dmgMultiplicator = 1;
        foreach (DamageType d in weaknesses) {
            if (d == dmgType){
                dmgMultiplicator *= 2;
            }
        }
        foreach (DamageType d in resistances) {
            if (d == dmgType){
                dmgMultiplicator /= 2;
            }
        }

        currentHP -= (int)(amount * dmgMultiplicator);
    }

    public void testMethod(){
        Debug.Log("Hallo!");
    }
}
