using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour
{

    public bool Boss3;
    public bool Boss2;
    public bool Boss1;
    public bool Endboss;

    public bool dungeon01Puzzle01;
    public bool dungeon01Puzzle02;
    public bool dungeon01Puzzle03;

    public bool schluesselWeisheit;
    public bool schluesselMacht;
    public bool schluesselFrieden;

    #region Singleton
    private static Progress instance;
    public static Progress getInstance(){
        return instance;
    }

    
    private void Awake(){
        if(instance != null){
            Debug.LogWarning("Something went wrong, 2 progress instances!!!");
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion
}
