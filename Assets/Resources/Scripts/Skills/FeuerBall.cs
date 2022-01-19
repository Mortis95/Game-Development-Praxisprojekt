using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeuerBall : MonoBehaviour
{
    public GameObject FireBallPrefab;
    void Start()
    {
            FireBallPrefab = Resources.Load<GameObject>("Prefabs/Skills/FireBall");
            //var FireArrowPrefab = Resources.Load<GameObject>("Prefabs/FireArrow.Prefabs");
            Instantiate(FireBallPrefab, transform.position, Quaternion.identity);
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
