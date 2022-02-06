using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerOnAwakeIfStoryProgress : MonoBehaviour{
    
    public StoryProgress toCheck;
    public UnityEvent eventIfTrue;
    public UnityEvent eventIfFalse;

    void Start(){
        Progress p = Progress.getInstance();
        switch(toCheck){
            case StoryProgress.schluesselWeisheit:
                if(p.schluesselWeisheit){runTrue();}
                else{runFalse();}
                break;
            case StoryProgress.schluesselMacht:
                if(p.schluesselMacht){runTrue();}
                else{runFalse();}
                break;
            case StoryProgress.schluesselFrieden:
                if(p.schluesselFrieden){runTrue();}
                else{runFalse();}
                break;
        }
        Destroy(gameObject);
    }

    private void runTrue(){if(eventIfTrue != null){eventIfTrue.Invoke();}}
    private void runFalse(){if(eventIfFalse != null){eventIfFalse.Invoke();}}

    #region UsefulTriggerFunctions
    public void healPlayerHealth(int amount){
        Player.getInstance().addHealthPoints(amount);
    }

    public void damagePlayer(int amount){
        Player.getInstance().takeDamage(amount);
    }

    public void healPlayerMana(int amount){
        Player.getInstance().addMagicPoints(amount);
    }

    public void playMusic(string musicName){
        AudioManager.getInstance().PlayMusic(musicName);
    }

    public void stopMusic(){
        AudioManager.getInstance().StopMusic();
    }

    public void playSoundEffect(string soundName){
        AudioManager.getInstance().PlaySound(soundName);
    }
    #endregion
}

public enum StoryProgress{
    schluesselWeisheit,
    schluesselMacht,
    schluesselFrieden
}