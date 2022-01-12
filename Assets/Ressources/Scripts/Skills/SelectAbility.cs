using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAbility : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Startet");
    }
    public string name;
    void OnMouseDown()
    {
        Debug.Log(name);
        GameObject.Find("UI_Inventory").GetComponent<UI_Inventory>().selectAbility(name);
    }
}
