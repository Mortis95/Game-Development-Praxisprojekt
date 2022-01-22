using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SingletonMitAwake
    private static GameManager instance;

    private void Awake(){
        if (instance != null){
            Debug.LogWarning("Two Instances of GameManager tried running!!! Second instance will self-destruct!");
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }

    public static GameManager getInstance(){
        return instance;
    }
    #endregion

    private bool gameIsPaused;
    private bool inventoryOpen;
    private bool dialogueActive;
    private bool skillTreeOpen;

    void Start(){
        gameIsPaused   = false;
        inventoryOpen  = false;
        dialogueActive = false;
        skillTreeOpen  = false;
    }

    public void pauseGame(){
        gameIsPaused = true;
        Time.timeScale = 0;
    }

    public void unpauseGame(){
        gameIsPaused = false;
        Time.timeScale = 1;
    }

    public bool getGameIsPaused(){
        return gameIsPaused;
    }

    public bool openInventory(){
        if(dialogueActive || skillTreeOpen || gameIsPaused){
            return false;
        }
        inventoryOpen = true;
        pauseGame();
        return true;
    }

    public void closeInventory(){
        inventoryOpen = false;
        unpauseGame();
    }

    public bool openSkillTree(){
        if(dialogueActive || inventoryOpen || gameIsPaused){
            return false;
        }
        skillTreeOpen = true;
        pauseGame();
        return true;
    }

    public void closeSkillTree(){
        skillTreeOpen = false;
        unpauseGame();
    }

    public bool startDialogue(){
        if(inventoryOpen || skillTreeOpen || gameIsPaused){
            return false;
        }
        dialogueActive = true;
        pauseGame();
        return true;
    }

    public void endDialogue(){
        dialogueActive = false;
        unpauseGame();
    }

}
