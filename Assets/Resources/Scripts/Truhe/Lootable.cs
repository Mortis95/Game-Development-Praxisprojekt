using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable : MonoBehaviour
{  
    public Item item;
    void Start(){
    }
    public void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject != null && col.tag == "Player"){
                GameObject OpenPrefab = Resources.Load<GameObject>("Prefabs/open");
                ItemDropController.createItemDropWithOffset(transform,item,1);
                Destroy(gameObject);
                Instantiate(OpenPrefab, transform.position, Quaternion.identity);
        }
    }    
}
