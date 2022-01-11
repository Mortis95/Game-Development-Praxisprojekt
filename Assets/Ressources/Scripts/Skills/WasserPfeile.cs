using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasserPfeile : MonoBehaviour
{
   public GameObject WasserPfeilePrefab;

    void Start()
    {

        WasserPfeilePrefab = Resources.Load<GameObject>("Prefabs/WasserPfeil");        
        Instantiate(WasserPfeilePrefab,  GameObject.FindGameObjectWithTag("Player").transform.position , Quaternion.identity);
    }
}
