using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wasserfläche : MonoBehaviour

{
    public GameObject Wasserflaeche;

    void Start()
    {

        Wasserflaeche = Resources.Load<GameObject>("Prefabs/Wasserflaeche");        
        Instantiate(Wasserflaeche,  GameObject.FindGameObjectWithTag("Player").transform.position , Quaternion.identity);
    }
    
}
