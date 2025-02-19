using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebLoader : MonoBehaviour
{

    public GameObject web;
    public float interval = 1; // in seconds

    void Start()
    {
        InvokeRepeating("ShootWeb", interval, interval);
    }

    void ShootWeb()
    {
        GameObject go = Instantiate(web, transform.position, Quaternion.identity);
    }

}
