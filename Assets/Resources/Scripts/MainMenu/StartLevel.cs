using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour
{
    public void StartOnClick(){
        SceneManager.LoadScene("Anfangsgebiet(Wald)");
    }
}
