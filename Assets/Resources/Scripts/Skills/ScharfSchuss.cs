using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScharfSchuss : MonoBehaviour
{
    public GameObject SnipePrefab;
    void Start()
    {
            SnipePrefab = Resources.Load<GameObject>("Prefabs/Skills/Snipe");
            //var FireArrowPrefab = Resources.Load<GameObject>("Prefabs/FireArrow.Prefabs");
            Instantiate(SnipePrefab, transform.position, Quaternion.identity);
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
