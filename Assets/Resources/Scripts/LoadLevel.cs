using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{

    public string lvToLoad; //Name von dem nächsten level  
    public int x;           //xpos im neuen level
    public int y;           //ypos im neuen level
    public Animator transition;
    public float transitiontime=3f;
    public GameObject canvas;
    public string musicToPlay;

    void Start()
    {
        canvas.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionObject = collision.gameObject;

        if(collisionObject.tag =="Player")  //nur wenn en spieler und kein mob hereinläuft funzt es
        {
                GameManager.getInstance().pauseGame();
                StartCoroutine(Load(lvToLoad, collisionObject));
                
        }
    }


    IEnumerator Load(string name, GameObject go)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSecondsRealtime(transitiontime);

         SceneManager.LoadScene(name);
         GameManager.getInstance().unpauseGame();
         AudioManager.getInstance().PlayMusic(musicToPlay);

    }
}
