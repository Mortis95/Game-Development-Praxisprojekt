using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kettenblitz : MonoBehaviour
{

    private Player caster;
    private int damage;
    private int maxTargets = 5;
    private int range = 20;
    private GameObject[] targets;
    private bool[] targetsHit;
    private int numOfHits = 0;

    private int[] targetIndices;
    private LineRenderer lr;
    private bool spellFinished = false;
    private void Awake(){
        //Setup damit alles ordentlich funktioniert
        caster = Player.getInstance();                          //Get Player Instance and get Player-Intelligence
        damage = caster.inte;                                   //Player-Intelligence = Damage (TODO: Set reasonable damage)
        targets = GameObject.FindGameObjectsWithTag("Enemy");   //Gibt alle Enemies in current Szene
        targetsHit = new bool[targets.Length];                  //Create an array um zu merken welcher Gegner bereits gehittet wurde
        for (int i = 0; i < targetsHit.Length; i++){
            targetsHit[i] = false;                              //Am Anfang wurde noch niemand gehittet
            Debug.Log("Target: " + targets[i].name + " Distance: " + Vector3.Distance(transform.position,targets[i].transform.position));
        }

        //Setup Array that keeps track of Indices hit in order, to help with smoother animation
        targetIndices = new int[maxTargets];

        //Setup Line-Renderer, damit man auch visuell was sieht
        lr = gameObject.AddComponent<LineRenderer>();
        lr.startColor = lr.endColor = Color.cyan;
        lr.alignment = LineAlignment.View;
        lr.startWidth = lr.endWidth = 0.3f;
        lr.positionCount = 1;
        lr.numCapVertices = 3;
        lr.numCornerVertices = 3;
        lr.SetPosition(0, transform.position);  //Start at Cast Origin
        lr.sortingLayerName = "on floor";
        lr.material = new Material(Shader.Find("Sprites/Default"));
        
        //Soundeffekt abspielen
        AudioManager.getInstance().Play("Kettenblitz");

        //MainLoop starten
        StartCoroutine(mainLoop());
    }
    

    // Update the 0th position to stick to the caster, but only as long as the spell isn't finished.
    // Also update all other positions to stick to their respective enemies
    // Once the spell is finished, stop updating so the spell can properly disappear
    // Also check for NullPointer, as enemies since the hit happened may have died already
    void Update(){
        if (!spellFinished){
            lr.SetPosition(0, caster.transform.position);

            for (int i = 0; i < numOfHits; i++){
                GameObject enemyHit = targets[targetIndices[i]];
                if (enemyHit != null){
                    lr.SetPosition(i+1, enemyHit.transform.position);
                }
            }
        }
    }
    

    IEnumerator mainLoop(){
        while (numOfHits < maxTargets){
            Debug.Log(numOfHits);
            int n = getIndexOfClosestNotYetBeenHitEnemy(); 
            //Wenn n == int.MaxValue, dann haben wir keinen Gegner den wir hitten können
            if(n == int.MaxValue){
                break;
            }
            targetIndices[numOfHits] = n;
            drawLineToEnemy(n);
            hitEnemy(n);
            moveToEnemy(n);
            numOfHits++;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.1f);
        
        spellFinished = true;

        for (int i = 0; i < numOfHits; i++){
            deleteOldestLine();
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }

    void drawLineToEnemy(int targetIndex){
        GameObject target = targets[targetIndex];
        lr.positionCount = lr.positionCount + 1;
        lr.SetPosition(numOfHits+1, target.transform.position);
    }
    void hitEnemy(int targetIndex){
        GameObject enemy = targets[targetIndex];
        TestEnemy enemyScript = enemy.GetComponent<TestEnemy>();
        if(enemyScript == null){
            Debug.LogWarning("Kettenblitz hat einen Gegner gefunden, der nicht das erforderliche richtige Skript hat!");
            Debug.LogWarning("Alle Gegner mit den 'Enemy'-Tag müssen dieses Skript-Komponent haben. Wenn hier ein Fehler auftritt, bitte Moritz kontaktieren! :)");
            return;
        }
        enemyScript.takeDamage(DamageType.Blitz, damage);
        targetsHit[targetIndex] = true;
    }

    void moveToEnemy(int targetIndex){
        transform.position = targets[targetIndex].transform.position;
    }

    //Removes oldest Line by copying all other lines one step down
    void deleteOldestLine(){

        int newPositionCount = lr.positionCount - 1;
        for (int i = 0; i < newPositionCount; i++){
            lr.SetPosition(i, lr.GetPosition(i+1));
        }

        lr.positionCount = newPositionCount;
    }

    private int getIndexOfClosestNotYetBeenHitEnemy(){

        //Need to remember Distance and Index of closest Enemy
        //Wenn wir jemals 'int.MaxValue' Anzahl von Gegnern in unserem target-Array haben, 
        //haben wir ganz andere Probleme als dass dieser Code malfunctioned.
        float closestDistance = Mathf.Infinity;
        int closestIndex = int.MaxValue;

        for (int i = 0; i < targets.Length; i++) {
            //Check if target has already been hit, if yes, immediately continue.
            //Never hit same target twice.
            if (targetsHit[i]){
                continue;
            }
            //Check if current GameObject is closer than previous closest, if yes, remember new closest.
            GameObject g = targets[i];
            float currentDistance = Vector3.Distance(transform.position, g.transform.position);
            if (currentDistance < closestDistance && currentDistance < range){
                closestDistance = currentDistance;
                closestIndex = i;
            }
        }
        return closestIndex;

    }

}
