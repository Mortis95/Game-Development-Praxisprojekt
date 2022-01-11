using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{

    public string lvToLoad; //Name von dem nächsten level  
    public int x;           //xpos im neuen level
    public int y;           //ypos im neuen level

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionObject = collision.gameObject;

        if(collisionObject.tag =="Player")  //nur wenn en spieler und kein mob hereinläuft funzt es
        {
                loadScene();
                collisionObject.transform.position=new Vector3(x,y,0);
        }
    }

    public void loadScene() //extra funktion, falls mal animation und co dazukommen
    {
        Application.LoadLevel(lvToLoad);
    }
}
