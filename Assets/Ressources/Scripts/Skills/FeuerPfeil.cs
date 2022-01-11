using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FeuerPfeil : MonoBehaviour
{
    public GameObject FireArrowPrefab;
    void Start()
    {
            FireArrowPrefab = Resources.Load<GameObject>("Prefabs/Skills/FireArrow");
            //var FireArrowPrefab = Resources.Load<GameObject>("Prefabs/FireArrow.Prefabs");
            Debug.Log(FireArrowPrefab);
            Instantiate(FireArrowPrefab, transform.position, Quaternion.identity);
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
